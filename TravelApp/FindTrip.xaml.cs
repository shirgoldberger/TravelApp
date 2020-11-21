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
        List<Trip> trips;
        private List<Language> languages;
        private List<Language> choosenLanguages;
        string username;
        public FindTrip(string _username)
        {
            InitializeComponent();
            username = _username;
            findTrip_model = new FindTripModel();
            trips = findTrip_model.getAllTrip();
            allTripsListBox.ItemsSource = trips;
            joinTripListBox.ItemsSource = trips;
            choosenLanguages = new List<Language>();
            languages = findTrip_model.getLanguages();
            languagesComboBox.ItemsSource = languages;
        }

        private void Languages_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = languages.Where(x => x.Name.StartsWith(languagesComboBox.Text.Trim()));
        }
        private void clickOnTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            Trip currentTrip = findTrip_model.getTripById(id);
            watchTrip wt = new watchTrip(currentTrip);
            wt.Show();
        }

        
        private void clickJoinTrip(object sender, RoutedEventArgs e)
        {
            string id = ((Button)sender).Uid.ToString();
            findTrip_model.insertUserToTrip(username, id);
        }

        private void Button_Click_Find(object sender, RoutedEventArgs e)
        {
            int age = 0;
            if (ageText.Text.ToString() != "")
            {
                age = int.Parse(ageText.Text.ToString());
            }
            trips = findTrip_model.findTripByAge(age);
            trips.AddRange(findTrip_model.findTripByLanguage(choosenLanguages));

            allTripsListBox.ItemsSource = trips;
            joinTripListBox.ItemsSource = trips;
            if (allTripsListBox.Items.Count == 0)
            {
                MessageBox.Show("There are no trips that match this search");
            }
        }

        private void MyCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var language = baseobj.DataContext as Language;
            BindListBox(language);
        }

        private void resetLanguages()
        {
            choosenLanguages.Clear();
            foreach (var language in languages)
            {
                language.Check_Status = false;
            }
            languagesComboBox.SelectedIndex = -1;
        }
        private void BindListBox(Language language)
        {
            if (language.Check_Status)
            {
                choosenLanguages.Add(language);
            }
            else
            {
                choosenLanguages.Remove(language);
            }
        }
    }
}
