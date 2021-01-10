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
    public partial class AttractionProperties : Window
    {
        private AttractionProperties_Controller controller;
        private List<City> cities;
        private List<string> types;
        private string cityBegin;
        private string typeBegin;
        private string city_code;
        private bool creation;

        public string AttractionReturned { set; get; }
        public string ContinentReturned { set; get; }
        public string CountryReturned { set; get; }
        public string CityReturned { set; get; }
        public string TypeReturned { set;get; }
        public string City_code { set; get; }


        public AttractionProperties(bool _creation)
        {
            controller = new AttractionProperties_Controller();
            creation = _creation;
            InitializeComponent();
            reset();
        }

        private async Task<List<City>> getCitiesAsync(string begin)
        {
            Tuple <bool, List<City>> tuple = await Task.Run(() => controller.getCitiesByContinent(null, begin));
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access cities recods");
            }
            List<City> list = tuple.Item2;
            return list;
        }

        private async void bindCities()
        {
            startLoadCities();
            cities = await getCitiesAsync(cityBegin);
            cityBox.ItemsSource = cities;
            endLoadCities();
        }

        private async Task<List<string>> getTypesAsync(string begin)
        {
            Tuple<bool, List<string>> tuple = await Task.Run(() => controller.getTypes(begin));
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying to access attraction types records");
            }
            List<string> list = tuple.Item2;
            return list;
        }

        private async void bindTypes()
        {
            startLoadTypes();
            types = await getTypesAsync(typeBegin);
            typeBox.ItemsSource = types;
            endLoadTypes();
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

        private void startLoadTypes()
        {
            progressBarTypes.IsIndeterminate = true;
            progressBarTextTypes.Visibility = Visibility.Visible;
            progressBarTypes.Visibility = Visibility.Visible;
        }

        private void endLoadTypes()
        {
            progressBarTypes.IsIndeterminate = false;
            progressBarTextTypes.Visibility = Visibility.Hidden;
            progressBarTypes.Visibility = Visibility.Hidden;
        }

        private void reset()
        {
            endLoadCities();
            endLoadTypes();
            attrationName.Text = "";
            continentName.Text = "";
            countryName.Text = "";
            cityName.Text = "";
            cityBox.Text = "";
            cityBox.SelectedIndex = -1;
            typeBox.Text = "";
            typeBox.SelectedIndex = -1;
            cityBegin = "";
            typeBegin = "";
            city_code = "";
            bindCities();
            bindTypes();
        }

        public City CitySelected { set; get; }

        private void setClick(object sender, RoutedEventArgs e)
        {
            if(creation)
            {
                if (attrationName.Text == "" || typeBox.SelectedIndex == -1 || typeBox.SelectedIndex == -1 ||
                    cityName.Text == "")
                {
                    MessageBox.Show("There is problem with arguments for creating your attraction");
                    return;
                }
                TypeReturned = types[typeBox.SelectedIndex];
                ContinentReturned = continentName.Text;
                CountryReturned = countryName.Text;
                CityReturned = cityName.Text;
            }
            else
            {
                if(typeBox.SelectedIndex != -1)
                {
                    TypeReturned = types[typeBox.SelectedIndex];
                }
                else
                {
                    TypeReturned = "";
                }
            }
            AttractionReturned = attrationName.Text;
            City_code = city_code;
            GetWindow(this).Close();

        }

        private void citySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cityBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                City _city = cities[selectedIndex];
                changeFieldsByCity(_city);
                CitySelected = _city;
            }            
        }

        private void findByContinent(object sender, RoutedEventArgs e)
        {
            FindCityByContinent fbc = new FindCityByContinent(controller);
            fbc.ReturenedCity = null;
            fbc.ShowDialog();
            City returnedCity = fbc.ReturenedCity;
            if (returnedCity != null)
            {
                changeFieldsByCity(returnedCity);
            }
        }

        private void findByCountry(object sender, RoutedEventArgs e)
        {
            FindCityByCountry fbc = new FindCityByCountry(controller);
            fbc.ReturenedCity = null;
            fbc.ShowDialog();
            City returnedCity = fbc.ReturenedCity;
            if (returnedCity != null)
            {
                changeFieldsByCity(returnedCity);
            }
            
        }

        private void changeFieldsByCity(City _city)
        {
            cityName.Text = _city.Name;
            continentName.Text = _city.Continent;
            countryName.Text = _city.Country;
            city_code = _city.Id;
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            cityBegin = cityBox.Text;
            bindCities();
            cityBox.IsDropDownOpen = true;
        }

        private void filterTypeByText(object sender, RoutedEventArgs e)
        {
            typeBegin = typeBox.Text;
            bindTypes();
            typeBox.IsDropDownOpen = true;
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            reset();
        }
    }
}
