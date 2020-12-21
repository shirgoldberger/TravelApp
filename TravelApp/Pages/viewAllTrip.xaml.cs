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
        private string username;
        private List<Trip> trips;
        private viewAllTrip_controller controller;

        public viewAllTrip(string _username)
        {
            username = _username;
            InitializeComponent();
            controller = new viewAllTrip_controller(username);
            trips = controller.getAllTrip();
            allTripsListBox.ItemsSource = trips;
        }
        private void Button_Click_add_trip(object sender, RoutedEventArgs e)
        {

            addNewTrip ant = new addNewTrip(username);
            this.NavigationService.Navigate(ant);

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
            var itemIndex = allTripsListBox.Items.IndexOf(item);
            //check if he is the admin of this trip.
            var t = controller.click_delete(trips[itemIndex]);
            if (t.Item2 != "")
            {
                MessageBox.Show(t.Item2);
            }
            trips = controller.getAllTrip();
            allTripsListBox.ItemsSource = trips;
            
          }

        public bool Updated
        {
            set
            {
                if(value)
                {
                    update();
                }
            }
        }
        public void clickEdit(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allTripsListBox.Items.IndexOf(item);
            Trip pushTrip = trips[itemIndex];
            if (pushTrip.Admin!= username)
            {
                MessageBox.Show("you can not edit this trip - only admin can edit");

            }
            else
            {
                List<User> mem = controller.getAllMem(pushTrip);
                List<Attraction> att = controller.getAllAtt(pushTrip);
                addNewTrip ant = new addNewTrip(this, pushTrip, att, mem);
                Updated = false;
                NavigationService.Navigate(ant);
            }
        }

        private void allTripsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void update()
        {
            trips = controller.getAllTrip();
            allTripsListBox.ItemsSource = null;
            allTripsListBox.ItemsSource = trips;
        }
    }
}
