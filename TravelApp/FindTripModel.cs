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
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString() + ";";
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

        public List<Trip> findTripByAge(List<Language> languages)
        {
            List<Trip> trips = new List<Trip>();
            return trips;
        }
    }
}
