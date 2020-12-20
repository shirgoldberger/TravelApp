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
            bool enter = false;
            string pattern = "INSERT INTO member (trip_code, username) VALUES('" + trip_code + "', '";
            string endPattern = "');";
            string command = "";
            if(admin != null)
            {
                command = pattern + admin + endPattern;
                enter = true;
            }
            foreach (User participants in choosenParticipants)
            {
                if (enter)
                    command += " ";
                else
                    enter = true;
                command += pattern + participants.Username + endPattern;
            }
            return command;
        }

        private string addTripAttractionsCommand(string trip_code, List<Attraction> choosenAttractions)
        {
            string command = "INSERT INTO trip_attractions (trip_code, attraction_code) VALUES('" + trip_code + "', '" +
                    choosenAttractions[0].Attraction_code + "');";
            
            for (int i = 1; i < choosenAttractions.Count; i++)
            {
                Attraction attraction = choosenAttractions[i];
                command += " INSERT INTO trip_attractions (trip_code, attraction_code) VALUES('" + trip_code + "', '" +
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

        private string removeAttractionsCommand(string trip_code, List<Attraction> attractionsToRemove)
        {
            string attractionsString = "'" + attractionsToRemove[0].Attraction_code + "'";
            for(int i = 1; i < attractionsToRemove.Count; i++)
            {
                attractionsString += ", '" + attractionsToRemove[i].Attraction_code + "'";
            }
            string command = "DELETE FROM trip_attractions WHERE trip_code = '" + trip_code + "' AND attraction_code IN(" + attractionsString + ");";
            return command;
        }

        private string removeParticipantsCommand(string trip_code, List<User> participantsToRemove)
        {
            string participantsString = "'" + participantsToRemove[0].Username + "'";
            for (int i = 1; i < participantsToRemove.Count; i++)
            {
                participantsString += ", '" + participantsToRemove[i].Username + "'";
            }
            string command = "DELETE FROM member WHERE trip_code = '" + trip_code + "' AND username IN(" + participantsString + ");";
            return command;
        }

        private void appendCommand(ref bool change, ref string command)
        {
            if (change)
                command += ", ";
            else
                change = true;
        }

        private string convertDate(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        private string updateString(Trip inputedtrip, string tripName, int minAge, int maxAge, int maxParts, string startConverted, string endConverted, bool maleOnly, bool femaleOnly)
        {
            bool change = false;
            string command = "UPDATE trip SET ";
            if(minAge != inputedtrip.Min_Age)
            {
                command += "min_age = '" + minAge.ToString() + "'";
                change = true;
            }
            if(maxAge != inputedtrip.Max_Age)
            {
                appendCommand(ref change, ref command);
                command += "max_age = '" + maxAge.ToString() + "'";
            }
            if (maxParts != inputedtrip.Max_Participants)
            {
                appendCommand(ref change, ref command);
                command += "max_participants = '" + maxParts.ToString() + "'";
            }
            if (maleOnly != inputedtrip.Male_Only)
            {
                appendCommand(ref change, ref command);
                command += "male_only = '";
                command += maleOnly ? "1'" : "0'";
            }
            if (femaleOnly != inputedtrip.Female_Only)
            {
                appendCommand(ref change, ref command);
                command += "female_only = '";
                command += femaleOnly ? "1'" : "0'";
            }
            if (tripName != inputedtrip.Name)
            {
                appendCommand(ref change, ref command);
                command += "trip_name = '" + tripName + "'";
            }
            if (startConverted != convertDate(inputedtrip.Start_Date))
            {
                appendCommand(ref change, ref command);
                command += "start_date = '" + startConverted + "'";
            }
            if (endConverted != convertDate(inputedtrip.End_Date))
            {
                appendCommand(ref change, ref command);
                command += "end_date = '" + endConverted + "'";
            }
            return change ? command + " WHERE trip_code = '" + inputedtrip.Id.ToString() + "';" : "";
        }

        public void updateTrip(Trip inputedtrip, string tripName, int minAge, int maxAge, int maxParts, string startConverted, string endConverted, bool maleOnly, bool femaleOnly, List<User> partsToRemove, List<User> partsToAdd, List<Attraction> attractionsToRemove, List<Attraction> attractionsToAdd)
        {
            List<string> commands = new List<string>();

            if (partsToAdd.Count > 0)
            {
                commands.Add(addTripMembersCommand(null, inputedtrip.Id.ToString(), partsToAdd));
            }

            if (partsToRemove.Count > 0)
            {
                commands.Add(removeParticipantsCommand(inputedtrip.Id.ToString(), partsToRemove));
            }
            
            if (attractionsToAdd.Count > 0)
            {
                commands.Add(addTripAttractionsCommand(inputedtrip.Id.ToString(), attractionsToAdd));
            }
            
            if (attractionsToRemove.Count > 0)
            {
                commands.Add(removeAttractionsCommand(inputedtrip.Id.ToString(), attractionsToRemove));
            }

            string update_Command = updateString(inputedtrip, tripName, minAge, maxAge, maxParts, startConverted, endConverted, maleOnly, femaleOnly);
            if (update_Command != "")
            {
                commands.Add(update_Command);
            }

            lock (DbConnection.Locker)
            {
                DbConnection.ExecuteNonQueryTransaction(commands);
            }
        }
    }
}
