using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public sealed class UsersModel
    {
        private static readonly UsersModel instance = new UsersModel();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static UsersModel()
        {
        }

        private UsersModel()
        {
        }

        public static UsersModel Instance
        {
            get
            {
                return instance;
            }
        }

        private void appendCommand(ref bool firstConstraint, ref string command)
        {
            if (firstConstraint)
            {
                command += " WHERE";
                firstConstraint = false;
            }
            else
            {
                command += " AND";
            }
        }

        public List<User> getRestFriends(List<User> drop, string username, int minAge, int maxAge, bool femaleOnly, bool maleOnly)
        {
            List<User> rest = new List<User>();
            List<Attraction> attractions = new List<Attraction>();
            bool firstConstarint = true;
            string friends1 = "SELECT username1 FROM friends WHERE username2 = '" + username + "'";
            string friends2 = "SELECT username2 FROM friends WHERE username1 = '" + username + "'";
            string friends = friends1 + " UNION ALL " + friends2;
            string command = "SELECT * FROM user JOIN (" + friends + ") AS temp ON username = temp.username1";
            if (minAge > 0)
            {
                command += " WHERE age >= '" + minAge.ToString() + "'";
                firstConstarint = false;
            }
            if (maxAge > 0)
            {
                appendCommand(ref firstConstarint, ref command);
                command += " age <= '" + maxAge.ToString() + "'";
            }
            if (femaleOnly)
            {
                appendCommand(ref firstConstarint, ref command);
                command += " is_male = '0'";
            }
            if (maleOnly)
            {
                appendCommand(ref firstConstarint, ref command);
                command += " is_male = '1'";
            }
            if (drop.Count > 0)
            {
                string dropIt = "'" + drop[0].Username + "'";
                for (int i = 1; i < drop.Count; i++)
                {
                    dropIt += ", '" + drop[i].Username + "'";
                }
                appendCommand(ref firstConstarint, ref command);
                command += " username NOT IN (" + dropIt + ")";
            }
            command += ";";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        string usernameArg = dr.GetString("username");
                        string password = dr.GetString("password");
                        string mail = dr.GetString("mail");
                        char is_male = dr.GetString("is_male")[0];
                        int age = int.Parse(dr.GetString("age"));
                        string phone = dr.GetString("phone");
                        rest.Add(new User(usernameArg, password, mail, is_male, age, phone));
                    }
                    dr.Close();
                }
            }
            return rest;
        }

        public List<string> getRestUsers(string username, string beginning)
        {
            List<string> users = new List<string>();
            string noUser = "SELECT username FROM user WHERE username<>'" + username + "'";
            string userFriends1 = "SELECT username1 From friends WHERE username2='" + username + "'";
            string userFriends2 = "SELECT username2 From friends WHERE username1='" + username + "'";
            string userFriends = userFriends1 + " UNION " + userFriends2;
            string command = "SELECT username FROM ((" + noUser + ") as noUser LEFT JOIN (" + userFriends + ") as userFriends " +
                             "ON noUser.username=userFriends.username1) WHERE userFriends.username1 IS NULL AND username LIKE '" + beginning + "%';";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        users.Add(dr.GetString("username"));
                    }
                    dr.Close();
                }
            }

            return users;
        }

        public bool addNewFriend(string user1, string user2)
        {
            user1 = "'" + user1 + "'";
            user2 = "'" + user2 + "'";
            string command = "INSERT INTO friends(username1, username2) VALUES (" + user1 + ", " + user2 + ");";
            bool dr;

            lock (DbConnection.Locker)
            {
                dr = DbConnection.ExecuteNonQuery(command);
            }

            return dr;
        }

        public User getUserByName(string username)
        {
            User result = null;
            string command = "SELECT * FROM user WHERE username = '" + username + "';";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        string password = dr.GetString("password");
                        string mail = dr.GetString("mail");
                        char is_male = dr.GetString("is_male")[0];
                        int age = int.Parse(dr.GetString("age"));
                        string phone = dr.GetString("phone");
                        result = new User(username, password, mail, is_male, age, phone);
                    }
                    dr.Close();
                }
            }

            return result;
        }
    }
}
