using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

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
            languages = controller.getLanguages();
            languagesComboBox.ItemsSource = languages;
            users = controller.getUsers();
            friendsComboBox.ItemsSource = users;
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
            if (textBoxUserName.Text.Length == 0)
            {
                MessageBox.Show("Please insert username");
                textBoxUserName.Focus();
                return;
            }
            if (choosenLanguages.Count == 0)
            {
                MessageBox.Show("Please choose languages");
                languagesComboBox.Focus();
                return;
            }
            if (textBoxPhone.Text.Length != 10)
            {
                MessageBox.Show("Please insert correct phone number");
                textBoxPhone.Focus();
                return;
            }
            if (male.IsChecked == false && female.IsChecked == false)
            {
                MessageBox.Show("Choose Gender");
                male.Focus();
                return;
            }
            if (textBoxEmail.Text.Length == 0)
            {
                MessageBox.Show("Enter an email");
                textBoxEmail.Focus();
                return;
            }
            if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Enter a valid email");
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
                return;
            }
            char gender;
            if ((bool)(female.IsChecked))
            {
                gender = '0';
            } else
            {
                gender = '1';
            }
            int age;
            try
            {
                age = int.Parse(textBoxAge.Text);
            }
            catch
            {
                MessageBox.Show("Enter valid age");
                textBoxAge.Focus();
                return;
            }
            if (passwordBox.Password.Length == 0)
            {
                MessageBox.Show("Enter password");
                passwordBox.Focus();
                return;
            }
            if (passwordBoxConfirm.Password.Length == 0)
            {
                MessageBox.Show("Enter Confirm password");
                passwordBoxConfirm.Focus();
                return;
            }
            if (passwordBox.Password != passwordBoxConfirm.Password)
            {
                MessageBox.Show("Confirm password must be same as password");
                passwordBoxConfirm.Focus();
                return;
            }
            else
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
    }
}
