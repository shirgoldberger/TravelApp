using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using TravelApp.Objects;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for viewAllTrip.xaml
    /// </summary>
    public partial class ViewAllTrip: Page
    {
        private string username;
        private List<Trip> trips;
        private ViewAllTrip_controller controller;

        public ViewAllTrip(string _username)
        {
            username = _username;
            InitializeComponent();
            controller = new ViewAllTrip_controller(username);
            trips = getAllTrips();
            allTripsListBox.ItemsSource = trips;
        }
        private void Button_Click_add_trip(object sender, RoutedEventArgs e)
        {

            AddNewTrip ant = new AddNewTrip(username, this);
            Updated = false;
            this.NavigationService.Navigate(ant);

        }
        public void row_click(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allTripsListBox.Items.IndexOf(item);
            WatchTrip watch = new WatchTrip(trips[itemIndex]);
            watch.Show();
        }
        public void clickDelete(object sender, RoutedEventArgs e) 
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allTripsListBox.Items.IndexOf(item);
            //check if he is the admin of this trip.
            Trip clickedTrip = trips[itemIndex];
            if (username != clickedTrip.Admin)
            {
                if(!controller.click_delete(clickedTrip))
                {
                    Utils.Instance.errorAndExit("Error trying to access trip records");
                }
                else
                {
                    MessageBox.Show("Delete succeed");
                }
            }
            else
            {
                DeleteTrip delete = new DeleteTrip(clickedTrip, username, this);
                delete.Show();
            }
            
            trips = getAllTrips();
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
                MessageBox.Show("You cannot edit this trip - only admin can");

            }
            else
            {
                Tuple<bool, List<User>> memTuple = controller.getAllMem(pushTrip);
                if(!memTuple.Item1)
                {
                    Utils.Instance.errorAndExit("Error trying access members records");
                }
                List<User> mem = memTuple.Item2;
                Tuple < bool, List<Attraction>> attTuple = controller.getAllAtt(pushTrip);
                if(!attTuple.Item1)
                {
                    Utils.Instance.errorAndExit("Error trying access attractions records");
                }
                List<Attraction> att = attTuple.Item2;
                AddNewTrip ant = new AddNewTrip(this, pushTrip, att, mem);
                Updated = false;
                NavigationService.Navigate(ant);
            }
        }

        private List<Trip> getAllTrips()
        {
            Tuple<bool, List<Trip>> tuple = controller.getAllTrip();
            if (!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access trips records");
            }
            return tuple.Item2;
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
            trips = getAllTrips();
            allTripsListBox.ItemsSource = null;
            allTripsListBox.ItemsSource = trips;
        }
    }
}
