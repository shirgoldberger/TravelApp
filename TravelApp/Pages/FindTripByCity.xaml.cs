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
using TravelApp.Objects;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for FindByLocation.xaml
    /// </summary>
    public partial class FindTripByCity : Window, INotifyPropertyChanged
    {
        private FindTrip_Controller controller;
        private List<City> cities;
        private List<City> selectedCities;
        private List<string> countries;
        private string countryBegin;

        public FindTripByCity(FindTrip_Controller c)
        {
            InitializeComponent();
            endLoadCities();
            endLoadCountries();
            controller = c;
            countryBegin = "";
            bindCountries();
            selectedCities = new List<City>();
        }

        public List<City> Cities
        {
            get { return cities; }
        }
        public List<City> SelectedCities
        {
            get { return selectedCities; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }
        }

        private void find_Click(object sender, RoutedEventArgs e)
        {
            GetWindow(this).Close();
        }

        private async Task<Tuple<bool, List<string>>> getCountriesAsync(string begin)
        {
            Tuple<bool, List<string>> list = await Task.Run(() => controller.getCountriesByBegin(begin));
            return list;
        }

        private async void bindCountries()
        {
            startLoadCountries();
            Tuple<bool, List<string>> t = await getCountriesAsync(countryBegin);
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access countries records");
            }
            countries = t.Item2;
            countryBox.ItemsSource = countries;
            endLoadCountries();
        }

        private async Task<Tuple<bool, List<City>>> getCitiesAsync(string country)
        {
            Tuple<bool, List<City>> list = await Task.Run(() => controller.getCitiesByCountry(country, ""));
            return list;
        }

        private async void bindCities()
        {
            startLoadCities();
            string country = null;
            if (countryBox.SelectedIndex != -1)
            {
                country = countryBox.SelectedItem.ToString();
            }
            Tuple<bool, List<City>> t = await getCitiesAsync(country);
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access cities records");
            }
            cities = t.Item2;
            foreach (City c in cities)
            {
                if (selectedCities.Exists(x => x.Id == c.Id))
                {
                    c.Can_Choose = false;
                }
            }
            citiesList.ItemsSource = cities;
            endLoadCities();
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
        private void finishButton_Click(object sender, RoutedEventArgs e)
        {
            GetWindow(this).Close();
        }

        private void startLoadCities()
        {
            progressBarCities.IsIndeterminate = true;
            progressBarTextCities.Visibility = Visibility.Visible;
            progressBarCities.Visibility = Visibility.Visible;
        }

        private void endLoadCities()
        {
            progressBarCities.IsIndeterminate = false;
            progressBarTextCities.Visibility = Visibility.Hidden;
            progressBarCities.Visibility = Visibility.Hidden;
        }

        private void startLoadCountries()
        {
            progressBarCountries.IsIndeterminate = true;
            progressBarTextCountries.Visibility = Visibility.Visible;
            progressBarCountries.Visibility = Visibility.Visible;
        }

        private void endLoadCountries()
        {
            progressBarCountries.IsIndeterminate = false;
            progressBarTextCountries.Visibility = Visibility.Hidden;
            progressBarCountries.Visibility = Visibility.Hidden;
        }
    }
}