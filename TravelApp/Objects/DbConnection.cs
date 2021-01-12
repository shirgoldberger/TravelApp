using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace TravelApp
{
    public class DbConnection
    {
        private static MySqlConnection _connection;
        private static Object my_lock;

        public static void createConnection(string user_id, string password, string name_database, int port)
        {
            if (_connection == null)
            {
                // setup new connection
                MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
                my_lock = new Object();
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
                } catch
                {
                    throw new Exception("Cannot connect to MySQL");
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

        public static Object Locker
        {
            get
            {
                return my_lock;
            }
        }

        public static bool ExecuteNonQueryTransaction(List<string> commands)
        {
            bool result = true;

            if (commands.Count == 0)
            {
                return result;
            }

            MySqlCommand myCommand = DbConnection.Connection.CreateCommand();
            MySqlTransaction myTrans;
            myTrans = DbConnection.Connection.BeginTransaction();
            myCommand.Connection = DbConnection.Connection;
            myCommand.Transaction = myTrans;

            try
            {
                foreach (string command in commands)
                {
                    myCommand.CommandText = command;
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

        public static bool ExecuteNonQuery(string command)
        {
            try
            {
                MySqlCommand cmd2 = DbConnection.Connection.CreateCommand();
                cmd2.CommandType = CommandType.Text;
                cmd2.CommandText = command;
                cmd2.ExecuteNonQuery();
                return true;
            }
            catch
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
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}
