using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public sealed class LocationsModel
    {
        private static readonly LocationsModel instance = new LocationsModel();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static LocationsModel()
        {
        }

        private LocationsModel()
        {
        }

        public static LocationsModel Instance
        {
            get
            {
                return instance;
            }
        }

        public string getCityCode(string country, string city)
        {
            string cityCode = "";
            city = "'" + city + "'";
            country = "'" + country + "'";
            string command = "SELECT city_id FROM city WHERE name=" + city + " AND country=" + country + ";";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        cityCode = dr.GetString("city_id");
                    }
                    dr.Close();
                }
            }

            return cityCode;
        }

        public List<City> getCitiesByBegin(string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT * FROM city WHERE name LIKE '" + begin + "%';";
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string id = dr.GetString("city_id");
                        string name = dr.GetString("name");
                        string country = dr.GetString("country");
                        City c = new City(id, name, country);
                        cities.Add(c);
                    }
                    dr.Close();
                }
            return cities;
        }

        public List<string> getContinents(string begin)
        {
            List<string> continents = new List<string>();
            string command = "SELECT name FROM continent WHERE name LIKE '" + begin + "%';";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        continents.Add(dr.GetString("name"));
                    }
                    dr.Close();
                }
            }

            return continents;
        }

        public string getContinentByCountry(string country)
        {
            string continent = "";
            string command = "SELECT continent FROM country WHERE name='" + country + "';";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        continent = dr.GetString("continent");
                    }
                    dr.Close();
                }
            }

            return continent;
        }

        public List<string> getCountries(string begin)
        {
            List<string> countries = new List<string>();
            string command = "SELECT name FROM country WHERE name LIKE '" + begin + "%';";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        countries.Add(dr.GetString("name"));
                    }
                    dr.Close();
                }
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

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string city = dr.GetString("name");
                        string country = dr.GetString("country");
                        string city_id = dr.GetString("city_id");
                        string continentArg = continent != null ? continent : dr.GetString("continent");
                        City cityObj = new City(city_id, city, country, continentArg);
                        cities.Add(cityObj);
                    }
                    dr.Close();
                }
            }

            return cities;
        }

        public List<City> getCitiesByCountry(string country, string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT city_id, city.name, country, continent " +
                          "FROM city join country ON city.country = country.name " +
                          "WHERE city.name LIKE '" + begin + "%'";
            if (country != null)
            {
                country = "'" + country + "'";
                command += " AND country=" + country;
            }
            command += ";";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string city = dr.GetString("name");
                        string countryArg = dr.GetString("country");
                        string city_id = dr.GetString("city_id");
                        string continent = dr.GetString("continent");
                        City cityObj = new City(city_id, city, countryArg, continent);
                        cities.Add(cityObj);
                    }
                    dr.Close();
                }
            }

            return cities;
        }

        public List<City> getAllCities()
        {
            List<City> cities = new List<City>();
            string command = "SELECT * FROM City JOIN Country WHERE City.country=Country.name;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string id = dr.GetString("city_id");
                    string name = dr.GetString("name");
                    string country = dr.GetString("country");
                    string continent = dr.GetString("continent");
                    City c = new City(id, name, country, continent);
                    cities.Add(c);
                }
                dr.Close();
            }
            return cities;

        }

     
    }
}
    