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
    public partial class AddNewAttForTrip : Window
    {
        UserPageModel UPmodel;
        List<City> cities;
        List<string> types;
        private string cityBegin;
        private string typeBegin;
        private string city_code;
        private List<Attraction> attractions;
        private CreateTripModel CTmodel;
        private List<Attraction> choosed;

        public Attraction ReturenedAttraction { set; get; }


        public AddNewAttForTrip(UserPageModel _UPmodel, CreateTripModel _CTmodel, List<Attraction> _choosed)
        {
            UPmodel = _UPmodel;
            choosed = _choosed;
            CTmodel = _CTmodel;
            InitializeComponent();
            reset();
        }

        private async Task<List<City>> getCitiesAsync(string begin)
        {
            List<City> list = await Task.Run(() => UPmodel.getCitiesByContinent(null, begin));
            return list;
        }

        private async void bindCities()
        {
            cities = await getCitiesAsync(cityBegin);
            cityBox.ItemsSource = cities;
        }

        private async Task<List<string>> getTypesAsync(string begin)
        {
            List<string> list = await Task.Run(() => UPmodel.getTypes(begin));
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
            attractionsList.ItemsSource = null;
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            reset();
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = attractionsList.Items.IndexOf(item);
            ReturenedAttraction = attractions[itemIndex];
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
            FindCityByContinent fbc = new FindCityByContinent(UPmodel);
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
            FindCityByCountry fbc = new FindCityByCountry(UPmodel);
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

        private async Task<List<Attraction>> getAttractionsAsync(string cityId, string type, string name, List<Attraction> drop)
        {
            List<Attraction> list = await Task.Run(() => CTmodel.getAttractions(cityId, type, name, drop));
            return list;
        }

        private async void bindAttractions()
        {
            string type = "";
            if(typeBox.SelectedIndex != -1)
            {
                type = types[typeBox.SelectedIndex];
            }
            attractions = await getAttractionsAsync(city_code, type, attrationName.Text, choosed);
            attractionsList.ItemsSource = attractions;
        }

        private void filterClick(object sender, RoutedEventArgs e)
        {
            bindAttractions();
        }
    }
}
