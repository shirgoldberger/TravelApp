using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    }
}
