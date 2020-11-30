using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.RegularExpressions;
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

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        private CreateAccoutModel ca_model;
        private List<String> languages;
        private List<String> choosenLanguages;
        private List<String> users;
        private List<String> choosenFriends;
        public CreateAccount()
        {
            InitializeComponent();
            ca_model = new CreateAccoutModel();
            choosenLanguages = new List<String>();
            choosenFriends = new List<String>();
            languages = ca_model.getLanguages();
            languagesComboBox.ItemsSource = languages;
            users = ca_model.getUsers();
            List<String> us = new List<string>();
            friendsComboBox.ItemsSource = users;
        }
        
        private void Checked_Friend(object sender, RoutedEventArgs e)
        {
            string username = ((CheckBox)sender).Uid.ToString();
            if (!choosenFriends.Exists(x => x == username))
            {
                choosenFriends.Add(username);
            }
        }
        private void Unchecked_Friend(object sender, RoutedEventArgs e)
        {
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
            string name = ((CheckBox)sender).Uid.ToString();
            if (!choosenLanguages.Exists(x => x == name))
            {
                choosenLanguages.Add(name);
            }
        }

        private void Unchecked_Language(object sender, RoutedEventArgs e)
        {
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
            if (choosenLanguages.Count == 0)
            {
                MessageBox.Show("Please choose languages");
                languagesComboBox.Focus();
            }
            if (male.IsChecked == false && female.IsChecked == false)
            {
                MessageBox.Show("Choose Gender");
                male.Focus();
            }
            if (textBoxEmail.Text.Length == 0)
            {
                MessageBox.Show("Enter an email");
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                MessageBox.Show("Enter a valid email");
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
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
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    MessageBox.Show("Enter Confirm password");
                    passwordBoxConfirm.Focus();
                    return;
                }
                else if (passwordBox.Password != passwordBoxConfirm.Password)
                {
                    MessageBox.Show("Confirm password must be same as password");
                    passwordBoxConfirm.Focus();
                    return;
                }
                else
                {
                    int flag = ca_model.createAccount(textBoxUserName.Text, textBoxPhone.Text,
                        textBoxEmail.Text, passwordBox.Password, textBoxAddress.Text, age, gender, choosenFriends, choosenLanguages);
                    if(flag == 0)
                    {
                        HomePage up = new HomePage();
                        this.NavigationService.Navigate(up);
                    }
                    if (flag == 1)
                    {
                        MessageBox.Show("username is already exist");
                        return;
                    }
                    if (flag == 2)
                    {
                        MessageBox.Show("Cannot create account, please try again");
                        return;
                    }
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
    }
}
