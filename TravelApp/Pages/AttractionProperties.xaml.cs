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
    public partial class AttractionProperties : Window
    {
        private UserPageModel model;
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


        public AttractionProperties(UserPageModel _model, bool _creation)
        {
            model = _model;
            creation = _creation;
            InitializeComponent();
            reset();
        }

        private async Task<List<City>> getCitiesAsync(string begin)
        {
            List<City> list = await Task.Run(() => model.getCitiesByContinent(null, begin));
            return list;
        }

        private async void bindCities()
        {
            cities = await getCitiesAsync(cityBegin);
            cityBox.ItemsSource = cities;
        }

        private async Task<List<string>> getTypesAsync(string begin)
        {
            List<string> list = await Task.Run(() => model.getTypes(begin));
            return list;
        }

        private async void bindTypes()
        {
            types = await getTypesAsync(typeBegin);
            typeBox.ItemsSource = types;
        }

        private void reset()
        {
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
                cityBox.Text = _city.Name;
            }            
        }

        private void findByContinent(object sender, RoutedEventArgs e)
        {
            FindCityByContinent fbc = new FindCityByContinent(model);
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
            FindCityByCountry fbc = new FindCityByCountry(model);
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
        }

        private void filterTypeByText(object sender, RoutedEventArgs e)
        {
            typeBegin = typeBox.Text;
            bindTypes();
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            reset();
        }
    }
}
