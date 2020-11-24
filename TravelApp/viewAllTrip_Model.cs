using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TravelApp
{
    class viewAllTrip_Model
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
        public bool deleteTrip(Trip trip, User user)
        {
            string trip_code = trip.Id;
            trip_code = "'" + trip_code + "'";
            string username = user.Username;
            username = "'" + username + "'";
            string command = "DELETE FROM member WHERE trip_code = "+trip_code+"AND username = "+username+";";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            DateTime start_date= trip.Start_Date;
            return true;
        }
    }
}
