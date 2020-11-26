using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TravelApp
{
    public class viewAllTrip_Model
    {
        User user;
        public viewAllTrip_Model(User user)
        {
            this.user = user;
        }
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
            string userName = user.Username;
            userName = "'" + userName + "'";
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM trip, member " +
                "WHERE member.username = " + userName +
                " AND member.trip_code = trip.trip_code;";
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
            string command = "DELETE FROM member WHERE trip_code = "+trip_code+" AND username = "+username+";";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            DateTime start_date= trip.Start_Date;
            return true;
        }
        public bool delteAllTripMember(Trip trip)
        {
            string trip_code = trip.Id;
            trip_code = "'" + trip_code + "'";
            string command = "DELETE FROM member WHERE trip_code = " + trip_code +" ;";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            DateTime start_date = trip.Start_Date;
            return true;
        }
        public bool setUserAdmin(Trip trip, User user, string newUsername)
        {
            //first - set new admin
            string start_date1 = "'" + trip.Start_Date.ToString("dd.MM.yyyy") + "'";
            string end_date1 = "'" + trip.End_Date.ToString("dd.MM.yyyy") + "'";
            string min_age1 = "'" + trip.Min_Age.ToString() + "'";
            string max_age1 = "'" + trip.Min_Age.ToString() + "'";
            string max_part1 = "'" + trip.Max_Participants.ToString() + "'";
            string trip_code1 = "'" + trip.Id.ToString() + "'";
            string admin1 = "'" + newUsername + "'";
            string command = "UPDATE trip SET trip_code = " + trip_code1 +
                " , admin = " + admin1 + " , start_date = " + start_date1 +
                " , end_date = " + end_date1 + " , min_age = " + min_age1 +
                " , max_age = " + max_age1 + " , max_participants = " + max_part1 +
                " WHERE trip_code = " + trip_code1 + " ;";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            //now delete the user from trip.
            bool dr2 = deleteTrip(trip, user);
            if (dr2 == false)
            {
                return false;
            }
            DateTime start_date = trip.Start_Date;
            return true;
        
    }
    }
}
