using K4os.Compression.LZ4.Internal;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using TravelApp.Objects;
using TravelApp.Pages;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for FindTrip.xaml
    /// </summary>
    public partial class FindTrip : Page
    {
        private FindTrip_Controller controller;
        private FindTripByCity fbc;
        private FindTripByAttraction ftba;
        private List<Trip> trips;
        private List<string> languages;
        private List<string> choosenLanguages;
        private List<City> choosenCities;
        private List<Attraction> choosenAttractions;
        private DateTime startDate_Selected;
        private DateTime endDate_Selected;
        private List<string> members;
        private List<string> choosenMembers;
        private string username;
        private string howToFilter;
        public FindTrip(string _username)
        {
            InitializeComponent();
            endLoadTrips();
            username = _username;
            controller = new FindTrip_Controller();;
            choosenLanguages = new List<string>();
            choosenMembers = new List<string>();
            choosenCities = new List<City>();
            choosenAttractions = new List<Attraction>();
            startDate_Selected = new DateTime();
            endDate_Selected = new DateTime();
            howToFilter = "";
            bindUsers();
            bindLanguages();
            bindTrips();
        }

        private async void bindUsers()
        {
            startLoadUsers();
            Tuple<bool, List<string>> t = await getFriendsAsync();
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access users records");
            }
            members = t.Item2;
            membersComboBox.ItemsSource = members;
            endLoadUsers();
        }

        private async void bindTrips()
        {
            startLoadTrips();
            Tuple<bool, List<Trip>> t = await getTripsAsync();
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access trips records");
            }
            trips = t.Item2;
            allTripsListBox.ItemsSource = trips;
            endLoadTrips();
        }

        private async void bindLanguages()
        {
            startLoadLanguages();
            Tuple<bool, List<string>> t = await getLanguagesAsync();
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access languages records");
            }
            languages = t.Item2;
            languagesComboBox.ItemsSource = languages;
            endLoadLanguages();
        }

        private async Task<Tuple<bool, List<string>>> getFriendsAsync()
        {
            Tuple<bool, List<string>> list = await Task.Run(() => controller.getFriendsForUser(this.username));
            return list;
        }
        private async Task<Tuple<bool, List<Trip>>> getTripsAsync()
        {
            Tuple<bool, List<Trip>> tuple = await Task.Run(() => controller.getTripForUser(this.username));
            if (!tuple.Item1)
            {
                return tuple;
            }
            List<Trip> trips = tuple.Item2;
            List<Trip> correctTrips = new List<Trip>();
            foreach (Trip t in trips)
            {
                if (t.Member_Amount < t.Max_Participants)
                {
                    correctTrips.Add(t);
                }
            }
            return new Tuple<bool, List<Trip>>(tuple.Item1, correctTrips);
        }
        private async Task<Tuple<bool, List<string>>> getLanguagesAsync()
        {
            Tuple<bool, List<string>> list = await Task.Run(() => controller.getLanguages());
            return list;
        }

        private void Languages_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = languages.Where(x => x.StartsWith(languagesComboBox.Text.Trim()));
            languagesComboBox.IsDropDownOpen = true;
        }

        private void Members_TextChanged(object sender, EventArgs e)
        {
            membersComboBox.ItemsSource = members.Where(x => x.StartsWith(membersComboBox.Text.Trim()));
            membersComboBox.IsDropDownOpen = true;
        }

        private void clickOnTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            Trip currentTrip = trips.Find(x => x.Id.ToString() == id);
            WatchTrip wt = new WatchTrip(currentTrip);
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

            }
            else
            {
                MessageBox.Show("Something happened. Registration for the trip failed");
            }
        }

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            if (howToFilter == "")
            {
                MessageBox.Show("Choose how to filter the trips");
                return;
            }
            int age = -1;
            if (ageText.Text.ToString() != "")
            {
                try
                {
                    age = int.Parse(ageText.Text.ToString());
                }
                catch
                {
                    MessageBox.Show("Please insert a valid age");
                    return;
                }
            }
            bindFindTrips(age, choosenMembers, choosenLanguages, choosenAttractions, choosenCities, startDate_Selected, endDate_Selected, howToFilter);
        }

        private async Task<Tuple<bool, List<Trip>>> findTripsAsync(int age, List<string> choosenMembers, List<string> choosenLanguages,
            List<Attraction> choosenAttractions, List<City> choosenCities,
            DateTime startDate_Selected, DateTime endDate_Selected, string howToFilter)
        {
            Tuple<bool, List<Trip>> list = await Task.Run(() => controller.FindTrip(username, age, choosenMembers, choosenLanguages,
                choosenAttractions, choosenCities, startDate_Selected,
                endDate_Selected, howToFilter));
            return list;
        }

        private async void bindFindTrips(int age, List<string> choosenMembers, List<string> choosenLanguages,
            List<Attraction> choosenAttractions, List<City> choosenCities,
            DateTime startDate_Selected, DateTime endDate_Selected, string howToFilter)
        {
            startLoadTrips();
            Tuple<bool, List<Trip>> t = await findTripsAsync(age, choosenMembers, choosenLanguages,
                choosenAttractions, choosenCities, startDate_Selected,
                endDate_Selected, howToFilter);
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access trips records");
            }
            trips = t.Item2;
            allTripsListBox.ItemsSource = null;
            allTripsListBox.ItemsSource = trips;
            endLoadTrips();
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

        private void chooseCities_Click(object sender, RoutedEventArgs e)
        {
            fbc = new FindTripByCity(controller);
            fbc.ShowDialog();
            choosenCities = fbc.SelectedCities;
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

        private void chooseAttractions_Click(object sender, RoutedEventArgs e)
        {
            ftba = new FindTripByAttraction(controller);
            ftba.ShowDialog();
            choosenAttractions = ftba.SelectedAttractions;
        }

        private void someTrips_Checked(object sender, RoutedEventArgs e)
        {
            allTrips.IsChecked = false;
            howToFilter = "some";

        }

        private void someTrips_Unchecked(object sender, RoutedEventArgs e)
        {
            allTrips.IsChecked = true;
            howToFilter = "all";
        }

        private void allTrips_Checked(object sender, RoutedEventArgs e)
        {
            someTrips.IsChecked = false;
            howToFilter = "all";
        }

        private void allTrips_Unchecked(object sender, RoutedEventArgs e)
        {
            someTrips.IsChecked = true;
            howToFilter = "some";
        }

        private void startLoadTrips()
        {
            progressBarTrips.IsIndeterminate = true;
            progressBarTextTrips.Visibility = Visibility.Visible;
            progressBarTrips.Visibility = Visibility.Visible;
        }

        private void endLoadTrips()
        {
            progressBarTrips.IsIndeterminate = false;
            progressBarTextTrips.Visibility = Visibility.Hidden;
            progressBarTrips.Visibility = Visibility.Hidden;

        }

        private void startLoadLanguages()
        {
            progressBarLanguages.IsIndeterminate = true;
            progressBarTextLanguages.Visibility = Visibility.Visible;
            progressBarLanguages.Visibility = Visibility.Visible;
        }

        private void endLoadLanguages()
        {
            progressBarLanguages.IsIndeterminate = false;
            progressBarTextLanguages.Visibility = Visibility.Hidden;
            progressBarLanguages.Visibility = Visibility.Hidden;
        }

        private void startLoadUsers()
        {
            progressBarFriends.IsIndeterminate = true;
            progressBarTextFriends.Visibility = Visibility.Visible;
            progressBarFriends.Visibility = Visibility.Visible;
        }

        private void endLoadUsers()
        {
            progressBarFriends.IsIndeterminate = false;
            progressBarTextFriends.Visibility = Visibility.Hidden;
            progressBarFriends.Visibility = Visibility.Hidden;

        }
    }
}
