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
        editTheTrip_Model model;
        List<string> members;
        Trip trip;
        string error;

        public add_new_mem_for_trip(editTheTrip_Model model, Trip trip1)
        {
            InitializeComponent();
            this.model = model;
            this.trip = trip1;
            members = model.getAllMembersNOtInTHisTrip(trip);
            allAtList.ItemsSource = members;
            error = "there is 0 members to add";
        }
        private void clickAdd_loc(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allAtList.Items.IndexOf(item);
            bool a = model.add_new_Mem_for_trip(trip, members[itemIndex]);
            if (a == true)
            {
                MessageBox.Show("delete sucses");

            }
            else
            {
                MessageBox.Show("delete failed");

            }
            members = model.getAllMembersNOtInTHisTrip(trip);
            allAtList.ItemsSource = members;
        }

        public string Error
        {
            get { return error; }

        }
    }
}
