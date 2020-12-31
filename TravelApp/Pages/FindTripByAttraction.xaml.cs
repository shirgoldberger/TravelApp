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
    /// Interaction logic for FindTripByAttraction.xaml
    /// </summary>
    public partial class FindTripByAttraction : Window, INotifyPropertyChanged
    {
        private FindTrip_Controller controller;
        private List<City> cities;
        private List<Attraction> attractions;
        private List<Attraction> selectedAttractions;
        private string cityBegin;
        private City selectedCity;
        public FindTripByAttraction(FindTrip_Controller c)
        {
            InitializeComponent();
            endLoadAttractions();
            endLoadCities();
            controller = c;
            cityBegin = "";
            bindCities();
            selectedAttractions = new List<Attraction>();
            selectedCity = null;
            
        }

        public City SelectedCity
        {
            get { return selectedCity; }
            set { selectedCity = value;
            }
        }

        public List<Attraction> Attractions
        {
            get { return attractions; }
        }
        public List<City> Cities
        {
            get { return cities; }
        }
        public List<Attraction> SelectedAttractions
        {
            get { return selectedAttractions; }
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

        private async Task<Tuple<bool, List<City>>> getCitiesAsync(string begin)
        {
            Tuple<bool, List<City>> list = await Task.Run(() => controller.getCitiesByBegin(begin));
            return list;
        }

        private async void bindCities()
        {
            startLoadCities();
            Tuple<bool, List<City>> t = await getCitiesAsync(cityBegin);
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access cities records");
            }
            cities = t.Item2;
            cityBox.ItemsSource = cities;
            endLoadCities();
        }

        private async Task<Tuple<bool, List<Attraction>>> getAttractionsAsync(City city)
        {
            Tuple<bool, List<Attraction>> list = await Task.Run(() => controller.getAttractionsByCity(city, ""));
            return list;
        }

        private async void bindAttractions()
        {
            startLoadAttractions();
            City city = null;
            if (cityBox.SelectedItem != null)
            {
                city = (City)cityBox.SelectedItem;
            }
            Tuple<bool, List<Attraction>> t = await getAttractionsAsync(selectedCity);
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access attractions records");
            }
            attractions = t.Item2;
            foreach (Attraction a in attractions)
            {
                if (selectedAttractions.Exists(x => x.Attraction_code == a.Attraction_code))
                {
                    a.Can_Choose = false;
                }
            }
            attractionsList.ItemsSource = attractions;
            endLoadAttractions();
        }

        private void chooseClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = attractionsList.Items.IndexOf(item);
            Attraction a = attractions[itemIndex];
            if (!selectedAttractions.Exists(x => x.Attraction_code == a.Attraction_code))
            {
                selectedAttractions.Add(a);
            }
            Button b = (Button)sender;
            b.IsEnabled = false;
        }

        private void citySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City c = (City)cityBox.SelectedItem;
            if (c != null)
            {
                SelectedCity = c;
                bindAttractions();
            }
        }

        private void filterByText(object sender, RoutedEventArgs e)
        {
            cityBegin = cityBox.Text;
            bindCities();
            cityBox.IsDropDownOpen = true;
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

        private void startLoadAttractions()
        {
            progressBarAttractions.IsIndeterminate = true;
            progressBarTextAttractions.Visibility = Visibility.Visible;
            progressBarAttractions.Visibility = Visibility.Visible;
        }

        private void endLoadAttractions()
        {
            progressBarAttractions.IsIndeterminate = false;
            progressBarTextAttractions.Visibility = Visibility.Hidden;
            progressBarAttractions.Visibility = Visibility.Hidden;
        }
    }
}