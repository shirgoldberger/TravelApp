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
        UserPageModel model;
        List<string> continents;
        List<string> cities;
        private string continentBegin;

        public string City {get;set;}

        public FindCityByContinent(UserPageModel _model)
        {
            model = _model;
            InitializeComponent();
            continentBegin = "";
            bindContinents();
        }

        private async Task<List<string>> getContinentsAsync(string begin)
        {
            List<string> list = await Task.Run(() => model.getContinents(begin));
            return list;
        }

        private async void bindContinents()
        {
            continents = await getContinentsAsync(continentBegin);
            continentBox.ItemsSource = continents;
        }


        private async Task<List<string>> getCitiesAsync(string continent)
        {
            List<string> list = await Task.Run(() => model.getCitiesByContinent(continent, ""));
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
            City = cities[itemIndex];
            GetWindow(this).Close();
        }

        private void continentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bindContinents();
            bindCities();
        }

        private void continentTextChanged(object sender, EventArgs e)
        {
            continentBegin = continentBox.Text;
            bindContinents();
        }

    }
}
