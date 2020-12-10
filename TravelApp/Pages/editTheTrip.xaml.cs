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
    /// Interaction logic for editTheTrip.xaml
    /// </summary>
    public partial class editTheTrip : Window
    {
        Trip trip;
        string username;
        DateTime start_date;
        DateTime end_date;
        private int min_age;
        private int max_age;
        private string genderMassage;
        private int max_participants;
        public bool male_only;
        public bool female_only;
        editTheTrip_Controller controller;
        List<string> members;
        List<Attraction> attractions;



        public editTheTrip(Trip trip, string _username)
        {
            this.trip = trip;
            username = _username;
            start_date = trip.Start_Date;
            end_date = trip.End_Date;
            min_age = trip.Min_Age;
            max_age = trip.Max_Age;
            max_participants = trip.Max_Participants;
            male_only = trip.Male_Only;
            female_only = trip.Female_Only;
            //
            if (female_only == true)
            {
                genderMassage = "female only trip";
            }
            else if (male_only == true)
            {
                genderMassage = "men only trip";
            }
            else
            {
                genderMassage = "There are no gender restrictions";

            }
            //
            InitializeComponent();
            DataContext = this;
            controller = new editTheTrip_Controller(trip);
            members = controller.getAllMembers();
            attractions = controller.getAllAttraction();
            allMemListBox.ItemsSource = members;
            allAttListBox.ItemsSource = attractions;


        }
        public void Button_Click_Submit(object sender, RoutedEventArgs e)
        {
            string a = start_date + "," + end_date + "," + min_age + "," + max_age + "," + max_participants;
            Console.WriteLine(a);
            var ans = controller.Button_Click_Submit(username, Start_date, End_date, Min_age, Max_age, Max_participants);
            MessageBox.Show(ans.Item2);
        }
        public void Button_Click_Add_Member(object sender, RoutedEventArgs e) { }
        public string Start_date
        {
            get { return start_date.ToString("dd.MM.yyyy"); }
            set { start_date = DateTime.Parse(value); }

        }
        public string End_date
        {
            get { return end_date.ToString("dd.MM.yyyy"); }
            set { end_date = DateTime.Parse(value); }


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

        public string GenderMassage
        {
            get { return genderMassage.ToString(); }
            set { genderMassage = value; }

        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        
        private void Button_Click_Add_New_Att(object sender, RoutedEventArgs e)
        {
            add_new_att_for_trip anl = new add_new_att_for_trip( trip, this);
            anl.ShowDialog();
            attractions = controller.getAllAttraction();
            allAttListBox.ItemsSource = attractions;
        }
        private void Button_Click_Add_New_Member(object sender, RoutedEventArgs e)
        {
            add_new_mem_for_trip anl = new add_new_mem_for_trip( trip, this);
            anl.ShowDialog();
            members = controller.getAllMembers();
            allMemListBox.ItemsSource = members;

        }
        private void clickDelete_mem(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allMemListBox.Items.IndexOf(item);
            var t = controller.delete_mem(members[itemIndex]);
            MessageBox.Show(t.Item2);
            members = controller.getAllMembers();
            allMemListBox.ItemsSource = members;
        }
        private void clickDelete_Att(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allAttListBox.Items.IndexOf(item);
            var t = controller.delete_att(attractions[itemIndex]);
            MessageBox.Show(t.Item2);
            attractions = controller.getAllAttraction();
            allAttListBox.ItemsSource = attractions;
          
        }
        public void updateAtt()
        {
            attractions = controller.getAllAttraction();
            allAttListBox.ItemsSource = attractions;
        }
        public void updateMember()
        {
            attractions = controller.getAllAttraction();
            allAttListBox.ItemsSource = attractions;
        }

    }
}
