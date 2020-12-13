using Org.BouncyCastle.Crypto.Tls;
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
    /// Interaction logic for FindTrip.xaml
    /// </summary>
    public partial class FindTrip : Page
    {
        FindTrip_Controller controller;
        private FindByLocation fbl;
        List<Trip> trips;
        private List<String> languages;
        private List<String> choosenLanguages;
        private List<City> choosenCities;
        private List<Attraction> choosenAttractions;
        private DateTime startDate_Selected;
        private DateTime endDate_Selected;
        private List<String> members;
        private List<String> choosenMembers;
        string username;
        public FindTrip(string _username)
        {
            InitializeComponent();
            username = _username;
            controller = new FindTrip_Controller();
            trips = controller.getTripForUser(username);
            allTripsListBox.ItemsSource = trips;
            choosenLanguages = new List<string>();
            languages = controller.getLanguages();
            languagesComboBox.ItemsSource = languages;
            members = controller.getFriendsForUser(this.username);
            membersComboBox.ItemsSource = members;
            choosenMembers = new List<string>();
            choosenCities = new List<City>();
            choosenAttractions = new List<Attraction>();
        }

        private void Languages_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = languages.Where(x => x.StartsWith(languagesComboBox.Text.Trim()));
        }
        
        private void Members_TextChanged(object sender, EventArgs e)
        {
            membersComboBox.ItemsSource = members.Where(x => x.StartsWith(languagesComboBox.Text.Trim()));
        }

        private void clickOnTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            Trip currentTrip = trips.Find(x => x.Id.ToString() == id);
            watchTrip wt = new watchTrip(currentTrip);
            wt.ShowDialog();

        }

        private void clickJoinTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            Trip t = trips.Find(x => x.Id.ToString() == id);
            if (controller.insertUserToTrip(username, t))
            {
                if (trips.Exists(x => x.Id.ToString() == id))
                {
                    trips.Remove(t);
                    allTripsListBox.ItemsSource = null;
                    allTripsListBox.ItemsSource = trips;
                    MessageBox.Show("Your registration was successful");
                }

            } else
            {
                MessageBox.Show("Something happened. Registration for the trip failed");
            }
        }

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            int age = -1;
            if (ageText.Text.ToString() != "")
            {
                age = int.Parse(ageText.Text.ToString());
            }
            List<Trip> t = controller.filterTrips(age, choosenLanguages, choosenAttractions, choosenMembers, startDate_Selected, endDate_Selected);
            allTripsListBox.ItemsSource = null;
            allTripsListBox.ItemsSource = t;
            if (allTripsListBox.Items.Count == 0)
            {
                MessageBox.Show("There are no trips that match this search");
            }
        }

        private void Checked_Member(object sender, RoutedEventArgs e)
        {
            string username = ((CheckBox)sender).Uid.ToString();
            if (!choosenMembers.Exists(x => x == username))
            {
                choosenMembers.Add(username);
            }
        }

        private void Unchecked_Member(object sender, RoutedEventArgs e)
        {
            string username = ((CheckBox)sender).Uid.ToString();
            if (choosenMembers.Exists(x => x == username))
            {
                choosenMembers.Remove(username);
            }
        }

        private void Checked_Language(object sender, RoutedEventArgs e)
        {
            string name = ((CheckBox)sender).Uid.ToString();
            if (!choosenLanguages.Exists(x => x == name))
            {
                choosenLanguages.Add(name);
            }
        }

        private void Unchecked_Language(object sender, RoutedEventArgs e)
        {
            string name = ((CheckBox)sender).Uid.ToString();
            if (choosenLanguages.Exists(x => x == name))
            {
                choosenLanguages.Remove(name);
            }
        }

        private void resetLanguages()
        {
            choosenLanguages.Clear();
            languagesComboBox.SelectedIndex = -1;
        }

        private void resetMembers()
        {
            choosenLanguages.Clear();
            languagesComboBox.SelectedIndex = -1;
        }

        private void filter_location_Click(object sender, RoutedEventArgs e)
        {
            if (fbl == null)
            {
                fbl = new FindByLocation(controller);
            }
            fbl.ShowDialog();
            choosenCities = fbl.SelectedCities;
            choosenAttractions = fbl.SelectedAttractions; 
        }

        private void tripDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            var calendar = sender as Calendar;
            // See if a date is selected.
            if (calendar.SelectedDate.HasValue)
            {
                startDate_Selected = calendar.SelectedDates[0];
                int len = calendar.SelectedDates.Count();
                endDate_Selected = calendar.SelectedDates[len - 1];
            }
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            resetLanguages();
            resetMembers();
            ageText.Text = "";
            tripDate.SelectedDates.Clear();
            tripDate.DisplayDate = DateTime.Today;
            allTripsListBox.ItemsSource = trips;
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
