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
    /// Interaction logic for add_new_att_for_trip.xaml
    /// </summary>
    public partial class add_new_att_for_trip : Window
    {
        editTheTrip_Model model;
        List<Attraction> attractions;
        Trip trip;

        public add_new_att_for_trip(editTheTrip_Model model, Trip trip1)
        {
            InitializeComponent();
            this.model = model;
            this.trip = trip1;
            attractions = model.getAllAttractionNotInThisTrip(trip);
            allAtList.ItemsSource = attractions;
        }
        private void clickAdd_loc(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = allAtList.Items.IndexOf(item);
            bool a = model.add_new_Att_for_trip(trip, attractions[itemIndex]);
            if (a == true)
            {
                MessageBox.Show("add sucses");
                attractions = model.getAllAttractionNotInThisTrip(trip);
                allAtList.ItemsSource = attractions;
            }
            else
            {
                MessageBox.Show("add failed");

            }
        }
    }
}