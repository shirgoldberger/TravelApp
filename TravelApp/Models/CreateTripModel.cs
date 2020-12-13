using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelApp.Models
{
    public class CreateTripModel
    {


        public User getUserByName(string username)
        {
            User result = null;
            string command = "SELECT * FROM user WHERE username = '" + username + "';";
            lock(DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        string password = dr.GetString("password");
                        string mail = dr.GetString("mail");
                        char is_male = dr.GetString("is_male")[0];
                        int age = int.Parse(dr.GetString("age"));
                        string phone = dr.GetString("phone");
                        result = new User(username, password, mail, is_male, age, phone);
                    }
                    dr.Close();
                }
            }
            
            return result;
        }

        public List<Attraction> getAttractions(string cityId, string type, string name, List<Attraction> drop)
        {
            List<Attraction> attractions = new List<Attraction>();
            bool firstConstarint = true;
            string command = "SELECT * FROM attraction";
            if(cityId != "")
            {
                command += " WHERE city_id = '" + cityId + "'";
                firstConstarint = false;
            }
            if(type != "")
            {
                if (firstConstarint)
                {
                    command += " WHERE";
                    firstConstarint = false;
                }
                else
                {
                    command += " AND";
                }
                command += " type = '" + type + "'";
            }
            if(name != "")
            {
                if (firstConstarint)
                {
                    command += " WHERE";
                    firstConstarint = false;
                }
                else
                {
                    command += " AND";
                }
                command += " name = '" + name + "'";
            }
            if(drop.Count > 0)
            {
                string dropIt = "'" + drop[0].Attraction_code + "'";
                for(int i = 1; i < drop.Count; i++)
                {
                    dropIt += ", '" + drop[i].Attraction_code + "'";
                }
                if (firstConstarint)
                {
                    command += " WHERE";
                }
                else
                {
                    command += " AND";
                }
                command += " attraction_code NOT IN (" + dropIt + ")";
            }
            command += ";";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        string att_code_arg = dr.GetString("attraction_code");
                        string nameArg = dr.GetString("name");
                        string typeArg = dr.GetString("type");
                        string city_id_Arg = dr.GetString("city_id");
                        attractions.Add(new Attraction(att_code_arg, nameArg, city_id_Arg, typeArg));
                    }
                    dr.Close();
                }
            }
                
            return attractions;
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


        public List<User> getRestFriends(List<User> drop, string username, int minAge, int maxAge, bool femaleOnly, bool maleOnly)
        {
            List<User> rest = new List<User>();
            List<Attraction> attractions = new List<Attraction>();
            bool firstConstarint = true;
            string friends1 = "SELECT username1 FROM friends WHERE username2 = '" + username + "'";
            string friends2 = "SELECT username2 FROM friends WHERE username1 = '" + username + "'";
            string friends = friends1 + " UNION ALL " + friends2;
            string command = "SELECT * FROM user JOIN (" + friends + ") AS temp ON username = temp.username1";
            if (minAge > 0)
            {
                command += " WHERE age >= '" + minAge.ToString() + "'";
                firstConstarint = false;
            }
            if (maxAge > 0)
            {
                if (firstConstarint)
                {
                    command += " WHERE";
                    firstConstarint = false;
                }
                else
                {
                    command += " AND";
                }
                command += " age <= '" + maxAge.ToString() + "'";
            }
            if (femaleOnly)
            {
                if (firstConstarint)
                {
                    command += " WHERE";
                    firstConstarint = false;
                }
                else
                {
                    command += " AND";
                }
                command += " is_male = '0'";
            }
            if(maleOnly)
            {
                if (firstConstarint)
                {
                    command += " WHERE";
                    firstConstarint = false;
                }
                else
                {
                    command += " AND";
                }
                command += " is_male = '1'";
            }
            if (drop.Count > 0)
            {
                string dropIt = "'" + drop[0].Username + "'";
                for (int i = 1; i < drop.Count; i++)
                {
                    dropIt += ", '" + drop[i].Username + "'";
                }
                if (firstConstarint)
                {
                    command += " WHERE";
                }
                else
                {
                    command += " AND";
                }
                command += " username NOT IN (" + dropIt + ")";
            }
            command += ";";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {

                    while (dr.Read())
                    {
                        string usernameArg = dr.GetString("username");
                        string password = dr.GetString("password");
                        string mail = dr.GetString("mail");
                        char is_male = dr.GetString("is_male")[0];
                        int age = int.Parse(dr.GetString("age"));
                        string phone = dr.GetString("phone");
                        rest.Add(new User(usernameArg, password, mail, is_male, age, phone));
                    }
                    dr.Close();
                }
            }
            return rest;
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
