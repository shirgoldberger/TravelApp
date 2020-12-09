using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class DbConnection
    {
        private static MySqlConnection _connection;

        public static void createConnection(string user_id, string password, string name_database, int port)
        {
            if (_connection == null)
            {
                // setup new connection
                MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
                b.Server = "127.0.0.1";
                b.UserID = user_id;
                b.Password = password;
                b.Database = name_database;
                b.SslMode = MySqlSslMode.None;
                b.Port = (uint)port;
                _connection = new MySqlConnection(b.ToString());
                try
                {
                    _connection.Open();
                } catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public static MySqlConnection Connection
        {
            get
            {
                return _connection;
            }
        }

        public static bool ExecuteNonQuery(string command)
        {
            try
            {
                MySqlCommand cmd2 = DbConnection.Connection.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = command;
                cmd2.ExecuteNonQuery();
                return true;
            } catch
            {
                return false;
            }
        }
        public static MySqlDataReader ExecuteQuery(string command)
        {
            MySqlCommand cmd = DbConnection.Connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = command;
            try
            {
                MySqlDataReader dr = cmd.ExecuteReader();
                return dr;
            } catch(Exception e)
            {
                return null;
            }
        }
    }
}
