using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class DbConnection
    {
        private static MySqlConnection _connection;
        public static MySqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    // setup new connection
                    MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
                    b.Server = "127.0.0.1";
                    b.UserID = "root";
                    b.Password = "123456";
                    b.Database = "mydb";
                    b.SslMode = MySqlSslMode.None;
                    _connection = new MySqlConnection(b.ToString());
                    _connection.Open();
                }
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
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                return dr;
            } catch
            {
                return null;
            }
        }
    }
}
