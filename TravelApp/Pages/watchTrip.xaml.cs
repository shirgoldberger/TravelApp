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
    /// Interaction logic for watchTrip.xaml
    /// </summary>
    public partial class watchTrip : Window
    {
        private int id;
        private string admin;
        private string nameTrip;
        private DateTime start_date;
        private DateTime end_date;
        private int min_age;
        private int max_age;
        private int max_participants;
        private bool male_only;
        private bool female_only;
        private List<string> members;
        private List<Attraction> attractions;
        private watchTrup_Controller controller;



        public watchTrip(Trip trip)
        {
            InitializeComponent();
            start_date = trip.Start_Date;
            end_date = trip.End_Date;
            min_age = trip.Min_Age;
            max_age = trip.Max_Age;
            max_participants = trip.Max_Participants;
            male_only = trip.Male_Only;
            female_only = trip.Female_Only;
            nameTrip = trip.Name;
            id = trip.Id;
            admin = trip.Admin;
            //
            if (female_only== true)
            {
                female.IsChecked = true;
            }
            else if (male_only == true)
            {
                male.IsChecked = true;
            }
            female.IsEnabled = false;
            male.IsEnabled = false;

            //
            DataContext = this;
            controller = new watchTrup_Controller(trip);
            Tuple<bool, List<string>> membersTuple = controller.getMem();
            if(!membersTuple.Item1)
            {
                Utils.errorAndExit("Error trying to access members records");
            }
            members = membersTuple.Item2;

            Tuple<bool, List<Attraction>> attractionsTuple = controller.getAtt();
            if (!attractionsTuple.Item1)
            {
                Utils.errorAndExit("Error trying to access members records");
            }
            attractions = attractionsTuple.Item2;

            allMemListBox.ItemsSource = members;
            allAttListBox.ItemsSource = attractions;


        }
        public int Id
        {
            get { return id; }

        }
        public string Admin
        {
            get { return admin; }

        }
        public string NameTrip
        {
            get { return nameTrip; }

        }
        public string Start_date
        {
            get { return start_date.ToString("dd/MM/yyyy"); }

        }
        public string End_date
        {
            get { return end_date.ToString("dd/MM/yyyy"); }

        }
        public string Min_age
        {
            get { return min_age.ToString(); }
            set { min_age = Convert.ToInt32(value); }
        }
        public string Max_age
        {
            get { return max_age.ToString(); }
            set { max_age = Convert.ToInt32(value); }

        }
        public string Max_participants
        {
            get { return max_participants.ToString(); }
            set { max_participants = Convert.ToInt32(value); }

        }
        

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void gender_Copy_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
