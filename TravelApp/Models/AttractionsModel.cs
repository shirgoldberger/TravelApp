using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp.Models
{
    public sealed class AttractionsModel
    {
        private static readonly AttractionsModel instance = new AttractionsModel();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static AttractionsModel()
        {
        }

        private AttractionsModel()
        {
        }

        public static AttractionsModel Instance
        {
            get
            {
                return instance;
            }
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

        public bool addNewAttraction(string name, string city_code, string type)
        {
            bool success;
            name = "'" + name + "'";
            city_code = "'" + city_code + "'";
            type = "'" + type + "'";
            string command = "INSERT INTO attraction(name, city_id, type) VALUES (" + name + ", " + city_code + ", " + type + ");";

            lock (DbConnection.Locker)
            {
                success = DbConnection.ExecuteNonQuery(command);
            }

            return success;
        }

        public bool attractionAlreadyExist(string name, string city_code, string type)
        {
            name = "'" + name + "'";
            city_code = "'" + city_code + "'";
            type = "'" + type + "'";
            bool exist = false;
            string command = "SELECT attraction_code FROM attraction WHERE name=" + name + " AND city_id=" + city_code + " AND type=" + type + ";";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    if (dr.Read())
                    {
                        exist = true;
                    }
                    dr.Close();
                }
            }

            return exist;
        }

        private void appendCommand (ref bool firstConstraint, ref string command)
        {
            if(firstConstraint)
            {
                command += " WHERE";
                firstConstraint = false;
            }
            else
            {
                command += " AND";
            }
        }

        public List<Attraction> getAttractions(string cityId, string type, string name, List<Attraction> drop)
        {
            List<Attraction> attractions = new List<Attraction>();
            bool firstConstarint = true;
            string command = "SELECT * FROM attraction";
            if (cityId != "")
            {
                command += " WHERE city_id = '" + cityId + "'";
                firstConstarint = false;
            }
            if (type != "")
            {
                appendCommand(ref firstConstarint, ref command);
                command += " type = '" + type + "'";
            }
            if (name != "")
            {
                appendCommand(ref firstConstarint, ref command);
                command += " name = '" + name + "'";
            }
            if (drop.Count > 0)
            {
                string dropIt = "'" + drop[0].Attraction_code + "'";
                for (int i = 1; i < drop.Count; i++)
                {
                    dropIt += ", '" + drop[i].Attraction_code + "'";
                }
                appendCommand(ref firstConstarint, ref command);
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

        public List<string> getTypes(string begin)
        {
            List<string> types = new List<string>();
            string command = "SELECT type FROM attraction_types WHERE type LIKE '" + begin + "%';";

            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        types.Add(dr.GetString("type"));
                    }
                    dr.Close();
                }
            }

            return types;
        }

        public List<Attraction> getAttractionsByCity(City city, string begin)
        {
            List<Attraction> attractions = new List<Attraction>();
            string command = "SELECT * " +
                          "FROM city join attraction ON city.city_id = attraction.city_id " +
                          "WHERE city.name LIKE '" + begin + "%'";
            if (city != null)
            {
                string c = "'" + city.Name + "'";
                command += " AND city.name=" + c;
            }

            command += ";";
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string id = dr.GetString("attraction_code");
                        string name = dr.GetString("name");
                        string city_id = dr.GetString("city_id");
                        string type = dr.GetString("type");
                        Attraction a = new Attraction(id, name, city_id, type);
                        attractions.Add(a);
                    }
                    dr.Close();
                }
            return attractions;
        }
        public List<Attraction> getAllAttractionOfTrip(Trip trip)
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
