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
    public partial class FindCityByContinent : Window
    {
        AttractionProperties_Controller controller;
        List<string> continents;
        List<City> cities;
        private string continentBegin;

        public City ReturenedCity { get; set; }

        public FindCityByContinent(AttractionProperties_Controller _controller)
        {
            controller = _controller;
            InitializeComponent();
            continentBegin = "";
            bindContinents();
            bindCities();
        }

        private async Task<List<string>> getContinentsAsync(string begin)
        {
            List<string> list = await Task.Run(() => controller.getContinents(begin));
            return list;
        }

        private async void bindContinents()
        {
            continents = await getContinentsAsync(continentBegin);
            continentBox.ItemsSource = continents;
        }


        private async Task<List<City>> getCitiesAsync(string continent)
        {
            List<City> list = await Task.Run(() => controller.getCitiesByContinent(continent, ""));
            return list;
        }

        private async void bindCities()
        {
            string continent = null;
            if (continentBox.SelectedIndex != -1)
            {
                continent = continentBox.SelectedItem.ToString();
            }
            cities = await getCitiesAsync(continent);
            citiesList.ItemsSource = cities;
        }

        private void chooseClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = citiesList.Items.IndexOf(item);
            ReturenedCity = cities[itemIndex];
            GetWindow(this).Close();
        }

        private void continentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindContinents();
            bindCities();
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            continentBegin = continentBox.Text;
            bindContinents();
            continentBox.IsDropDownOpen = true;
        }
    }
}
