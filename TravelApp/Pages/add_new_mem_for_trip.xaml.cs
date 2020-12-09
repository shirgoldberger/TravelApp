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
    /// Interaction logic for add_new_mem_for_trip.xaml
    /// </summary>
    public partial class add_new_mem_for_trip : Window
    {
        List<string> members;
        Trip trip;
        editTheTrip editTheTrip;
        add_new_mem_for_trip_Controller controller;

        public add_new_mem_for_trip(Trip trip1, editTheTrip ed)
        {
            InitializeComponent();
            editTheTrip = ed;
            this.trip = trip1;
            controller = new add_new_mem_for_trip_Controller(trip1);
            members = controller.getAllMembersNOtInTHisTrip();
            allAtList.ItemsSource = members;
        }
        private void clickAdd_loc(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allAtList.Items.IndexOf(item);
            var t = controller.Click_add(members[itemIndex]);
            MessageBox.Show(t.Item2);
            members = controller.getAllMembersNOtInTHisTrip();
            editTheTrip.updateMember();
            allAtList.ItemsSource = members;
        }


    }
}

