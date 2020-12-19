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
using System.Windows.Shapes;
using TravelApp.Models;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class FindCityByCountry : Window
    {
        AttractionProperties_Controller controller;
        List<string> countries;
        List<City> cities;
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
            List<string> list = await Task.Run(() => controller.getCountries(begin));
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
