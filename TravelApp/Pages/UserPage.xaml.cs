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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravelApp.Models;
using TravelApp.Pages;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private string username;
        private UserPageModel model;

        public UserPage(string _username)
        {
            DataContext = this;
            username = _username;
            InitializeComponent();
            helloStr.Text = "Hello " + username;
            model = new UserPageModel();
        }

        private void viewTripsButton_Click(object sender, RoutedEventArgs e)
        {
            viewAllTrip vt = new viewAllTrip(username);
            NavigationService.Navigate(vt);
        }

        private void searchNewTripButton_Click(object sender, RoutedEventArgs e)
        {
            FindTrip vt = new FindTrip(username);
            NavigationService.Navigate(vt);
        }

        private void addFriendButton_Click(object sender, RoutedEventArgs e)
        {
            addNewFriend nf = new addNewFriend(username, model);
            nf.Show();
        }

        private void addNewTripButton_Click(object sender, RoutedEventArgs e)
        {
            addNewTrip nt = new addNewTrip(username);
            NavigationService.Navigate(nt);
        }

        private void addAttractionButton_Click(object sender, RoutedEventArgs e)
        {
            AddNewAtt at = new AddNewAtt(model);
            at.Show();
        }
        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
