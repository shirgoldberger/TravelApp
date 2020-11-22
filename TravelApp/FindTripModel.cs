using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class FindTripModel
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
            bool male_only = dr.GetBoolean(dr.GetOrdinal("male_only"));
            bool female_only = dr.GetBoolean(dr.GetOrdinal("female_only"));

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
            }
            dr.Close();
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

        public bool insertUserToTrip(string username, string trip_code)
        {
            string command = "insert into member values('"
                + trip_code + "', '" + username + "');";
            return DbConnection.ExecuteNonQuery(command);
        }
    }
}


