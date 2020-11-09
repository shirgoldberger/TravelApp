using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

enum Type
{
    Friends, Properties, Locations
}
namespace TravelApp
{
    /// <summary>
    /// Interaction logic for addNewTrip.xaml
    /// </summary>
    public partial class addNewTrip : Page
    {
        
        private ArrayList objLocationList;
        private ArrayList objPropertiesList;
        private ArrayList objFriendsList;
        private List<string> friends;
        private List<string> locations;
        private List<string> properties;
        private List<string> choosenLocations;
        private List<string> choosenProperties;
        private List<string> choosenFriends;
        public class MyLocation
        {
            public int LocationID
            {
                get;
                set;
            }
            public string LocationString
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

        public class MyFriend
        {
            public int FriendID
            {
                get;
                set;
            }
            public string FriendString
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

        public class MyProperty
        {
            public int PropID
            {
                get;
                set;
            }
            public string ProprtyString
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
        public addNewTrip()
        {
            InitializeComponent();
            choosenLocations = new List<string>();
            choosenProperties = new List<string>();
            choosenFriends = new List<string>();
            friends = new List<string>() { "Gilad", "Ben", "Yael", "Rinat", "Ehud" };
            locations = new List<string>() { "Paris", "Eilat", "Roma", "London", "Haifa" };
            properties = new List<string>() { "Adventurous", "Organized", "Cultural" };
            objLocationList = InsertElementsInList(locations, Type.Locations);
            objFriendsList = InsertElementsInList(friends, Type.Friends);
            objPropertiesList = InsertElementsInList(properties, Type.Properties);
            BindCBDropDown<MyProperty>(propComboBox, objPropertiesList);
            BindCBDropDown<MyLocation>(locationComboBox, objLocationList);
            BindCBDropDown<MyFriend>(friendsComboBox, objFriendsList);
        }

        private ArrayList InsertElementsInList(List<string> inputList, Type type)
        {
            ArrayList returnedList = new ArrayList();
            int counter = 0;
            foreach (string element in inputList)
            {
                switch(type)
                {
                    case Type.Friends:
                        MyFriend friend = new MyFriend();
                        friend.FriendID = counter++;
                        friend.FriendString = element;
                        returnedList.Add(friend);
                        break;
                    case Type.Locations:
                        MyLocation location = new MyLocation();
                        location.LocationID = counter++;
                        location.LocationString = element;
                        returnedList.Add(location);
                        break;
                    case Type.Properties:
                        MyProperty prop = new MyProperty();
                        prop.PropID = counter++;
                        prop.ProprtyString = element;
                        returnedList.Add(prop);
                        break;
                }
            }
            return returnedList;
        }

        private void BindCBDropDown<T>(ComboBox cb, ArrayList objList)
        {
            cb.ItemsSource = objList;
        }

        private void MyCheckedAndUncheckedProps(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var prop = baseobj.DataContext as MyProperty;
            BindListBox<MyProperty>(prop, choosenProperties, Type.Properties);
        }

        private void MyCheckedAndUncheckedLocations(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var location = baseobj.DataContext as MyLocation;
            BindListBox<MyLocation>(location, choosenLocations, Type.Locations);
        }

        private void MyCheckedAndUncheckedFriends(object sender, RoutedEventArgs e)
        {
            var baseobj = sender as FrameworkElement;
            var friend = baseobj.DataContext as MyFriend;
            BindListBox<MyFriend>(friend, choosenFriends, Type.Friends);
        }

