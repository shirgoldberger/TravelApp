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
        private List<Language> languages;
        private List<Language> choosenLanguages;
        public CreateAccount()
        {
            InitializeComponent();
            ca_model = new CreateAccoutModel();
            choosenLanguages = new List<Language>();
            languages = ca_model.getLanguages();
            languagesComboBox.ItemsSource = languages;
        }

        private void MyCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var language = baseobj.DataContext as Language;
            BindListBox(language);
        }

        private void resetLanguages()
        {
            choosenLanguages.Clear();
            foreach (var language in languages)
            {
                language.Check_Status = false;
            }
            languagesComboBox.SelectedIndex = -1;
        }
        private void BindListBox(Language language)  
        {
            if(language.Check_Status)
            {
                choosenLanguages.Add(language);
            }
            else
            {
                choosenLanguages.Remove(language);
            }
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
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
                        textBoxEmail.Text, passwordBox.Password, textBoxAddress.Text, age, gender);
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
            languagesComboBox.ItemsSource = languages.Where(x => x.Name.StartsWith(languagesComboBox.Text.Trim()));
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // close the program
            System.Environment.Exit(0);
        }
    }
}
