using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : INotifyPropertyChanged
    {
        private string username = "";
        private string password = "";
        private string message = "";
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            Message = "";
            // if the user didn't insert username or password
            if (username.Equals("") || password.Equals(""))
            {
                Message = "You need to insert username and password";
                return;
            }
            //else
            //{
            //    viewAllTrip vt = new viewAllTrip();
            //    this.NavigationService.Navigate(vt);
            //}
            // Check the username & password 
            MySqlConnectionStringBuilder b = new MySqlConnectionStringBuilder();
            b.Server = "127.0.0.1";
            b.UserID = "root";
            b.Password = "123456";
            b.Database = "mydb";
            b.SslMode = MySqlSslMode.None;
            MySqlConnection connect = new MySqlConnection(b.ToString());




            //MySqlConnection connect = new MySqlConnection("server = localhost; database = TripApp; uid = root; pwd = 123456;");
            MySqlCommand cmd = new MySqlCommand("SELECT username, password FROM users WHERE username='" + name.Text + "' AND password='" + passwordBox.Text + "'");
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connect;
            connect.Open();
            try
            {
                MySqlDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    string username = dr.GetString("username");
                    string password = dr.GetString("password");
                    if (username == "NULL" || password == "NULL")
                    {
                        MessageBox.Show("cannot find this account");
                        return;
                    }
                    else
                    {
                        viewAllTrip vt = new viewAllTrip();
                        this.NavigationService.Navigate(vt);
                        dr.Close();
                        return;
                    }

                }
                MessageBox.Show("cannot find this account");
                dr.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connect.State == ConnectionState.Open)
                {
                    connect.Close();
                }
            }
        }
        private void Button_Click_New_Account(object sender, RoutedEventArgs e)
        {
            CreateAccount ca = new CreateAccount();
            this.NavigationService.Navigate(ca);
        }
        public string Username
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
                NotifyPropertyChanged("Ip");

            }
        }
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
                NotifyPropertyChanged("Port");
            }
        }
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                NotifyPropertyChanged("Message");
            }
        }


        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // close the program
            System.Environment.Exit(0);
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }
    }
}