        private void BindListBox<T>(T currObject, List<string> inputList, Type type)
        {
            switch (type)
            {
                case Type.Friends:
                    var friend = currObject as MyFriend;
                    if (friend.Check_Status)
                    {
                        inputList.Add(friend.FriendString);
                    }
                    else
                    {
                        inputList.Remove(friend.FriendString);
                    }
                    break;
                case Type.Locations:
                    var location = currObject as MyLocation;
                    if (location.Check_Status)
                    {
                        inputList.Add(location.LocationString);
                    }
                    else
                    {
                        inputList.Remove(location.LocationString);
                    }
                    break;
                case Type.Properties:
                    var prop = currObject as MyProperty;
                    if (prop.Check_Status)
                    {
                        inputList.Add(prop.ProprtyString);
                    }
                    else
                    {
                        inputList.Remove(prop.ProprtyString);
                    }
                    break;
            }
            
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        public void Reset()
        {
            tripDates.SelectedDate = null;
            tripDates.DisplayDate = DateTime.Today;
            tripNameVal.Text = "";
            minParticipantsVal.Text = "";
            maxParticipantsVal.Text = "";
            resetComboBox(friendsComboBox, choosenFriends, objFriendsList, Type.Friends);
            resetComboBox(locationComboBox, choosenLocations, objLocationList, Type.Locations);
            resetComboBox(propComboBox, choosenProperties, objPropertiesList, Type.Properties);
        }

        private void resetComboBox(ComboBox cb, List<string> lst, ArrayList objList, Type type)
        {
            
            switch (type)
            {
                case Type.Friends:
                    
                    foreach (var friend in objList)
                    {
                        var friendCasted = friend as MyFriend;
                        friendCasted.Check_Status = false;
                    }
                    break;
                case Type.Locations:
                    foreach (var location in objList)
                    {
                        var locationCasted = location as MyLocation;
                        locationCasted.Check_Status = false;
                    }
                    break;
                case Type.Properties:
                    foreach (var prop in objList)
                    {
                        var propCasted = prop as MyProperty;
                        propCasted.Check_Status = false;
                    }
                    break;
            }
            lst.Clear();
            cb.SelectedIndex = -1;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            HomePage login = new HomePage();
            this.NavigationService.Navigate(login);
        }

        private void Friend_TextChanged(object sender, EventArgs e)
        {
            friendsComboBox.ItemsSource = objFriendsList.Cast<MyFriend>().ToList().Where(x => x.FriendString.StartsWith(friendsComboBox.Text.Trim()));
        }

        private void Properties_TextChanged(object sender, EventArgs e)
        {
            propComboBox.ItemsSource = objPropertiesList.Cast<MyProperty>().ToList().Where(x => x.ProprtyString.StartsWith(propComboBox.Text.Trim()));
        }

        private void Location_TextChanged(object sender, EventArgs e)
        {
            locationComboBox.ItemsSource = objLocationList.Cast<MyLocation>().ToList().Where(x => x.LocationString.StartsWith(locationComboBox.Text.Trim()));
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (tripDates.SelectedDate == null)
            {
                errormessage.Text = "Enter dates of the trip";
                tripDates.Focus();
                return;
            }
            if (choosenLocations.Count == 0)
            {
                errormessage.Text = "Enter location for the trip";
                locationComboBox.Focus();
                return;
            }
            if (choosenProperties.Count == 0)
            {
                errormessage.Text = "Enter properties of the trip";
                propComboBox.Focus();
                return;
            }
            if (minParticipantsVal.Text == "" || maxParticipantsVal.Text == "")
            {
                errormessage.Text = "Enter number of participants for the trip";
                minParticipantsVal.Focus();
                return;
            }
            else
            {
                try
                {
                    int min = int.Parse(minParticipantsVal.Text);
                    int max = int.Parse(maxParticipantsVal.Text);
                    if(min < 1 || min > max)
                    {
                        errormessage.Text = "Enter valid number of participants for the trip";
                        minParticipantsVal.Focus();
                        return;
                    }
                }
                catch
                {
                    errormessage.Text = "Enter valid number of participants for the trip";
                    minParticipantsVal.Focus();
                    return;
                }
            }
            if(tripNameVal.Text == "")
            {
                errormessage.Text = "Enter the name of the trip";
                tripNameVal.Focus();
                return;
            }
        }
        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {

        }

        private void maleChecked(object sender, RoutedEventArgs e)
        {
            female.IsChecked = false;
        }

        private void femaleChecked(object sender, RoutedEventArgs e)
        {
            male.IsChecked = false;
        }
    }
}
