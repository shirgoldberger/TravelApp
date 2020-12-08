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
using TravelApp.Models;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for add_new_att_for_trip.xaml
    /// </summary>
    public partial class addNewFriend : Window
    {
        string username;
        List<string> users;
        UserPageModel model;

        public addNewFriend(string _username, UserPageModel _model)
        {
            InitializeComponent();
            username = _username;
            model = _model;
            bindNonUserList();
        }

        private void bindNonUserList()
        {
            string beginning = beginTextBox.Text;
            users = model.getRestUsers(username, beginning);
            userList.ItemsSource = users;
        }

        private void addClick(object sender, MouseButtonEventArgs e)
        {
            var item = ((Viewbox)sender).DataContext;
            var itemIndex = userList.Items.IndexOf(item);
            string candidate = users[itemIndex];
            bool result = model.addNewFriend(username, candidate);
            string msg = "Adding " + candidate + " to friends list";
            if (result)
            {
                bindNonUserList();
                MessageBox.Show(msg + " succeed");
            }
            else
            {
                MessageBox.Show(msg + " failed");
            }
        }

        private void textChanged(object sender, TextChangedEventArgs e)
        {
            bindNonUserList();
        }

    }
}