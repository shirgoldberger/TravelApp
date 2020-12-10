using Microsoft.Win32;
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
            foreach (Trip t in trips) {
                AddMemberAmount(t);
            }
            return trips;
        }

        public List<String> getLanguages()
        {
            List<String> languages = new List<String>();
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

        public List<String> getFriendsForUser(string username)
        {
            List<String> friends = new List<string>();
            string command = "SELECT * FROM Friends WHERE username1='" + username
                + "' OR username2='" + username + "';";
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
            return friends;
        }

        public List<Trip> findTripByDates(DateTime startDate, DateTime endDate)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM Trip WHERE start_date >= '" +
                startDate.ToString("yyyy-MM-dd") + "' AND end_date <= '" + endDate.ToString("yyyy-MM-dd") + "';";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByAttractions(List<Attraction> attractions)
        {
            List<Trip> trips = new List<Trip>();
            string allAttractions = "";
            int i;
            for (i = 0; i < attractions.Count; i++)
            {
                allAttractions += ("'" + attractions[i].Attraction_code + "'");
                if (i != attractions.Count - 1)
                {
                    allAttractions += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM trip_attractions" +
                " WHERE attraction_code IN(" + allAttractions + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + attractions.Count;
            string command = "SELECT * FROM Trip WHERE trip_code IN(" +
                innerTable + ");";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByLanguage(List<String> languages)
        {
            List<Trip> trips = new List<Trip>();
            string allLanguages = "";
            int i;
            for (i = 0; i < languages.Count; i++)
            {
                allLanguages += ("'" + languages[i] + "'");
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
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByMember(List<String> users)
        {
            List<Trip> trips = new List<Trip>();
            string allUsers = "";
            int i;
            for (i = 0; i < users.Count; i++)
            {
                allUsers += ("'" + users[i] + "'");
                if (i != users.Count - 1)
                {
                    allUsers += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM member" +
                " WHERE username IN(" + allUsers + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + users.Count;
            string command = "SELECT * FROM Trip WHERE trip_code IN(" +
                innerTable + ");";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByAge(int age)
        {
            string command = "SELECT * FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString() + ";";
            return getTripsByCommand(command);
        }
    }
}


