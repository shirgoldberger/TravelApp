﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        public Tuple<bool, List<User>> getRestFriends(List<User> drop, string username, int minAge, int maxAge, bool femaleOnly, bool maleOnly)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<User>>(result, rest);
        }

        public Tuple<bool, List<String>> getRestUsers(string username, string beginning)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }

            return new Tuple<bool, List<string>>(result, users);
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

        public Tuple<bool, User> getUserByName(string username)
        {
            bool result_status = true;
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
                else
                {
                    result_status = false;
                }
            }
            return new Tuple<bool,User>(result_status, result);
        }

        public Tuple<bool, List<String>> getUsers()
        {
            bool result = true;
            List<String> users = new List<String>();
            string command = "SELECT * FROM User;";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string name = dr.GetString("username");
                        users.Add(name);
                    }
                    dr.Close();
                }
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<string>>(result, users);
        }

        //shir do lock
        public Tuple<bool, string> createAccount(string username, string phone, string email, string password, string passwordConfirm,
    string address, string stringAge, bool male_box, bool female_box, List<String> friends, List<String> languages)
        {
            if (username.Length > 30)
            {
                return new Tuple<bool, string>(false, "Please insert username with length < 30");
            }
            if (username.Length == 0)
            {
                return new Tuple<bool, string>(false, "Please insert username");
            }
            if (languages.Count == 0)
            {
                return new Tuple<bool, string>(false, "Please choose languages");
            }
            if (phone.Length != 10)
            {
                return new Tuple<bool, string>(false, "Please insert correct phone number");
            }
            if (!male_box && !female_box)
            {
                return new Tuple<bool, string>(false, "Choose Gender");
            }
            if (email.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter an email");
            }
            if (!Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                return new Tuple<bool, string>(false, "Enter a valid email");
            }
            int age;
            try
            {
                age = int.Parse(stringAge);
            }
            catch
            {
                return new Tuple<bool, string>(false, "Enter valid age");
            }
            char is_male;
            if (female_box)
            {
                is_male = '0';
            }
            else
            {
                is_male = '1';
            }
            if (password.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter password");
            }
            if (passwordConfirm.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter Confirm password");
            }
            if (password != passwordConfirm)
            {
                return new Tuple<bool, string>(false, "Confirm password must be same as password");
            }
            string command = "SELECT username FROM user WHERE username='" + username + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    dr.Close();
                    return new Tuple<bool, string>(false, "Username is already exist, try another username");
                }
                dr.Close();
                // create account
                string command1 = "insert into user (username,password,mail,is_male,age,phone) values('" + username + "','" + password + "','" + email + "','" + is_male + "'," + stringAge + ",'" + phone + "');";
                // insert friends here
                int i;
                string friendString = "";
                for (i = 0; i < friends.Count(); i++)
                {
                    friendString += ("('" + username + "', '" +
                        friends[i] + "')");
                    if (i != friends.Count() - 1)
                    {
                        friendString += ", ";
                    }
                }
                string command2 = "insert into Friends VALUES " + friendString + ";";
                // insert languages here
                string languageString = "";
                for (i = 0; i < languages.Count(); i++)
                {
                    languageString += ("('" + username + "', '" +
                        languages[i] + "')");
                    if (i != languages.Count() - 1)
                    {
                        languageString += ", ";
                    }
                }
                string command3 = "insert into user_languages VALUES " + languageString + ";";
                List<string> commands = new List<string>();
                commands.Add(command1);
                commands.Add(command2);
                commands.Add(command3);
                if (DbConnection.ExecuteNonQueryTransaction(commands))
                {
                    return new Tuple<bool, string>(true, "Create account success!");
                }
                else
                {
                    return new Tuple<bool, string>(false, "Cannot create account, please try again");
                }
            }
            return new Tuple<bool, string>(false, "Cannot create account, please try again");
        }

        private User createUser(MySqlDataReader dr)
        {
            string usernameArg = dr.GetString("username");
            string password = dr.GetString("password");
            string mail = dr.GetString("mail");
            char is_male = dr.GetString("is_male")[0];
            int age = int.Parse(dr.GetString("age"));
            string phone = dr.GetString("phone");
            User a = new User(usernameArg, password, mail, is_male, age, phone);
            return a;
        }

        public Tuple<bool, List<String>> getFriendsForUser(string username)
        {
            bool result = true;
            List<String> friends = new List<string>();
            string command = "SELECT * FROM Friends WHERE username1='" + username
                + "' OR username2='" + username + "';";
            lock (DbConnection.Locker)
            { 
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string name1 = dr.GetString("username1");
                        string name2 = dr.GetString("username2");
                        if (name1 != username)
                        {
                            friends.Add(name1);
                        }
                        else if (name2 != username)
                        {
                            friends.Add(name2);
                        }
                    }
                    dr.Close();
                }
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<string>>(result, friends);
        }
        public Tuple<int, string> login(string name, string password)
        {
            int result = 0;
            string result_msg = "";
            bool found = false;
            // check username
            string command = "SELECT password FROM user WHERE username='" + name + "';";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    
                    while (dr.Read())
                    {
                        found = true;
                        string password_ = dr.GetString("password");
                        if (password != password_)
                        {
                            result = 1;
                            result_msg = "This password doesnt match the entered username";
                        }
                    }
                    dr.Close();
                    if (!found)
                    {
                        result = 1;
                        result_msg = "Cannot find this account";
                    }
                } 
                else
                {
                    result = 2;
                    result_msg = "Error trying to access users records";
                }
                
               
            }
            return new Tuple<int, string>(result, result_msg);
        }
        public Tuple<bool, List<User>> getAllMembers(Trip trip, string username)
        {
            bool result = true;
            List<User> users = new List<User>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string username1 = "'" + username + "'";
            string command = "SELECT * FROM user WHERE username in (SELECT username FROM member" +
                    " WHERE trip_code =" + trip_code + ")" +
                       "AND username<>" + username1 + ";";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        User a = createUser(dr);
                        users.Add(a);
                    }
                }
                else
                {
                    result = false;
                }
                dr.Close();
            }
            return new Tuple<bool, List<User>>(result, users);
        }
        public Tuple<bool, List<string>> getAllMembersInTrip(Trip trip)
        {
            bool result = true;
            List<string> users = new List<string>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string command = "SELECT username FROM member " +
                "WHERE member.trip_code = " + trip_code + ";";
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
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<string>>(result, users);
        }
    }
}
