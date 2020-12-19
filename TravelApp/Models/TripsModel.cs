using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public sealed class TripsModel
    {
        private static readonly TripsModel instance = new TripsModel();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static TripsModel()
        {
        }

        private TripsModel()
        {
        }

        public static TripsModel Instance
        {
            get
            {
                return instance;
            }
        }

        public bool isExistTrip()
        {
            bool result = false;
            string command = "SELECT Count(*) as count FROM trip;";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        int count = int.Parse(dr.GetString("count"));
                        if (count > 0)
                        {
                            result = true;
                        }
                    }
                    dr.Close();
                }
            }

            return result;
        }

        private string createTripCommand(TripToAdd trip)
        {
            string name = trip.Name;
            string admin = trip.Admin;
            string start = trip.Start_Date;
            string end = trip.End_Date;
            string minAge = trip.Min_Age.ToString();
            string maxAge = trip.Max_Age.ToString();
            string maxParts = trip.Max_Participants.ToString();
            string male = trip.Male_Only ? "1" : "0";
            string female = trip.Female_Only ? "1" : "0";

            string command = "INSERT INTO trip(name, admin, start_date, end_date, min_age, max_age, max_participants, male_only, female_only) " +
                "VALUES ('" + name + "', '" + admin + "', '" + start + "', '" + end + "', '" + minAge + "', '" + maxAge + "', '" + maxParts +
                "', '" + male + "', '" + female + "');";

            return command;
        }

        private string addTripMembersCommand(string admin, string trip_code, List<User> choosenParticipants)
        {
            string pattern = "INSERT INTO member (trip_code, username) VALUES('" + trip_code + "', '";
            string endPattern = "');";
            string command = pattern + admin + endPattern;
            foreach (User participants in choosenParticipants)
            {
                command += " " + pattern + participants.Username + endPattern;
            }
            return command;
        }

        private string addTripAttractionsCommand(string trip_code, List<Attraction> choosenAttractions)
        {
            string command = "";
            bool first = true;
            foreach (Attraction attraction in choosenAttractions)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    command += " ";
                }
                command += "INSERT INTO trip_attractions (trip_code, attraction_code) VALUES('" + trip_code + "', '" +
                    attraction.Attraction_code + "');";
            }
            return command;
        }

        public void generateTrip(TripToAdd trip, List<User> choosenParticipants, List<Attraction> choosenAttractions)
        {
            lock (DbConnection.Locker)
            {
                MySqlCommand myCommand = DbConnection.Connection.CreateCommand();
                MySqlTransaction myTrans;
                myTrans = DbConnection.Connection.BeginTransaction();
                myCommand.Connection = DbConnection.Connection;
                myCommand.Transaction = myTrans;

                try
                {
                    string tripCommand = createTripCommand(trip);
                    myCommand.CommandText = tripCommand;
                    myCommand.ExecuteNonQuery();

                    /*
                    string trip_code = "";
                    myCommand.CommandText = "SELECT MAX(trip_code) AS max FROM trip;";
                    MySqlDataReader dr = myCommand.ExecuteReader();
                    if (dr != null)
                    {
                        while (dr.Read())
                        {
                            trip_code = dr.GetString("max");
                        }
                        dr.Close();
                    }
                    */

                    string trip_code = myCommand.LastInsertedId.ToString();

                    string membersCommand = addTripMembersCommand(trip.Admin, trip_code, choosenParticipants);
                    myCommand.CommandText = membersCommand;
                    myCommand.ExecuteNonQuery();

                    if (choosenAttractions.Count > 0)
                    {
                        string attractionCommand = addTripAttractionsCommand(trip_code, choosenAttractions);
                        myCommand.CommandText = attractionCommand;
                        myCommand.ExecuteNonQuery();
                    }

                    myTrans.Commit();
                }
                catch (Exception)
                {
                    try
                    {
                        myTrans.Rollback();
                    }
                    catch (MySqlException) { }
                }
            }
        }
    }
}
