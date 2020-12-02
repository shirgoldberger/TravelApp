using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TravelApp
{
    public class Home_page_Model
    {
        public Home_page_Model() { }
        public string login(string name,string password)
        {
            // check username
            string command = "SELECT username FROM user WHERE username='" + name + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    // Check the username & password 
                    command = "SELECT username FROM user WHERE username='" + name + "' AND password='" + password + "';";
                    dr.Close();
                    MySqlDataReader dr2 = DbConnection.ExecuteQuery(command);
                    if (dr2 != null)
                    {
                        while (dr2.Read())
                        {
                            string username = dr2.GetString("username");
                            dr2.Close();
                            return username;
                        }
                        dr2.Close();
                        throw new Exception("The password does not match this username");
                    }
                    else
                    {
                        throw new Exception("The password does not match this username");
                    }
                }
                dr.Close();
                throw new Exception("cannot find this account");
            }
            throw new Exception("cannot find this account");
        }
    }
}
