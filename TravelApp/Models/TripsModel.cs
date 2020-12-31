using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Crypto.Engines;
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



        private string createTripCommand(Trip trip)
        {
            string name = trip.Name;
            string admin = trip.Admin;
            string start = trip.StartString;
            string end = trip.EndString;
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

        public bool generateTrip(Trip trip, List<User> choosenParticipants, List<Attraction> choosenAttractions)
        {
            bool result = true;
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
                    result = false;
                    try
                    {
                        myTrans.Rollback();
                    }
                    catch (MySqlException) { }
                }
                return result;
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

        public bool updateTrip(Trip inputedtrip, string tripName, int minAge, int maxAge, int maxParts, string startConverted, string endConverted, bool maleOnly, bool femaleOnly, List<User> partsToRemove, List<User> partsToAdd, List<Attraction> attractionsToRemove, List<Attraction> attractionsToAdd)
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
                return DbConnection.ExecuteNonQueryTransaction(commands);
            }
        }

        public Tuple<bool, List<Trip>> FindTrip(string username, int age, List<string> members, List<string> languages, List<Attraction> attractions, List<City> cities, DateTime start, DateTime end, string op)
        {
            List<Trip> trips = new List<Trip>();
            string fullCommand = "";
            bool filterSelected = false;
            if (age != -1)
            {
                string ageCommand = "SELECT trip_code FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString();
                fullCommand = fullCommand + "trip_code IN(" + ageCommand + ")";
                filterSelected = true;
            }
            if (members.Count != 0)
            {
                string membersCommand = CreateMembersCommand(members);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + membersCommand + ")";
                filterSelected = true;
            }
            if (languages.Count != 0)
            {
                string languagesCommand = CreateLanguagesCommand(languages);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + languagesCommand + ")";
                filterSelected = true;
            }
            if (attractions.Count != 0)
            {
                string attractionsCommand = CreateAttractionsCommand(attractions);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + attractionsCommand + ")";
                filterSelected = true;
            }
            if (cities.Count != 0)
            {
                string citiesCommand = CreateCitiesCommand(cities);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + citiesCommand + ")";
                filterSelected = true;
            }
            if (checkDates(start, end))
            {
                string datesCommand = CreateDatesCommand(start, end);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + datesCommand + ")";
                filterSelected = true;
            }
            if (fullCommand == "")
            {
                return getTripsForUser(username, "not in");
            }
            else
            {
                fullCommand += ";";
                fullCommand = "SELECT distinct * FROM trip WHERE\n" + fullCommand;
            }
            return getTripsByCommand(fullCommand);
        }

        private string CreateMembersCommand(List<string> members)
        {
            string allUsers = "";
            int i;
            for (i = 0; i < members.Count; i++)
            {
                allUsers += ("'" + members[i] + "'");
                if (i != members.Count - 1)
                {
                    allUsers += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM member" +
                " WHERE username IN(" + allUsers + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + members.Count;
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        private string CreateLanguagesCommand(List<string> languages)
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
                " language.name = user_languages.language_name) as Lan";
            string command = "select trip_code from trip where trip_code in(select Lan.trip_code from "
                + createTable + " where name in (" + allLanguages + "))";
            return command;
        }

        private string CreateAttractionsCommand(List<Attraction> attractions)
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
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        private string CreateCitiesCommand(List<City> cities)
        {
            List<Trip> trips = new List<Trip>();
            string allCities = "";
            int i;
            for (i = 0; i < cities.Count; i++)
            {
                allCities += ("'" + cities[i].Id + "'");
                if (i != cities.Count - 1)
                {
                    allCities += ",";
                }
            }
            string innerTable = "SELECT trip_attractions.trip_code FROM trip_attractions join attraction" +
                " WHERE attraction.attraction_code = trip_attractions.attraction_code AND attraction.city_id IN(" + allCities + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + cities.Count;
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        private string CreateDatesCommand(DateTime start, DateTime end)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT trip_code FROM Trip WHERE start_date >= '" +
                start.ToString("yyyy-MM-dd") + "' AND end_date <= '" + end.ToString("yyyy-MM-dd") + "'";
            return command;
        }

        private bool checkDates(DateTime start, DateTime end)
        {
            if (start.ToString("yyyy-MM-dd") == "0001-01-01" &&
                end.ToString("yyyy-MM-dd") == "0001-01-01")
            {
                return false;
            }
            return true;
        }

        public bool AddMemberAmount(Trip t)
        {
            bool result = true;
            string command = "SELECT count(username) FROM member WHERE trip_code = '"
                + t.Id + "';";
            lock (DbConnection.Locker)
            {
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
                else
                {
                    result = false;
                }
            }
            return result;
        }
        public Tuple<bool,Trip> getTripById(string id)
        {
            bool result = true;
            Trip t = null;
            string command = "SELECT * FROM Trip WHERE trip_code = '" + id + "';";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        t = createTrip(dr);
                    }
                    dr.Close();
                }
                else
                {
                    result = false;
                }

            }
            return new Tuple<bool, Trip>(result, t);
        }

        private Trip createTrip(MySqlDataReader dr)
        {
            int id = int.Parse(dr.GetString("trip_code"));
            string name = dr.GetString("name");
            string admin = dr.GetString("admin");
            DateTime start_date = DateTime.Parse(dr.GetString("start_date"));
            DateTime end_date = DateTime.Parse(dr.GetString("end_date"));
            int min_age = int.Parse(dr.GetString("min_age"));
            int max_age = int.Parse(dr.GetString("max_age"));
            int max_participants = int.Parse(dr.GetString("max_participants"));
            bool male_only = dr.GetString("male_only")[0] == '1';
            bool female_only = dr.GetString("female_only")[0] == '1';

            Trip t = new Trip(id, name, admin, start_date, end_date, min_age,
                max_age, max_participants, male_only, female_only);
            return t;
        }

        private Tuple<bool, List<Trip>> getTripsByCommand(string command)
        {
            bool result = true;
            List<Trip> trips = new List<Trip>();
            lock (DbConnection.Locker)
            {
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
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<Trip>>(result, trips);
        }

        public bool insertUserToTrip(string username, Trip trip)
        {
            bool result = false;
            string command = "insert into member values('"
                + trip.Id + "', '" + username + "');";
            lock (DbConnection.Locker)
            {
                if (DbConnection.ExecuteNonQuery(command))
                {
                    trip.Member_Amount += 1;
                    result = true;
                }
            }
            return result;
        }

        public bool deleteTrip(Trip trip, string username)
        {
            bool result;
            string trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            username = "'" + username + "'";
            string command = "DELETE FROM member WHERE trip_code = " + trip_code + " AND username = " + username + ";";
            lock (DbConnection.Locker)
            {
                result = DbConnection.ExecuteNonQuery(command);
            }
            return result;
        }
        public bool delteAllTripMember(Trip trip)
        {
            bool result;
            string trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string command = "DELETE FROM member WHERE trip_code = " + trip_code + " ;";
            string command2 = "DELETE FROM trip_attractions WHERE trip_code = " + trip_code + " ;";
            string command3 = "DELETE FROM trip WHERE trip_code = " + trip_code + " ;";

            lock (DbConnection.Locker)
            {
                result = DbConnection.ExecuteNonQueryTransaction(new List<string> { command, command2, command3 });
            }
            return result;
        }

        public bool setUserAdmin(Trip trip, string username, string newUsername)
        {
            //first - set new admin
            string trip_code1 = "'" + trip.Id.ToString() + "'";
            string admin1 = "'" + newUsername + "'";
            string command = "UPDATE trip SET" +
                " admin = " + admin1 +
                " WHERE trip_code = " + trip_code1 + " ;";
            lock (DbConnection.Locker)
            {
                if(!DbConnection.ExecuteNonQuery(command))
                {
                    return false;
                }
            }
            //now delete the user from trip
            if (!deleteTrip(trip, username))
            {
                return false;
            }
            return true;
        }

        public Tuple<bool, List<string>> getAllMembersWithoutMe(Trip trip, string username)
        {
            bool result = true;
            List<string> users = new List<string>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string username1 = "'" + username + "'";
            string command = "SELECT username FROM member " +
                "WHERE member.trip_code = " + trip_code +
                "AND username != " + username1 + ";";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        users.Add(dr.GetString("username"));
                    }
                    dr.Close();
                }
                else
                {
                    result = false;
                }
            }
            
            return new Tuple<bool, List<string>>(result, users);
        }

        public Tuple<bool, List<Trip>> getAllTripForUser(string username)
        {
            bool result = true;
            string userName = "'" + username + "'";
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM trip, member " +
                "WHERE member.username = " + userName +
                " AND member.trip_code = trip.trip_code;";
            lock (DbConnection.Locker)
            {
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
                else
                {
                    result = false;
                }
            }
            return new Tuple<bool, List<Trip>>( result, trips);
        }

        public Tuple<bool, List<Trip>> getTripsForUser(string username, string op)
        {
            List<Trip> trips = new List<Trip>();
            string command = "select temp.trip_code, name, admin, start_date, end_date, min_age, max_age, max_participants, male_only, female_only, count(member2.trip_code) as participants_count " +
                "From member as member2 JOIN(SELECT * " +
                "FROM trip " +
                "where trip_code " + op + " (select trip_code as coded " +
                "From member " +
                "where username = '" + username + "')) as temp " +
                "where(member2.trip_code = temp.trip_code) " +
                "group by(member2.trip_code)";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        int trip_code = int.Parse(dr.GetString("trip_code"));
                        string name = dr.GetString("name");
                        string admin = dr.GetString("admin");
                        DateTime start_date = DateTime.Parse(dr.GetString("start_date"));
                        DateTime end_date = DateTime.Parse(dr.GetString("end_date"));
                        int min_age = int.Parse(dr.GetString("min_age"));
                        int max_age = int.Parse(dr.GetString("max_age"));
                        int max_participants = int.Parse(dr.GetString("max_participants"));
                        bool male_only = dr.GetString("male_only")[0] == '1';
                        bool female_only = dr.GetString("female_only")[0] == '1';
                        int amount = int.Parse(dr.GetString("participants_count"));
                        Trip t = new Trip(trip_code, name, admin, start_date, end_date, min_age, max_age, max_participants, male_only, female_only);
                        t.Member_Amount = amount;
                        trips.Add(t);
                    }
                    dr.Close();
                } else
                {
                    return new Tuple<bool, List<Trip>>(false, trips);
                }
            }
            return new Tuple<bool, List<Trip>>(true, trips);
        }

    }
}