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
        FindTripModel findTrip_model;
        private FindByLocation fbl;
        List<Trip> trips;
        private List<String> languages;
        private List<String> choosenLanguages;
        private List<City> choosenCities;
        private List<Attraction> choosenAttractions;
        private DateTime startDate;
        private DateTime endDate;
        private List<String> members;
        private List<String> choosenMembers;
        string username;
        public FindTrip(string _username)
        {
            InitializeComponent();
            //tripDate.SelectedDate = DateTime.Now.AddDays(1);
            username = _username;
            findTrip_model = new FindTripModel();
            trips = findTrip_model.getTripForUser(username);
            allTripsListBox.ItemsSource = trips;
            joinTripListBox.ItemsSource = trips;
            choosenLanguages = new List<String>();
            languages = findTrip_model.getLanguages();
            languagesComboBox.ItemsSource = languages;
            members = findTrip_model.getFriendsForUser(this.username);
            membersComboBox.ItemsSource = members;
            choosenMembers = new List<string>();
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
            Trip currentTrip = trips.Find(x => x.Id == id);
            watchTrip wt = new watchTrip(currentTrip);
            wt.Show();

        }

        private void clickJoinTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            if (findTrip_model.insertUserToTrip(username, trips.Find(x => x.Id == id)))
            {
                MessageBox.Show("Your registration was successful");
            } else
            {
                MessageBox.Show("Something happened. Registration for the trip failed");
            }
        }

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            int age = 0;
            if (ageText.Text.ToString() != "")
            {
                age = int.Parse(ageText.Text.ToString());
                trips = findTrip_model.findTripByAge(age);
            }
            if (choosenLanguages.Count() != 0)
            {
                trips.AddRange(findTrip_model.findTripByLanguage(choosenLanguages));
            }
            if (choosenAttractions.Count() != 0)
            {
                trips.AddRange(findTrip_model.findTripByAttractions(choosenAttractions));
            }
            if (startDate != null && endDate != null)
            {

            }
            if (choosenMembers.Count() != 0)
            {
                trips.AddRange(findTrip_model.findTripByMember(choosenMembers));

            }
            allTripsListBox.ItemsSource = trips;
            joinTripListBox.ItemsSource = trips;
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
                fbl = new FindByLocation(findTrip_model);
            }
            fbl.Show();
            choosenCities = fbl.SelectedCities;
            choosenAttractions = fbl.SelectedAttractions; 
        }

        private void tripDate_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {

            var calendar = sender as Calendar;
            // ... See if a date is selected.
            if (calendar.SelectedDate.HasValue)
            {
                startDate = calendar.SelectedDates[0];
                int len = calendar.SelectedDates.Count();
                endDate = calendar.SelectedDates[len - 1];
            }
        }

        private void Button_Click_Reset(object sender, RoutedEventArgs e)
        {
            resetLanguages();
            resetMembers();
            ageText.Text = "";
            tripDate.SelectedDate = null;
            tripDate.DisplayDate = DateTime.Today;
        }
    }
}
