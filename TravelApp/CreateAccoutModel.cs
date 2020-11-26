using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class CreateAccoutModel
    {
        public CreateAccoutModel()
        {

        }

        public int createAccount(string username, string phone, string email, string password,
            string address, int stringAge, char is_male, List<User> friends)
        {
            string command = "SELECT username FROM user WHERE username='" + username + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    dr.Close();
                    return 1;
                }
                dr.Close();
                // create account
                command = "insert into user (username,password,mail,is_male,age,phone) values('" + username + "','" + password + "','" + email + "','" + is_male + "'," + stringAge + ",'" + phone + "');";
                if (DbConnection.ExecuteNonQuery(command))
                {
                    dr.Close();
                    // insert friends here
                    int i;
                    string friendString = "";
                    for (i = 0; i < friends.Count(); i++)
                    {
                        friendString += ("('" + username + "', '" +
                            friends[i].Username + "')");
                        if (i != friends.Count() - 1)
                        {
                            friendString += ", ";
                        }
                    }
                    return 0;
                }
                else
                {
                    dr.Close();
                    return 2;
                }
            }
            else
            {
                return 2;
            }
        }

        public List<Language> getLanguages()
        {
            List<Language> languages = new List<Language>();
            string command = "SELECT * FROM Language;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            while (dr.Read())
            {
                string name = dr.GetString("name");
                string id = dr.GetString("language_code");
                Language l = new Language(id, name);
                languages.Add(l);
            }
            dr.Close();
            return languages;
        }

        public List<User> getUsers()
        {
            List<User> users = new List<User>();
            string command = "SELECT * FROM User;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string name = dr.GetString("username");
                    User u = new User(name);
                    users.Add(u);
                }
                dr.Close();
            }
            return users;
        }
    }
}
