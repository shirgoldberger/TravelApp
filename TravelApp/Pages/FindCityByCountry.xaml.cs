using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Models;
using TravelApp.Objects;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class FindCityByCountry : Window
    {
        private AttractionProperties_Controller controller;
        private List<string> countries;
        private List<City> cities;
        private string countryBegin;

        public City ReturenedCity { get;set;}

        public FindCityByCountry(AttractionProperties_Controller _controller)
        {
            controller = _controller;
            InitializeComponent();
            endLoadCities();
            endLoadCountries();
            countryBegin = "";
            bindCountries();
        }

        private async Task<List<string>> getCountriesAsync(string begin)
        {
            Tuple<bool, List<string>> tuple = await Task.Run(() => controller.getCountries(begin));
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying to access countries records");
            }
            List<string> list = tuple.Item2;
            return list;
        }

        private async void bindCountries()
        {
            startLoadCountries();
            countries = await getCountriesAsync(countryBegin);
            countryBox.ItemsSource = null;
            countryBox.ItemsSource = countries;
            endLoadCountries();
        }


        private async Task<List<City>> getCitiesAsync(string country)
        {
            Tuple<bool, List<City>> tuple = await Task.Run(() => controller.getCitiesByCountry(country, ""));
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying to access cities records");
            }
            List<City> list = tuple.Item2;
            return list;
        }

        private async void bindCities()
        {
            string country = null;
            if (countryBox.SelectedIndex != -1)
            {
                country = countryBox.SelectedItem.ToString();
            }
            startLoadCities();
            cities = await getCitiesAsync(country);
            citiesList.ItemsSource = null;
            citiesList.ItemsSource = cities;
            endLoadCities();
        }

        private void startLoadCities()
        {
            progressBar.IsIndeterminate = true;
            progressBarText.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Visible;
        }

        private void endLoadCities()
        {
            progressBar.IsIndeterminate = false;
            progressBarText.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Hidden;
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

        private void chooseClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = citiesList.Items.IndexOf(item);
            ReturenedCity = cities[itemIndex];
            GetWindow(this).Close();
        }

        private void countrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(countryBox.SelectedIndex != -1)
            {
                bindCities();
            }
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            countryBegin = countryBox.Text;
            bindCountries();
            countryBox.IsDropDownOpen = true;
        }
    }
}
