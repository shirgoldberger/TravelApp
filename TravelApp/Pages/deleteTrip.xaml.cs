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
using TravelApp.Objects;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for deleteTrip.xaml
    /// </summary>
    public partial class deleteTrip : Window
    {
        Trip trip;
        string username;
        List<string> members;
        deleteTrip_Controller controller;
        Page pageToUpdate;
        public deleteTrip(Trip trip, string _username, Page pageToUpdate)
        {
            username = _username;
            InitializeComponent();
            controller = new deleteTrip_Controller(trip, username);
            var touple = controller.getAllMembers();
            if (touple.Item1 == false)
            {
                Utils.Instance.errorAndExit("Failed to get all members - check SQL");
            }
            members = touple.Item2;
            allMemListBox.ItemsSource = members;
            this.trip = trip;
            this.pageToUpdate = pageToUpdate;
        }
        public void Button_Click_All(object sender, RoutedEventArgs e)
        {
            if(!controller.Click_All())
            {
                Utils.Instance.errorAndExit("Error tyring to access trip records");
            }
            MessageBox.Show("Delete all trip members and trip itself succeed");
            (pageToUpdate as viewAllTrip).Updated = true;
            this.Close();

        }
        public void Button_Click_OnlyMe(object sender, RoutedEventArgs e)
        {

        }
        public void row_click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allMemListBox.Items.IndexOf(item);
            string newAdmin = members[itemIndex];
            if (!controller.row_click(newAdmin))
            {
                Utils.Instance.errorAndExit("Error tyring to delete trip member");
            }
            MessageBox.Show("Delete succeed, the new assigned admin is " + newAdmin);
            (pageToUpdate as viewAllTrip).Updated = true;
            this.Close();
        }


    }
}
