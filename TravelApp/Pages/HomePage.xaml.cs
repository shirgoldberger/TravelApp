using System.ComponentModel;
using System.Windows;
using TravelApp.Objects;


namespace TravelApp
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : INotifyPropertyChanged
    {
        private string username = "";
        private string password = "";
        private string message = "";
        private HomePage_Controller controller;
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            controller = new HomePage_Controller();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        private void Button_Click_Login(object sender, RoutedEventArgs e)
        {
            var tuple = controller.click_Login(name.Text, passwordBox.Text);
            switch (tuple.Item1)
            {
                case 0:
                    UserPage userpage = new UserPage(username);
                    this.NavigationService.Navigate(userpage);
                    break;
                case 1:
                    MessageBox.Show(tuple.Item2);
                    break;
                case 2:
                    Utils.Instance.errorAndExit(tuple.Item2);
                    break;

            }

        }

        private void Button_Click_New_Account(object sender, RoutedEventArgs e)
        {
            CreateAccount ca = new CreateAccount();
            this.NavigationService.Navigate(ca);
        }
        public string Username
        {
            get {  return this.username; }
            set {
                this.username = value;
                NotifyPropertyChanged("Username");
            }
        }
        public string Password
        {
            get { return this.password;   }
            set
            {
                this.password = value;
                NotifyPropertyChanged("Password");
            }
        }
        public string Message
        {
            get { return this.message; }
            set
            {
                this.message = value;
                NotifyPropertyChanged("Message");
            }
        }


        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            // close the program
            System.Environment.Exit(0);
        }

        private void Username_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            Message = "";
        }
    }
}
