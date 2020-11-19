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
    /// Interaction logic for watchTrip.xaml
    /// </summary>
    public partial class watchTrip : Window
    {

        public string start_date { get; set; }
        public DateTime end_date;
        public int min_age;
        public int max_age;
        public int max_participants;
        public bool male_only;
        public bool female_only;

        viewAllTrip_Model view;
        List<Trip> trips;


        public watchTrip(Trip trip)
        {
            start_date = "aaaaa";
            end_date = trip.End_Date;
            min_age = trip.Min_Age;
            max_age = trip.Max_Age;
            max_participants = trip.Max_Participants;
            male_only = trip.Male_Only;
            female_only = trip.Female_Only;
            //
            if (female_only== true)
            {
                male.IsChecked = false;
                female.IsChecked = true;
            }
            if (male_only == true)
            {
                male.IsChecked = false;
                female.IsChecked = true;
            }
            //
            InitializeComponent();

            view = new viewAllTrip_Model();
            trips = view.getAllTrip();
            allTripsListBox.ItemsSource = trips;


        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {

        }

        private void maxParticipantsVal_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void maxParticipantsVal_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
