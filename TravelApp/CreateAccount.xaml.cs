using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
        private List<MyLanguage> objLanguagesList;
        private List<string> languages;
        private List<string> choosenLanguages;
        public class MyLanguage
        {
            public int LanguageID
            {
                get;
                set;
            }
            public string LanguageString
            {
                get;
                set;
            }
            public Boolean Check_Status
            {
                get;
                set;
            }
        }
        public CreateAccount()
        {
            InitializeComponent();
            choosenLanguages = new List<string>();
            languages = new List<string>(){ "Hebrew", "English", "French", "Arabic", "Spanish" };
            objLanguagesList = InsertElementsInList(languages);
            BindLanguagesDropDown();
        }

        private List<MyLanguage> InsertElementsInList(List<string> languages)
        {
            List<MyLanguage> returnedList = new List<MyLanguage>();
            int counter = 0;
            foreach (string language in languages)
            {
                MyLanguage lang = new MyLanguage();
                lang.LanguageID = counter++;
                lang.LanguageString = language;
                returnedList.Add(lang);
            }
            return returnedList;
        }

        private void MyCheckedAndUnchecked(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var language = baseobj.DataContext as MyLanguage;
            BindListBox(language);
        }

        private void resetLanguages()
        {
            choosenLanguages.Clear();
            foreach (var language in objLanguagesList)
            {
                language.Check_Status = false;
            }
            languagesComboBox.SelectedIndex = -1;
        }
        private void BindListBox(MyLanguage language)  
        {
            if(language.Check_Status)
            {
                choosenLanguages.Add(language.LanguageString);
            }
            else
            {
                choosenLanguages.Remove(language.LanguageString);
            }
        }

        private void BindLanguagesDropDown()
        {
            languagesComboBox.ItemsSource = objLanguagesList;
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
                errormessage.Text = "Please choose languages";
                languagesComboBox.Focus();
            }
            if (male.IsChecked == false && female.IsChecked == false)
            {
                errormessage.Text = "Choose Gender";
                male.Focus();
            }
            if (textBoxEmail.Text.Length == 0)
            {
                errormessage.Text = "Enter an email.";
                textBoxEmail.Focus();
            }
            else if (!Regex.IsMatch(textBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                errormessage.Text = "Enter a valid email.";
                textBoxEmail.Select(0, textBoxEmail.Text.Length);
                textBoxEmail.Focus();
            }
            else
            {
                string firstname = textBoxUserName.Text;
                string lastname = textBoxPhone.Text;
                string email = textBoxEmail.Text;
                string password = passwordBox.Password;
                int age;
                try
                {
                    age = int.Parse(textBoxAge.Text);
                }
                catch
                {
                    errormessage.Text = "Enter valid age";
                    textBoxAge.Focus();
                }
                
                if (passwordBox.Password.Length == 0)
                {
                    errormessage.Text = "Enter password.";
                    passwordBox.Focus();
                }
                else if (passwordBoxConfirm.Password.Length == 0)
                {
                    errormessage.Text = "Enter Confirm password.";
                    passwordBoxConfirm.Focus();
                }
                else if (passwordBox.Password != passwordBoxConfirm.Password)
                {
                    errormessage.Text = "Confirm password must be same as password.";
                    passwordBoxConfirm.Focus();
                }
                else
                {
                    //errormessage.Text = "";
                    //string address = textBoxAddress.Text;
                    //SqlConnection con = new SqlConnection("Data Source=TESTPURU;Initial Catalog=Data;User ID=sa;Password=wintellect");
                    //con.Open();
                    //SqlCommand cmd = new SqlCommand("Insert into Registration (FirstName,LastName,Email,Password,Address) values('" + firstname + "','" + lastname + "','" + email + "','" + password + "','" + address + "')", con);
                    //cmd.CommandType = CommandType.Text;
                    //cmd.ExecuteNonQuery();
                    //con.Close();
                    //errormessage.Text = "You have Registered successfully.";
                    //Reset();
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

        private void languagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Languages_TextChanged(object sender, EventArgs e)
        {
            languagesComboBox.ItemsSource = objLanguagesList.Where(x => x.LanguageString.StartsWith(languagesComboBox.Text.Trim()));
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {

        }
    }
}
