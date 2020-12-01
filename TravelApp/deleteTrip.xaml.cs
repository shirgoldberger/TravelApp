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
    /// Interaction logic for deleteTrip.xaml
    /// </summary>
    public partial class deleteTrip : Window
    {
        Trip trip;
        User user;
        viewAllTrip_Model model;
        List<User> members;
        watchTrip_Model watchTrip_Model;
        public deleteTrip(Trip trip, viewAllTrip_Model model, User user)
        {
            this.user = user;
            InitializeComponent();
            watchTrip_Model = new watchTrip_Model(trip);
            members = watchTrip_Model.getAllMembers();
            allMemListBox.ItemsSource = members;
            this.trip = trip;
            this.model = model;
        }
        public void Button_Click_All(object sender, RoutedEventArgs e)
        {
            bool a = model.delteAllTripMember(trip);
            if (a == false)
            {
                MessageBox.Show("delete failed");

            }
            if (a == true)
            {
                MessageBox.Show("delete sucses");
                this.Close();
            }
        }
        public void Button_Click_OnlyMe(object sender, RoutedEventArgs e)
        {

        }
        public void row_click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allMemListBox.Items.IndexOf(item);
            bool a = model.setUserAdmin(trip, user, members[itemIndex].Username);
            if (a == true)
            {
                MessageBox.Show("you deleted from trup and "+members[itemIndex].Username +" is the new admin.");
                this.Close();
            }
            else
            {
                MessageBox.Show("delete faild");
            }
        }


    }
}
