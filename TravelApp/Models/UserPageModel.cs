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
            bool exist = false;
            string command = "SELECT attraction_code FROM attraction WHERE name=" + name + " AND city_id=" + city_code + " AND type=" + type + ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                if (dr.Read())
                {
                    exist = true;
                }
                dr.Close();
            }
            return exist;
        }

        public List<string> getContinents(string begin)
        {
            List<string> continents = new List<string>();
            string command = "SELECT name FROM continent WHERE name LIKE '" + begin + "%';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    continents.Add(dr.GetString("name"));
                }
                dr.Close();
            }
            return continents;
        }

        public string getContinentByCountry(string country)
        {
            string continent = "";
            string command = "SELECT continent FROM country WHERE name='" + country + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    continent = dr.GetString("continent");
                }
                dr.Close();
            }
            return continent;
        }

        public List<string> getCountries(string begin)
        {
            List<string> countries = new List<string>();
            string command = "SELECT name FROM country WHERE name LIKE '" + begin + "%';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    countries.Add(dr.GetString("name"));
                }
                dr.Close();
            }
            return countries;
        }

        public List<City> getCitiesByContinent(string continent, string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT city_id, city.name, country, continent " +
                          "FROM city join country ON city.country = country.name " +
                          "WHERE city.name LIKE '" + begin + "%'";
            if (continent != null)
            {
                command = "SELECT * FROM city WHERE name LIKE '" + begin + "%'";
                string continentTemp = "'" + continent + "'";
                string countriesOptions = "SELECT name FROM country WHERE continent=" + continentTemp;
                command += " AND country IN (" + countriesOptions + ")";
            }
            command += ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string city = dr.GetString("name");
                    string country = dr.GetString("country");
                    string city_id = dr.GetString("city_id");
                    string continentArg = continent != null? continent : dr.GetString("continent");
                    City cityObj = new City(city_id, city, country, continentArg);
                    cities.Add(cityObj);
                }
                dr.Close();
            }
            return cities;
        }

        public List<City> getCitiesByCountry(string country, string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT * FROM city WHERE name LIKE '" + "%'";
            if (country != null)
            {
                country = "'" + country + "'";
                command += " AND country=" + country;
            }
            
            command += ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string city = dr.GetString("name");
                    country = dr.GetString("country");
                    string city_id = dr.GetString("city_id");
                    string continent = getContinentByCountry(country);
                    City cityObj = new City(city_id, city, country, continent);
                    cities.Add(cityObj);
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
