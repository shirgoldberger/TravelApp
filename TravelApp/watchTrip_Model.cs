using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class watchTrip_Model
    {
        public Trip trip;
        public watchTrip_Model(Trip t)
        {
            this.trip = t;

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
            string command = "SELECT * FROM user;";
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
            string command = "SELECT * FROM attraction;";
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
