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

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for FindByLocation.xaml
    /// </summary>
    public partial class FindByLocation : Window
    {
        private FindTripModel model;
        private List<City> cities;

        public FindByLocation(FindTripModel m)
        {
            InitializeComponent();
            model = m;
            cities = m.getAllCities();
            CityListBox.ItemsSource = cities;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
