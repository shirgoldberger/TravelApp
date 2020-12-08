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

        public Tuple<bool, string> createAccount(string username, string phone, string email, string password,
            string address, int stringAge, char is_male, List<String> friends, List<String> languages)
        {
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
                            friends[i] + "')");
                        if (i != friends.Count() - 1)
                        {
                            friendString += ", ";
                        }
                    }
                    command = "insert into Friends VALUES " + friendString + ";";
                    if (DbConnection.ExecuteNonQuery(command))
                    {
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
                        command = "insert into user_languages VALUES " + languageString + ";";
                        if (DbConnection.ExecuteNonQuery(command))
                        {
                            return new Tuple<bool, string>(true, "Create account success!");
                        }
                    }
                }
                else
                {
                    dr.Close();
                    return new Tuple<bool, string>(false, "Cannot create account, please try again");
                }
            }
            return new Tuple<bool, string>(false, "Cannot create account, please try again");
        }

        public List<string> getLanguages()
        {
            List<string> languages = new List<string>();
            string command = "SELECT * FROM Language;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string name = dr.GetString("name");
                    languages.Add(name);
                }
                dr.Close();
            }
            return languages;
        }

        public List<string> getUsers()
        {
            List<String> users = new List<String>();
            string command = "SELECT * FROM User;";
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
            return users;
        }
    }
}
