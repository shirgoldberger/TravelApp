using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public class UserPageModel
    {
        public List<string> getRestUsers(string username, string beginning)
        {
            List<string> users = new List<string>();
            string noUser = "SELECT username FROM user WHERE username<>'" + username + "'";
            string userFriends1 = "SELECT username1 From friends WHERE username2='" + username + "'";
            string userFriends2 = "SELECT username2 From friends WHERE username1='" + username + "'";
            string userFriends = userFriends1 + " UNION " + userFriends2;
            string command = "SELECT username FROM ((" + noUser + ") as noUser LEFT JOIN (" + userFriends + ") as userFriends " +
                             "ON noUser.username=userFriends.username1) WHERE userFriends.username1 IS NULL AND username LIKE '" + beginning + "%';";

            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    users.Add(dr.GetString("username"));
                }
                dr.Close();
            }
            return users;
        }

        public bool addNewFriend(string user1, string user2)
        {
            user1 = "'" + user1 + "'";
            user2 = "'" + user2 + "'";
            string command = "INSERT INTO friends(username1, username2) VALUES (" + user1 + ", " + user2 + ");";
            bool dr = DbConnection.ExecuteNonQuery(command);
            return dr;
        }

        public string getCityCode(string country, string city)
        {
            string cityCode = "";
            city = "'" + city + "'";
            country = "'" + country + "'";
            string command = "SELECT city_id FROM city WHERE name=" + city + " AND country=" + country + ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    cityCode = dr.GetString("city_id");
                }
                dr.Close();
            }
            return cityCode;
        }

        public bool addNewAttraction(string name, string city_code, string type)
        {
            name = "'" + name + "'";
            city_code = "'" + city_code + "'";
            type = "'" + type + "'";
            string command = "INSERT INTO attration(name, city_id, type) VALUES (" + name + ", " + city_code + ", " + type + ");";
            bool dr = DbConnection.ExecuteNonQuery(command);
            return dr;
        }

        public bool attractionAlreadyExist(string name, string city_code, string type)
        {
            name = "'" + name + "'";
            city_code = "'" + city_code + "'";
            type = "'" + type + "'";
            string id = "";
            string command = "SELECT attraction_code FROM attraction WHERE name=" + name + " AND city_id=" + city_code + " AND type=" + type + ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    id = dr.GetString("attraction_code");
                }
                dr.Close();
            }
            return id != "";
        }

        public List<string> getContinents(string country, string begin)
        {
            List<string> continents = new List<string>();
            string search = "name";
            string command = "SELECT name FROM continent WHERE name LIKE '" + begin + "%'";
            if(country != null)
            {
                country = "'" + country + "'";
                command = "SELECT continent FROM country WHERE name=" + country + " AND continent LIKE '" + begin + "%'";
                search = "continent";
            }
            command += ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    continents.Add(dr.GetString(search));
                }
                dr.Close();
            }
            return continents;
        }

        public List<string> getCountries(string continent,string city, string begin)
        {
            List<string> countries = new List<string>();
            string command = "";
            string search = "name";
            if (city != null)
            {
                city = "'" + city + "'";
                command = "SELECT country FROM city WHERE name=" + city + " AND country LIKE '" + begin + "%'";
                search = "country";
            }
            else
            {
                command = "SELECT name FROM country WHERE name LIKE '" + begin + "%'";
                if (continent != null)
                {
                    continent = "'" + continent + "'";
                    command += " AND continent=" + continent;
                }
            }
            command += ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    countries.Add(dr.GetString(search));
                }
                dr.Close();
            }
            return countries;
        }

        public List<string> getCities(string continent, string country, string begin)
        {
            List<string> cities = new List<string>();
            string command = "SELECT name FROM city WHERE name LIKE '" + "%'";
            if (country != null)
            {
                country = "'" + country + "'";
                command += " AND country=" + country;
            }
            else if (continent != null)
            {
                continent = "'" + country + "'";
                string countriesOptions = "SELECT name FROM country WHERE continent=" + continent;
                command += " AND country IN (" + continent + ")";
            }
            command += ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    cities.Add(dr.GetString("name"));
                }
                dr.Close();
            }
            return cities;
        }

        public List<string> getTypes(string begin)
        {
            List<string> types = new List<string>();
            string command = "SELECT type FROM attraction_types WHERE type LIKE '" + begin + "%';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    types.Add(dr.GetString("type"));
                }
                dr.Close();
            }
            return types;
        }

    }
}
