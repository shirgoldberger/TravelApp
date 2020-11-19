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

            // Check the username & password 
            string command = "SELECT username, password FROM user WHERE username='" + name.Text + "' AND password='" + passwordBox.Text + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string username = dr.GetString("username");
                    string password = dr.GetString("password");
                    dr.Close();
                    UserPage up = new UserPage(username, password);
                    this.NavigationService.Navigate(up);
                    
                    return;
                }
                dr.Close();
                MessageBox.Show("cannot find this account");
            } else
            {
                MessageBox.Show("Error, problem with find this account");
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
