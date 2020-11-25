using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class editTheTrip_Model
    {
        Trip trip;

        public editTheTrip_Model( Trip trip ) {
            this.trip = trip;
        }
        public User createUser(MySqlDataReader dr)
        {
            string username = dr.GetString("username");
            string password = dr.GetString("password");
            User u = new User(username, password);
            return u;
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
        public List<User> getAllMembers()
        {
            List<User> users = new List<User>();
            String trip_code = trip.Id;
            trip_code = "'" + trip_code + "'";
            string command = "SELECT * FROM user, member " +
                "WHERE member.trip_code = " + trip_code +
                " AND member.username = user.username;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    User u = createUser(dr);
                    users.Add(u);
                }
            }
            dr.Close();
            return users;
        }
        public List<Attraction> getAllAttraction()
        {
            List<Attraction> att = new List<Attraction>();
            String trip_code = trip.Id;
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
    

        public bool update_submit(string trip_code, string admin, string start_date, string end_date, string min_age, string max_age, string max_part)
        {
            string start_date1 = "'" + start_date + "'";
            string end_date1 = "'" + end_date + "'";
            string min_age1 = "'" + min_age + "'";
            string max_age1 = "'" + max_age + "'";
            string max_part1 = "'" + max_part + "'";
            string trip_code1 = "'" + trip.Id + "'";
            string admin1 = "'" + admin + "'";
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
            return true;
        }
       
    }
}

