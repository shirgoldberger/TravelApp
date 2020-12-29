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
using System.Windows.Shapes;
using TravelApp.Controllers;
using TravelApp.Models;
using TravelApp.Objects;

namespace TravelApp
{
    /// <summary>
    /// Interaction logic for add_new_mem_for_trip.xaml
    /// </summary>
    public partial class addNewMembersToTrip : Window
    {
        private List<User> members;
        private string admin;
        private List<User> drop;
        private bool maleOnly;
        private bool femaleOnly;
        private int minAge;
        private int maxAge;
        private int maxParts;
        private AddNewMembers_Controller controller;
        private List<User> malesAdded;
        private List<User> femalesAdded;
        private int left;

        public List<User> MalesAdded { set;get; }
        public List<User> FemalesAdded { set; get; }

        public addNewMembersToTrip(List<User> _drop, string _admin, bool _maleOnly, bool _femaleOnly, int _max_parts, int _minAge, int _maxAge)
        {
            InitializeComponent();
            controller = new AddNewMembers_Controller();
            admin = _admin;
            drop = _drop;
            maleOnly = _maleOnly;
            femaleOnly = _femaleOnly;
            maxParts = _max_parts;
            malesAdded = new List<User>();
            femalesAdded = new List<User>();
            minAge = _minAge;
            maxAge = _maxAge;
            update();
        }

        private async Task<List<User>> getRestMembersAsync(List<User> dropped)
        {
            Tuple< bool, List<User>> tuple = await Task.Run(() => controller.getRestFriends(dropped, admin, minAge, maxAge, femaleOnly, maleOnly));
            if(!tuple.Item1)
            {
                Utils.errorAndExit("Error trying access users records");
            }
            List<User> list = tuple.Item2;
            return list;
        }

        private async void update()
        {
            List<User> ignore = drop.ToList();
            ignore.AddRange(femalesAdded);
            ignore.AddRange(malesAdded);
            left = maxParts - ignore.Count - 1;
            setLeftText();
            members = await getRestMembersAsync(ignore);
            usersList.ItemsSource = null;
            usersList.ItemsSource = members;
        }

        private void clickAdd(object sender, RoutedEventArgs e)
        {
            if(maxParts != -1 && left == 0)
            {
                MessageBox.Show("There was no place for adding new members to your trip");
                return;
            }
            var item = ((Button)sender).DataContext;
            int itemIndex = usersList.Items.IndexOf(item);
            User userToAdd = members[itemIndex];
            if(userToAdd.Is_male == '1')
            {
                malesAdded.Add(userToAdd);
            }
            else
            {
                femalesAdded.Add(userToAdd);
            }
            update();
        }

        private void setLeftText()
        {
            leftParticipant.Text = "There was left " + left.ToString() + " participants to add at the most";
            if (maxParts == -1)
            {
                leftParticipant.Text = "There is no limit for adding participants, add as much as you want !";
            }
        }

        private void addMembers(object sender, RoutedEventArgs e)
        {
            MalesAdded = malesAdded;
            FemalesAdded = femalesAdded;
            GetWindow(this).Close();
        }
    }
}

