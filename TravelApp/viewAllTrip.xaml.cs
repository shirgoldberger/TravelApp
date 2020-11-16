using System;
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

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for viewAllTrip.xaml
    /// </summary>
    public partial class viewAllTrip : Page
    {
        private string username;
        private string password;
        public viewAllTrip(string username, string password)
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addNewTrip at = new addNewTrip(username, password);
            this.NavigationService.Navigate(at);
        }
    }
}
