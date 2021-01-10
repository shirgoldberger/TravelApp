using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Objects;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for add_new_att_for_trip.xaml
    /// </summary>
    public partial class AddNewFriend : Window
    {
        private string username;
        private List<string> users;
        private UserPage_Controller controller;

        public AddNewFriend(string _username, UserPage_Controller _controller)
        {
            InitializeComponent();
            username = _username;
            controller = _controller;
            bindNonUserList();
        }

        private void bindNonUserList()
        {
            string beginning = beginTextBox.Text;
            Tuple<bool, List<string>> tuple = controller.getRestUsers(username, beginning);
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying to access users records");
            }
            users = tuple.Item2;
            userList.ItemsSource = null;
            userList.ItemsSource = users;
        }

        private void clickAdd(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            var itemIndex = userList.Items.IndexOf(item);
            string candidate = users[itemIndex];
            bool result = controller.addNewFriend(username, candidate);
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

        private void filterByText(object sender, RoutedEventArgs e)
        {
            bindNonUserList();
        }
    }
}
