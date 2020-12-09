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
    /// Interaction logic for add_new_att_for_trip.xaml
    /// </summary>
    public partial class add_new_att_for_trip : Window
    {
        List<Attraction> attractions;
        Trip trip;
        editTheTrip editTheTrip;
        add_new_att_for_trip_Controller controller;

        public add_new_att_for_trip(Trip trip1, editTheTrip edt)
        {
            InitializeComponent();
            editTheTrip = edt;
            controller = new add_new_att_for_trip_Controller(trip1);
            this.trip = trip1;
            attractions = controller.getAllAttractionNotInThisTrip(trip);
            allAtList.ItemsSource = attractions;
        }
        private void clickAdd_loc(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allAtList.Items.IndexOf(item);
            Attraction at = attractions[itemIndex];
            var t = controller.click_add(at);
            MessageBox.Show(t.Item2);
            editTheTrip.updateAtt();
            attractions = controller.getAllAttractionNotInThisTrip(trip);
            allAtList.ItemsSource = attractions;
        }
    }
}
