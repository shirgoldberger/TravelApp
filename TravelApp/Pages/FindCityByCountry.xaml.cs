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
            citiesList.ItemsSource = cities;
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
            bindCountries();
            bindCities();
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            countryBegin = countryBox.Text;
            bindCountries();
            countryBox.IsDropDownOpen = true;
        }
    }
}
