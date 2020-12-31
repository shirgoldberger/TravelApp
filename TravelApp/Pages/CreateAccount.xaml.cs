using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Objects;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        private CreateAccount_Controller controller;
        private List<string> languages;
        private List<string> choosenLanguages;
        private List<string> users;
        private List<string> choosenFriends;

        public CreateAccount()
        {
            InitializeComponent();
            controller = new CreateAccount_Controller();
            choosenLanguages = new List<string>();
            choosenFriends = new List<string>();
            bindUsers();
            bindLanguages();
        }

        private async void bindUsers()
        {
            startLoadUsers();
            Tuple<bool, List<string>> t = await getUsersAsync();
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access users records");
            }
            users = t.Item2;
            friendsComboBox.ItemsSource = users;
            endLoadUsers();
        }

        private async void bindLanguages()
        {
            startLoadLanguages();
            Tuple<bool, List<string>> t = await getLanguagesAsync();
            if (!t.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access languages records");
            }
            languages = t.Item2;
            languagesComboBox.ItemsSource = languages;
            endLoadLanguages();
        }

        private async Task<Tuple<bool, List<string>>> getUsersAsync()
        {
            Tuple<bool, List<string>> list = await Task.Run(() => controller.getUsers());
            return list;
        }
        private async Task<Tuple<bool, List<string>>> getLanguagesAsync()
        {
            Tuple<bool, List<string>> list = await Task.Run(() => controller.getLanguages());
            return list;
        }

        private void Checked_Friend(object sender, RoutedEventArgs e)
        {
            // add to choosen friends
            string username = ((CheckBox)sender).Uid.ToString();
            if (!choosenFriends.Exists(x => x == username))
            {
                choosenFriends.Add(username);
            }
        }

        private void Unchecked_Friend(object sender, RoutedEventArgs e)
        {
            // remove from choosen friands
            string username = ((CheckBox)sender).Uid.ToString();
            if (choosenFriends.Exists(x => x == username))
            {
                choosenFriends.Remove(username);
            }
        }

        private void resetLanguages()
        {
            choosenLanguages.Clear();
            languagesComboBox.Text = "";
            languagesComboBox.SelectedIndex = -1;
        }

        private void resetFriends()
        {
            choosenFriends.Clear();
            friendsComboBox.Text = "";
            friendsComboBox.SelectedIndex = -1;
        }

        private void Checked_Language(object sender, RoutedEventArgs e)
        {
            // add to choosen languages
            string name = ((CheckBox)sender).Uid.ToString();
            if (!choosenLanguages.Exists(x => x == name))
            {
                choosenLanguages.Add(name);
            }
        }

        private void Unchecked_Language(object sender, RoutedEventArgs e)
        {
            // remove from choosen languages
            string name = ((CheckBox)sender).Uid.ToString();
            if (choosenLanguages.Exists(x => x == name))
            {
                choosenLanguages.Remove(name);
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            textBoxUserName.Text = "";
            textBoxPhone.Text = "";
            textBoxAge.Text = "";
            textBoxEmail.Text = "";
            textBoxAddress.Text = "";
            passwordBox.Password = "";
            passwordBoxConfirm.Password = "";
            male.IsChecked = false;
            female.IsChecked = false;
            resetLanguages();
            resetFriends();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            HomePage login = new HomePage();
            this.NavigationService.Navigate(login);
        }
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            Tuple<bool, string> t = controller.createAccount(textBoxUserName.Text, textBoxPhone.Text, textBoxEmail.Text, passwordBox.Password, 
                passwordBoxConfirm.Password, textBoxAddress.Text, textBoxAge.Text, 
                (bool)male.IsChecked, (bool)female.IsChecked, choosenFriends, choosenLanguages);
            MessageBox.Show(t.Item2);
            if (t.Item1)
            {
                    HomePage hp = new HomePage();
                    this.NavigationService.Navigate(hp);
            }
        }
        private void male_Checked(object sender, RoutedEventArgs e)
        {
            female.IsChecked = false;
        }

        private void female_Checked(object sender, RoutedEventArgs e)
        {
            male.IsChecked = false;
        }

        private void Languages_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = languages.Where(x => x.StartsWith(languagesComboBox.Text.Trim()));
        }

        private void Friends_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = languages.Where(x => x.StartsWith(languagesComboBox.Text.Trim()));
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // close the program
            System.Environment.Exit(0);
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void startLoadUsers()
        {
            progressBarUsers.IsIndeterminate = true;
            progressBarTextUsers.Visibility = Visibility.Visible;
            progressBarUsers.Visibility = Visibility.Visible;
        }

        private void endLoadUsers()
        {
            progressBarUsers.IsIndeterminate = false;
            progressBarTextUsers.Visibility = Visibility.Hidden;
            progressBarUsers.Visibility = Visibility.Hidden;

        }

        private void startLoadLanguages()
        {
            progressBarLanguages.IsIndeterminate = true;
            progressBarTextLanguages.Visibility = Visibility.Visible;
            progressBarLanguages.Visibility = Visibility.Visible;
        }

        private void endLoadLanguages()
        {
            progressBarLanguages.IsIndeterminate = false;
            progressBarTextLanguages.Visibility = Visibility.Hidden;
            progressBarLanguages.Visibility = Visibility.Hidden;
        }
    }
}
