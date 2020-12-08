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
    public partial class AddNewAtt : Window
    {
        UserPageModel model;
        List<string> continents;
        List<string> countries;
        List<string> cities;
        List<string> types;
        private string continentBegin;
        private string countryBegin;
        private string cityBegin;
        private string typeBegin;
        public AddNewAtt(UserPageModel _model)
        {
            model = _model;
            InitializeComponent();
            continentBegin = "";
            countryBegin = "";
            cityBegin = "";
            typeBegin = "";
            refresh();
        }

        private void refresh()
        {
            bindContinents();
            bindCountries();
            bindCities();
            bindTypes();
        }

        private async Task<List<string>> getContinentsAsync(string country, string continentBegin)
        {
            List<string> list = await Task.Run(() => model.getContinents(country, countryBegin));
            return list;
        }

        private async void bindContinents()
        {
            string country = null;
            if(countryBox.SelectedIndex != -1)
            {
                country = countryBox.SelectedItem.ToString();
            }
            continents = await getContinentsAsync(country, continentBegin);
            continentBox.ItemsSource = continents;
            if(continentBox.Items.Count == 1)
            {
                continentBox.SelectedIndex = 0;
                continentBox.Text = continentBox.Items[0].ToString();
            }
        }

        private async Task<List<string>> getCountriesAsync(string continent, string city, string countryBegin)
        {
            List<string> list = await Task.Run(() => model.getCountries(continent, city, countryBegin));
            return list;
        }

        private async void bindCountries()
        {
            string continent = null;
            if (continentBox.SelectedIndex != -1)
            {
                continent = continentBox.SelectedItem.ToString();
            }
            string city = null;
            if (cityBox.SelectedIndex != -1)
            {
                city = cityBox.SelectedItem.ToString();
            }
            countries = await getCountriesAsync(continent, city, countryBegin);
            countryBox.ItemsSource = countries;
            if (countryBox.Items.Count == 1)
            {
                countryBox.SelectedIndex = 0;
                countryBox.Text = countryBox.Items[0].ToString();
            }
        }

        private async Task<List<string>> getCitiesAsync(string continent, string country, string cityBegin)
        {
            List<string> list = await Task.Run(() => model.getCities(continent, country, cityBegin));
            return list;
        }

        private async void bindCities()
        {
            string continent = null;
            if (continentBox.SelectedIndex != -1)
            {
                continent = continentBox.SelectedItem.ToString();
            }
            string country = null;
            if (countryBox.SelectedIndex != -1)
            {
                country = countryBox.SelectedItem.ToString();
            }
            cities = await getCitiesAsync(continent, country, cityBegin);
            cityBox.ItemsSource = cities;
        }

        private void bindTypes()
        {
            types = model.getTypes(typeBegin);
            typeBox.ItemsSource = types;
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            attrationName.Text = "";
            continentBox.Text = "";
            continentBox.SelectedIndex = -1;
            countryBox.Text = "";
            countryBox.SelectedIndex = -1;
            cityBox.Text = "";
            cityBox.SelectedIndex = -1;
            typeBox.Text = "";
            typeBox.SelectedIndex = -1;
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            if(attrationName.Text == "" || attrationName.Text.Length > 300 || typeBox.SelectedIndex == -1 ||    continentBox.SelectedIndex == -1 || countryBox.SelectedIndex == -1 || cityBox.SelectedIndex == -1)
            {
                MessageBox.Show("There is problem with arguments for creating your attraction");
                return;
            }
            string city = cityBox.Text;
            string country = countryBox.Text;
            string city_code = model.getCityCode(country, city);
            string name = attrationName.Text;
            string type = typeBox.Text;
            if(model.attractionAlreadyExist(name, city_code, type))
            {
                MessageBox.Show("This attraction is already exist");
                return;
            }
            bool result = model.addNewAttraction(name, city_code, type);
            string message = "Enter the attraction '" + name + "'";
            if(result)
            {
                message += " succeed";
            }
            else
            {
                message += " failed";
            }
            MessageBox.Show(message);
        }

        private void continentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindCountries();
            bindCities();
        }

        private void continentTextChanged(object sender, EventArgs e)
        {
            continentBegin = continentBox.Text;
            bindContinents();
        }

        private void citySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindCountries();
            bindContinents();
        }

        private void cityTextChanged(object sender, EventArgs e)
        {
            cityBegin = cityBox.Text;
            bindCities();
        }

        private void countrySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindContinents();
            bindCities();
        }

        private void countryTextChanged(object sender, EventArgs e)
        {
            countryBegin = countryBox.Text;
            bindCountries();
        }

        private void typeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void typeTextChanged(object sender, EventArgs e)
        {
            typeBegin = typeBox.Text;
            bindTypes();
        }
    }
}
