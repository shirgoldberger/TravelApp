﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;


namespace TravelApp
{
    public class viewAllTrip_Model
    {
        string username;
        public viewAllTrip_Model(string _username)
        {
            username = _username;
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
            string userName = "'" + username + "'";
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
        public bool deleteTrip(Trip trip, string username)
        {
            string trip_code = trip.Id;
            trip_code = "'" + trip_code + "'";
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
            command = "DELETE FROM trip_attractions WHERE trip_code = " + trip_code + " ;";
            bool dr2 = DbConnection.ExecuteNonQuery(command);
            if (dr2 == false)
            {
                return false;
            }
            command = "DELETE FROM trip WHERE trip_code = " + trip_code + " ;";
            bool dr3 = DbConnection.ExecuteNonQuery(command);
            if (dr3 == false)
            {
                return false;
            }
            return true;
        }
        public bool setUserAdmin(Trip trip, string username, string newUsername)
        {
            //first - set new admin
            string trip_code1 = "'" + trip.Id.ToString() + "'";
            string admin1 = "'" + newUsername + "'";
            string command = "UPDATE trip SET"+
                " admin = " + admin1  +
                " WHERE trip_code = " + trip_code1 + " ;";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            //now delete the user from trip.
            bool dr2 = deleteTrip(trip, username);
            if (dr2 == false)
            {
                return false;
            }
            DateTime start_date = trip.Start_Date;
            return true;
        
    }
    }
}
