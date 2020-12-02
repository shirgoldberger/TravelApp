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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace TravelApp
{
    /// <summary>
    /// Interaction logic for viewAllTrip.xaml
    /// </summary>
    public partial class viewAllTrip: Page
    {
        string username;
        viewAllTrip_Model view;
        List<Trip> trips;

        public viewAllTrip(string _username)
        {
            username = _username;
            InitializeComponent();
            view= new viewAllTrip_Model(username);
            trips = view.getAllTrip();
            allTripsListBox.ItemsSource = trips;
            buttomDelete.ItemsSource = trips;
            buttomEdit.ItemsSource = trips;
        }
        private void Button_Click_add_trip(object sender, RoutedEventArgs e)
        {

            addNewTrip ant = new addNewTrip(username);
            this.NavigationService.Navigate(ant);
            //watchTrip watch = new watchTrip(trips[0]);
            //watch.Show();
        }
        public void row_click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allTripsListBox.Items.IndexOf(item);
            watchTrip watch = new watchTrip(trips[itemIndex]);
            watch.Show();
        }
        public void clickDelete(object sender, RoutedEventArgs e) 
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = buttomDelete.Items.IndexOf(item);
            //check if he is the admin of this trip.
            if (username == trips[itemIndex].Admin)
            {
                deleteTrip delete = new deleteTrip(trips[itemIndex], view, username);
                delete.Show();
                trips = view.getAllTrip();
                allTripsListBox.ItemsSource = trips;
                buttomDelete.ItemsSource = trips;
            }
            else
            {
                bool a = view.deleteTrip(trips[itemIndex], username);
                if (a == false)
                {
                    MessageBox.Show("delete failed");

                }
                if (a == true)
                {
                    MessageBox.Show("delete sucses");
                    trips = view.getAllTrip();
                    allTripsListBox.ItemsSource = trips;
                    buttomDelete.ItemsSource = trips;
                    buttomEdit.ItemsSource = trips;

                }
            }

        }
        public void clickEdit(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = buttomDelete.Items.IndexOf(item);
            Trip pushTrip = trips[itemIndex];
            if (pushTrip.Admin!= username)
            {
                MessageBox.Show("you can not edit this trip - only admin can edit");

            }
            else
            {
                editTheTrip edt = new editTheTrip(pushTrip, username);
                edt.Show();
            }
        }

        private void allTripsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
