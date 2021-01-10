using System;
using System.Windows;
using TravelApp.Objects;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class UserDetails : Window
    {

        public UserDetails(string username, WatchTrip_Controller controller)
        {
            InitializeComponent();
            initFields(controller, username);
        }

        private void initFields(WatchTrip_Controller controller, string _username)
        {
            Tuple<bool, User> tuple = controller.getUserDetails(_username);
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying access users records");
            }
            User user = tuple.Item2;
            age.Text = user.Age.ToString();
            mail.Text = user.Mail;
            gender.Text = user.Is_male == '1' ? "Male" : "Female";
            phone.Text = user.Phone;
            username.Text = user.Username;
        }

    
    }
}
