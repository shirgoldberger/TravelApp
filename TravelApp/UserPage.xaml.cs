using System;
using System.Collections.Generic;
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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private string username;
        private string password;

        public UserPage(string _username, string _password)
        {
            DataContext = this;
            username = _username;
            password = _password;
            InitializeComponent();
            
        }

        public string Username
        {
            get
            {
                return username;
            }
        }

        private void viewTripsButton_Click(object sender, RoutedEventArgs e)
        {
            viewAllTrip vt = new viewAllTrip(username, password);
            this.NavigationService.Navigate(vt);
        }

        private void searchNewTripButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addNewTripButton_Click(object sender, RoutedEventArgs e)
        {
            addNewTrip nt = new addNewTrip(username, password);
            this.NavigationService.Navigate(nt);
        }
    }
}
