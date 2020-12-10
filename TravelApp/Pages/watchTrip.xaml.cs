﻿using System;
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
        int id;
        string admin;
        DateTime start_date; 
       DateTime end_date;
       private int min_age;
       private int max_age;
       private string genderMassage;
       private int max_participants;
       public bool male_only;
       public bool female_only;
        List<string> members;
        List<Attraction> attractions;
        watchTrup_Controller controller;



        public watchTrip(Trip trip)
        {
            start_date = trip.Start_Date;
            end_date = trip.End_Date;
            min_age = trip.Min_Age;
            max_age = trip.Max_Age;
            max_participants = trip.Max_Participants;
            male_only = trip.Male_Only;
            female_only = trip.Female_Only;
            id = trip.Id;
            admin = trip.Admin;
            //
            if (female_only== true)
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
            controller = new watchTrup_Controller(trip);
            members = controller.getMem();
            attractions = controller.getAtt();

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
        
        public string GenderMassage
        {
            get { return genderMassage.ToString(); }
            set { genderMassage = value; }

        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


    }
}
