using Org.BouncyCastle.Pkix;
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
using System.Windows.Shapes;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for FindByLocation.xaml
    /// </summary>
    public partial class FindByLocation : Window, INotifyPropertyChanged
    {
        private FindTrip_Controller controller;
        private List<City> cities;
        private List<City> selectedCities;
        private List<Attraction> attractions;
        private List<Attraction> selectedAttractions;
        private List<string> countries;
        private string countryBegin;

        public FindByLocation(FindTrip_Controller c)
        {
            InitializeComponent();
            controller = c;
            countryBegin = "";
            bindCountries();
            selectedCities = new List<City>();
            selectedAttractions = new List<Attraction>();
        }

        public List<Attraction> Attractions
        {
            get { return attractions; }
        }
        public List<City> Cities
        {
            get { return cities; }
        }
        public List<City> SelectedCities
        {
            get { return selectedCities; }
        }
        public List<Attraction> SelectedAttractions
        {
            get { return selectedAttractions; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }
        }

        private void CheckBox_Checked_Attraction(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            Attraction currentAttraction = attractions.Find(x => x.Attraction_code == id);
            selectedAttractions.Add(currentAttraction);
        }

        private void CheckBox_Unchecked_Attraction(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            Attraction currentAttraction = attractions.Find(x => x.Attraction_code == id);
            selectedAttractions.Remove(currentAttraction);
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            GetWindow(this).Close();
        }

        private async Task<List<string>> getCountriesAsync(string begin)
        {
            List<string> list = await Task.Run(() => controller.getCountriesByBegin(begin));
            return list;
        }

        private async void bindCountries()
        {
            countries = await getCountriesAsync(countryBegin);
            countryBox.ItemsSource = countries;
        }

        private async Task<List<City>> getCitiesAsync(string country)
        {
            List<City> list = await Task.Run(() => controller.getCitiesByCountry(country, ""));
            return list;
        }

        private async void bindCities()
        {
            string country = null;
            if (countryBox.SelectedIndex != -1)
            {
                country = countryBox.SelectedItem.ToString();
            }
            cities = await getCitiesAsync(country);
            foreach (City c in cities)
            {
                if (selectedCities.Exists(x => x.Id == c.Id))
                {
                    c.Can_Choose = false;
                }
            }
            citiesList.ItemsSource = cities;

        }

        private void chooseClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = citiesList.Items.IndexOf(item);
            City c = cities[itemIndex];
            if (!selectedCities.Exists(x => x.Id == c.Id))
            {
                selectedCities.Add(c);
            }
            Button b = (Button)sender;
            b.IsEnabled = false;
        }

        private void countrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindCountries();
            bindCities();
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            countryBegin = countryBox.Text;
            bindCountries();
            countryBox.IsDropDownOpen = true;
        }

        private void cityBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
