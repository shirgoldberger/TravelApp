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
        User user;
        viewAllTrip_Model view;
        List<Trip> trips;
        private List<Language> languages;
        private List<Language> choosenLanguages;
        public viewAllTrip(User _user)
        {
            user = _user;
            InitializeComponent();
            view= new viewAllTrip_Model(user);
            trips = view.getAllTrip();
            allTripsListBox.ItemsSource = trips;
            buttomDelete.ItemsSource = trips;
            buttomEdit.ItemsSource = trips;
            choosenLanguages = new List<Language>();
        }
        private void Button_Click_add_trip(object sender, RoutedEventArgs e)
        {

            addNewTrip ant = new addNewTrip(user.Username, user.Password);
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
            bool a= view.deleteTrip(trips[itemIndex], user);
            if (a == false)
            {
                MessageBox.Show("delete failed");

            }
            if (a== true)
            {
                MessageBox.Show("delete sucses");
                trips = view.getAllTrip();
                allTripsListBox.ItemsSource = trips;
                buttomDelete.ItemsSource = trips;
                buttomEdit.ItemsSource = trips;
                choosenLanguages = new List<Language>();

            }

        }
        public void clickEdit(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = buttomDelete.Items.IndexOf(item);
            Trip pushTrip = trips[itemIndex];
            if (pushTrip.Admin!= user.Username)
            {
                MessageBox.Show("you can not edit this trip - only admin can edit");

            }
            else
            {
                editTheTrip edt = new editTheTrip(pushTrip, user);
                edt.Show();
            }
        }

        private void allTripsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
