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

       

        public Tuple<bool, List<City>> getCitiesByBegin(string begin)
        {
            bool result = true;
            List<City> cities = new List<City>();
            string command = "SELECT * FROM city WHERE name LIKE '" + begin + "%';";
            lock (DbConnection.Locker)
            {
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
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<City>>(result, cities);
        }

        public Tuple<bool, List<string>> getContinents(string begin)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }

            return new Tuple<bool, List<string>>(result, continents);
        }


        public Tuple<bool, List<string>> getCountries(string begin)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }

            return new Tuple<bool, List<string>>(result, countries);
        }

        public Tuple<bool, List<City>> getCitiesByContinent(string continent, string begin)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }

            return new Tuple<bool, List<City>>(result, cities);
        }

        public Tuple<bool, List<City>> getCitiesByCountry(string country, string begin)
        {
            bool result = true;
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
                else
                {
                    result = false;
                }
            }

            return new Tuple<bool, List<City>>(result, cities);
        }


     
    }
}
    