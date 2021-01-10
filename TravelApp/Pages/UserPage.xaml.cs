using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelApp.Pages;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private string username;
        private UserPage_Controller controller;

        public UserPage(string _username)
        {
            DataContext = this;
            username = _username;
            InitializeComponent();
            helloStr.Text = "Hello " + username;
            controller = new UserPage_Controller();
        }

        private void viewTripsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewAllTrip vt = new ViewAllTrip(username);
            NavigationService.Navigate(vt);
        }

        private void searchNewTripButton_Click(object sender, RoutedEventArgs e)
        {
            FindTrip vt = new FindTrip(username);
            NavigationService.Navigate(vt);
        }

        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewFriend nf = new AddNewFriend(username, controller);
            nf.ShowDialog();
        }

        private void addNewTripButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewTrip nt = new AddNewTrip(username);
            NavigationService.Navigate(nt);
        }

        private void addAttractionButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewAtt at = new CreateNewAtt(controller);
            at.ShowDialog();
        }
        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
