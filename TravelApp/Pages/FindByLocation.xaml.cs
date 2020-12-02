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

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for FindByLocation.xaml
    /// </summary>
    public partial class FindByLocation : Window, INotifyPropertyChanged
    {
        private FindTripModel model;
        private List<City> cities;
        private List<City> selectedCities;
        private List<Attraction> attractions;
        private List<Attraction> selectedAttractions;

        public FindByLocation(FindTripModel m, List<City> _cities, List<Attraction> _attractions)
        {
            InitializeComponent();
            model = m;
            cities = _cities;
            CityListBox.ItemsSource = cities;
            attractions = _attractions;
            AttractionListBox.ItemsSource = attractions;
            selectedCities = new List<City>();
            selectedAttractions = new List<Attraction>();
        }

        public List<Attraction> Attractions
        {
            get { return attractions; }
        }
        public List<City> Cities
        {
            get { return cities; }
        }
        public List<City> SelectedCities
        {
            get { return selectedCities; }
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

        private void CheckBox_Checked_City(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            City currentCity = cities.Find(x => x.Id == id);
            if (!selectedCities.Exists(x => x.Id == id))
            {
                selectedCities.Add(currentCity);
            }
            CityListBox.ItemsSource = selectedCities;
            attractions = model.GetAttractionsByCities(selectedCities);
            AttractionListBox.ItemsSource = attractions;
        }

        private void CheckBox_Unchecked_City(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            City currentCity = cities.Find(x => x.Id == id);
            if (selectedCities.Exists(x => x.Id == id))
            {
                selectedCities.Remove(currentCity);
            }
            if (selectedCities.Count() == 0)
            {
                selectedCities = cities;
            }
            CityListBox.ItemsSource = selectedCities;
            attractions = model.GetAttractionsByCities(selectedCities);
            AttractionListBox.ItemsSource = attractions;
        }

        private void CheckBox_Checked_Attraction(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            Attraction currentAttraction = attractions.Find(x => x.Attraction_code == id);
            selectedAttractions.Add(currentAttraction);
        }

        private void CheckBox_Unchecked_Attraction(object sender, RoutedEventArgs e)
        {
            string id = ((CheckBox)sender).Uid.ToString();
            Attraction currentAttraction = attractions.Find(x => x.Attraction_code == id);
            selectedAttractions.Remove(currentAttraction);
        }

        private void TextBox_TextChanged_City(object sender, TextChangedEventArgs e)
        {
            string text = searchBoxCity.Text;
            CityListBox.ItemsSource = cities.Where(x => x.City_String.Contains(text));
        }

        private void TextBox_TextChanged_Attraction(object sender, TextChangedEventArgs e)
        {
            string text = searchBoxAttraction.Text;
            CityListBox.ItemsSource = attractions.Where(x => x.Attraction_String.Contains(text));
        }
        private void find_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
