using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class FindTripModel
    {

        public Trip createTrip(MySqlDataReader dr)
        {
            string id = dr.GetString("trip_code");
            string admin = dr.GetString("admin");
            DateTime start_date = DateTime.Parse(dr.GetString("start_date"));
            DateTime end_date = DateTime.Parse(dr.GetString("end_date"));
            int min_age = int.Parse(dr.GetString("min_age"));
            int max_age = int.Parse(dr.GetString("max_age"));
            int max_participants = int.Parse(dr.GetString("max_participants"));
            bool male_only = dr.GetBoolean("male_only");
            bool female_only = dr.GetBoolean("female_only");

            Trip t = new Trip(id, admin, start_date, end_date, min_age,
                max_age, max_participants, male_only, female_only);
            return t;
        }
        public List<Trip> getTripsByCommand(string command)
        {
            List<Trip> trips = new List<Trip>();
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
            }
            dr.Close();
            return trips;
        }

        public List<Trip> getAllTrip()
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM trip;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            return trips;
        }

        public List<Trip> getTripForUser(string username)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * from trip WHERE trip_code NOT IN" +
                "(SELECT t.trip_code FROM trip INNER JOIN member AS t WHERE t.username = '"
                + username + "');";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            return trips;
        }

        public List<Trip> findTripByAge(int age)
        {
            string command = "SELECT * FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString() + ";";
            return getTripsByCommand(command);
        }

        public List<Language> getLanguages()
        {
            List<Language> languages = new List<Language>();
            string command = "SELECT * FROM Language;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string name = dr.GetString("name");
                    string id = dr.GetString("language_code");
                    Language l = new Language(id, name);
                    languages.Add(l);
                }
                dr.Close();
            }
            return languages;
        }

        public List<Trip> findTripByLanguage(List<Language> languages)
        {
            List<Trip> trips = new List<Trip>();
            string allLanguages = "";
            int i;
            for(i = 0; i < languages.Count; i++)
            {
                allLanguages += ("'" + languages[i].Name + "'");
                if (i != languages.Count - 1)
                {
                    allLanguages += ",";
                }
            }
            string createTable = "(select language.name, trip.trip_code" +
                " from user join user_languages join language join trip join member" +
                " where user.username = user_languages.username and" +
                " user.username = member.username and" +
                " trip.trip_code = member.trip_code and" +
                " language.language_code = user_languages.language_code) as Lan";
            string command = "select * from trip where trip_code in(select Lan.trip_code from "
                + createTable + " where name in (" + allLanguages + "));";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            return trips;
        }

        public List<Trip> findTripByMember(List<User> users)
        {
            List<Trip> trips = new List<Trip>();
            string allUsers = "";
            int i;
            for (i = 0; i < users.Count; i++)
            {
                allUsers += ("'" + users[i].Username + "'");
                if (i != users.Count - 1)
                {
                    allUsers += ",";
                }
            }
            string createTable = "(select Lan.trip_code " +
                "from(select user.username, trip.trip_code " +
                "from user join trip join member " +
                "where user.username = member.username " +
                "and trip.trip_code = member.trip_code) as Lan";
            string command = "select * from trip where trip_code in " + createTable
                + " where username in (" + allUsers + "));";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            return trips;
        }

        public Trip getTripById(string id)
        {
            Trip t = null;
            string command = "SELECT * FROM Trip WHERE trip_code = '" + id + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    t = createTrip(dr);
                }
                dr.Close();
            }
            return t;
        }

        public List<Trip> findTripByLocation()
        {
            List<Trip> trips = new List<Trip>();


            return trips;
        }

        public bool insertUserToTrip(string username, Trip trip)
        {
            string command = "insert into member values('"
                + trip.Id + "', '" + username + "');";
            if (DbConnection.ExecuteNonQuery(command))
            {
                trip.Member_Amount += 1;
                return true;
            }
            return false;
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

        public void AddMemberAmount(Trip t)
        {
            string command = "SELECT count(username) FROM member WHERE trip_code = '"
                + t.Id + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string count = dr.GetString("count(username)");
                    t.Member_Amount = int.Parse(count);
                }
                dr.Close();
            }
        }
        public List<Attraction> GetAttractionsByCities(List<City> cities)
        {
            List<Attraction> attractions = new List<Attraction>();
            string allCities = "";
            int i = 0;
            for(i = 0; i < cities.Count(); i++)
            {
                allCities += ("'" + cities[i].Id + "'");
                if (i != cities.Count - 1)
                {
                    allCities += ",";
                }
            }
            string command = "SELECT * FROM Attraction WHERE city_id IN(" + allCities + ");";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string attraction_code = dr.GetString("attraction_code");
                    string name = dr.GetString("name");
                    string type = dr.GetString("type");
                    string city_id = dr.GetString("city_id");
                    Attraction a = new Attraction(attraction_code, name, city_id, type);
                    attractions.Add(a);
                }
                dr.Close();
            }
            return attractions;
        }
    }
}


