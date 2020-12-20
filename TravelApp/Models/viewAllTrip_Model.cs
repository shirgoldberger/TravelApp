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
        string username;
        public viewAllTrip_Model(string _username)
        {
            username = _username;
        }
        public Trip createTrip(MySqlDataReader dr)
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
        public Attraction createAtt(MySqlDataReader dr)
        {
            string attraction_code = dr.GetString("attraction_code");
            string name = dr.GetString("name");
            string city_id = dr.GetString("city_id");
            string type = dr.GetString("type");
            Attraction a = new Attraction(attraction_code, name, city_id, type);
            return a;
        }
        public User createUser(MySqlDataReader dr)
        {
            string usernameArg = dr.GetString("username");
            string password = dr.GetString("password");
            string mail = dr.GetString("mail");
            char is_male = dr.GetString("is_male")[0];
            int age = int.Parse(dr.GetString("age"));
            string phone = dr.GetString("phone");
            User a = new User(usernameArg,password,mail,is_male, age, phone);
            return a;
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
            string trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            username = "'" + username + "'";
            string command = "DELETE FROM member WHERE trip_code = " + trip_code + " AND username = " + username + ";";
            bool dr = DbConnection.ExecuteNonQuery(command);
            if (dr == false)
            {
                return false;
            }
            DateTime start_date = trip.Start_Date;
            return true;
        }
        public bool delteAllTripMember(Trip trip)
        {
            string trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            // Transaction:
            //defenition for transaction
            MySqlCommand myCommand = DbConnection.Connection.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = DbConnection.Connection.BeginTransaction();
            myCommand.Connection = DbConnection.Connection;
            myCommand.Transaction = myTrans;
            try
            {
                //add the command to send
                //1
                string command = "DELETE FROM member WHERE trip_code = " + trip_code + " ;";
                myCommand.CommandText = command;
                myCommand.ExecuteNonQuery();
                //2
                string command2 = "DELETE FROM trip_attractions WHERE trip_code = " + trip_code + " ;";
                myCommand.CommandText = command2;
                myCommand.ExecuteNonQuery();
                //3
                string command3 = "DELETE FROM trip WHERE trip_code = " + trip_code + " ;";
                myCommand.CommandText = command3;
                myCommand.ExecuteNonQuery();

                myTrans.Commit();
            }
            catch (Exception e)
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
            string command = "UPDATE trip SET" +
                " admin = " + admin1 +
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
        public List<string> getAllMembersWithoutMe( Trip trip){
            List<string> users = new List<string>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string username1 = "'" + username + "'";
            string command = "SELECT username FROM member " +
                "WHERE member.trip_code = " + trip_code +
                "AND username != " + username1 + ";";

            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    users.Add(dr.GetString("username"));
                }
            }
            dr.Close();
            return users;
        }
        public List<User> getAllMembers(Trip trip)
        {
            List<User> users = new List<User>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string username1 = "'" + username + "'";
            string command = "SELECT * FROM user WHERE username in (SELECT username FROM member" +
                    " WHERE trip_code =" + trip_code + ")" +
                       "AND username<>" + username1 + ";";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    User a = createUser(dr);
                    users.Add(a);
                }
            }
            dr.Close();
            return users;
        }
        public List<Attraction> getAllAttraction(Trip trip)
        {
            List<Attraction> att = new List<Attraction>();
            String trip_code = trip.Id.ToString();
            trip_code = "'" + trip_code + "'";
            string command = "SELECT * FROM attraction, trip_attractions " +
                "WHERE trip_attractions.trip_code = " + trip_code + " " +
                "AND trip_attractions.attraction_code = attraction.attraction_code;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Attraction a = createAtt(dr);
                    att.Add(a);
                }
            }
            dr.Close();
            return att;
        }
    }
}
