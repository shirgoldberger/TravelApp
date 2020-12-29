using Google.Protobuf.WellKnownTypes;
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
using TravelApp.Models;
using TravelApp.Objects;
using TravelApp.Pages;

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
        private List<Attraction> inputedAttractions;
        private List<Attraction> choosenAttractions;
        private List<User> inputedParticipants;
        private List<User> choosenMaleParticipants;
        private List<User> choosenFemaleParticipants;
        private List<User> choosenParticipants;
        private CreateTrip_Controller controller;
        private bool maleOnly;
        private bool femaleOnly;
        private bool isAdminMale;
        private User admin;
        private DateTime startDate;
        private DateTime endDate;
        private int minChoosenAge;
        private int maxChoosenAge;
        private string user_tripName;
        private string user_maxAge;
        private string user_minAge;
        private bool user_genderOnly;
        private string user_maxParticipants;
        private bool assignDates;
        private bool creation;
        private int trip_code;
        private Trip inputedtrip;
        private Page calledPage;

        public addNewTrip(Page page, Trip trip, List<Attraction> attractions, List<User> participants)
        {
            InitializeComponent();
            inputedtrip = trip;

            controller = new CreateTrip_Controller();
            setAdminAndGender(trip.Admin);
            
            choosenAttractions = attractions;
            tripAttractions.ItemsSource = null;
            tripAttractions.ItemsSource = choosenAttractions;
            inputedAttractions = choosenAttractions.ToList();

            choosenMaleParticipants = new List<User>();
            choosenFemaleParticipants = new List<User>();
            choosenParticipants = participants;
            tripParticipants.ItemsSource = null;
            tripParticipants.ItemsSource = choosenParticipants;
            foreach (User user in choosenParticipants)
            {
                if (user.Is_male == '1')
                {
                    choosenMaleParticipants.Add(user);
                }
                else
                {
                    choosenFemaleParticipants.Add(user);
                }
            }
            inputedParticipants = choosenParticipants.ToList();

            maleOnly = trip.Male_Only;
            femaleOnly = trip.Female_Only;
            user_tripName = trip.Name;
            user_maxAge = trip.Max_Age.ToString();
            user_minAge = trip.Min_Age.ToString();
            user_genderOnly = maleOnly || femaleOnly;
            user_maxParticipants = trip.Max_Participants.ToString() ;
            assignDates = true;
            createButton.Content = "Update Trip";
            startDate = trip.Start_Date;
            endDate = trip.End_Date;
            creation = false;
            trip_code = trip.Id;
            calledPage = page;
    }

        public addNewTrip(string username)
        {
            InitializeComponent();
            controller = new CreateTrip_Controller();
            choosenAttractions = new List<Attraction>();
            choosenMaleParticipants = new List<User>();
            choosenFemaleParticipants = new List<User>();
            choosenParticipants = new List<User>();
            maleOnly = false;
            femaleOnly = false;
            setAdminAndGender(username);
            
            user_tripName = "";
            user_maxAge = "";
            user_minAge = "";
            user_genderOnly = false;
            user_maxParticipants = "";
            assignDates = false;
            creation = true;
    }

        private void setMinAndMaxAge()
        {
            minChoosenAge = maxChoosenAge = admin.Age;
            foreach (User user in choosenParticipants)
            {
                int age = user.Age;
                if(age > maxChoosenAge)
                {
                    maxChoosenAge = age;
                }
                else if(minChoosenAge > age)
                {
                    minChoosenAge = age;
                }
            }
        }

        private void setAdminAndGender(string username)
        {
            Tuple<bool, User> tuple = controller.getUserByName(username);
            if(!tuple.Item1)
            {
                Utils.errorAndExit("Error trying to access users records");
            }
            admin = tuple.Item2;
            isAdminMale = admin.Is_male == '1';
        }


        private void addPartsClick(object sender, RoutedEventArgs e)
        {
            int max = -1;
            if(user_maxParticipants != "")
            {
                max = int.Parse(user_maxParticipants);
            }
            int minAgeArg = -1, maxAgeArg = -1;
            if(user_minAge != "")
            {
                minAgeArg = int.Parse(user_minAge);
            }
            if (user_maxAge != "")
            {
                maxAgeArg = int.Parse(user_maxAge);
            }
            addNewMembersToTrip anm = new addNewMembersToTrip(choosenParticipants, admin.Username, maleOnly, femaleOnly, max, minAgeArg, maxAgeArg);
            anm.FemalesAdded = null;
            anm.MalesAdded = null;
            anm.ShowDialog();
            bool changed = false;
            if(anm.FemalesAdded != null && anm.FemalesAdded.Count > 0)
            {
                choosenFemaleParticipants.AddRange(anm.FemalesAdded);
                changed = true;
            }
            if (anm.MalesAdded != null && anm.MalesAdded.Count > 0)
            {
                choosenMaleParticipants.AddRange(anm.MalesAdded);
                changed = true;
            }
            if(changed)
            {
                choosenParticipants = choosenFemaleParticipants.ToList();
                choosenParticipants.AddRange(choosenMaleParticipants);
                tripParticipants.ItemsSource = null;
                tripParticipants.ItemsSource = choosenParticipants;
            }
        }

        private void setTripPropClick(object sender, RoutedEventArgs e)
        {
            setMinAndMaxAge();
            bool containBoth = choosenFemaleParticipants.Count > 0 && choosenMaleParticipants.Count > 0;
            SetTripProperties stp = new SetTripProperties(admin, containBoth, minChoosenAge, maxChoosenAge, choosenParticipants.Count + 1);
            stp.TripName = user_tripName;
            stp.MaxAge = user_maxAge;
            stp.MinAge = user_minAge;
            stp.GenderOnly = user_genderOnly;
            stp.MaxParticipants = user_maxParticipants;
            stp.Changed = false;
            if(assignDates)
            {
                stp.TripDates = new Tuple<DateTime, DateTime>(startDate, endDate);
            }
            stp.ShowDialog();
            if(stp.Changed)
            {
                user_tripName = stp.TripName;
                user_maxAge = stp.MaxAge;
                user_minAge = stp.MinAge;
                user_genderOnly = stp.GenderOnly;
                if(user_genderOnly)
                {
                    if(isAdminMale)
                    {
                        maleOnly = true;
                    }
                    else
                    {
                        femaleOnly = true;
                    }
                }
                else
                {
                    maleOnly = false;
                    femaleOnly = false;
                }
                startDate = stp.TripDates.Item1;
                endDate = stp.TripDates.Item2;
                assignDates = true;
                user_maxParticipants = stp.MaxParticipants;
            }
        }

        private void addAttractionsClick(object sender, RoutedEventArgs e)
        {
            AddNewAttForTrip ant = new AddNewAttForTrip(choosenAttractions);
            ant.ReturenedAttractions = null;
            ant.ShowDialog();
            List<Attraction> returned = ant.ReturenedAttractions;
            if (returned != null)
            {
                choosenAttractions.AddRange(returned);
                tripAttractions.ItemsSource = null;
                tripAttractions.ItemsSource = choosenAttractions;
            }

        }

        private bool isFieldsWrong()
        {
            if(choosenAttractions.Count == 0)
            {
                MessageBox.Show("Please enter attractions for the trip");
                return true;
            }
            if(user_tripName == "" || user_maxAge == "" || user_minAge == "" || user_maxParticipants == "" || !assignDates)
            {
                MessageBox.Show("Please enter the trip properties");
                return true;
            }
            return false;
        }

        private void createTrip(TripToAdd trip)
        {
            controller.generateTrip(trip, choosenParticipants, choosenAttractions);
            MessageBox.Show("Trip was added sccessfully");
            this.NavigationService.GoBack();
        }

        private void updateTrip(int minAge, int maxAge, int maxParts, string startConverted, string endConverted, bool maleOnly, bool femaleOnly)
        {
            List<User> partsToRemove = inputedParticipants.Except(choosenParticipants).ToList();
            List<User> partsToAdd = choosenParticipants.Except(inputedParticipants).ToList();

            List<Attraction> attractionsToRemove = inputedAttractions.Except(choosenAttractions).ToList();
            List<Attraction> attractionsToAdd = choosenAttractions.Except(inputedAttractions).ToList();

            controller.updateTrip(inputedtrip, user_tripName, minAge, maxAge, maxParts, startConverted, endConverted, maleOnly, femaleOnly, partsToRemove, partsToAdd, attractionsToRemove, attractionsToAdd);
            MessageBox.Show("Trip was updated sccessfully");
            (calledPage as viewAllTrip).Updated = true;
            this.NavigationService.GoBack();
        }

        private void createTripClick(object sender, RoutedEventArgs e)
        {
            if(isFieldsWrong())
            {
                return;
            }
            int minAge = int.Parse(user_minAge);
            int maxAge = int.Parse(user_maxAge);
            string adminName = admin.Username;
            int maxParts = int.Parse(user_maxParticipants);
            string startConverted = startDate.ToString("yyyy-MM-dd");
            string endConverted = endDate.ToString("yyyy-MM-dd");
            if (creation)
            {
                TripToAdd trip = new TripToAdd(user_tripName, adminName, startConverted, endConverted, minAge, maxAge, maxParts, maleOnly, femaleOnly);
                createTrip(trip);
            }
            else
            {
                updateTrip(minAge, maxAge, maxParts, startConverted, endConverted, maleOnly, femaleOnly);
            }
            
            
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void removeAttClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = tripAttractions.Items.IndexOf(item);
            choosenAttractions.RemoveAt(itemIndex);
            tripAttractions.ItemsSource = null;
            tripAttractions.ItemsSource = choosenAttractions;
        }

        private void removeParticipantClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = tripParticipants.Items.IndexOf(item);
            User selected = choosenParticipants[itemIndex];
            if (selected.Is_male == '1')
            {
                choosenMaleParticipants.Remove(selected);
            }
            else
            {
                choosenFemaleParticipants.Remove(selected);
            }
            choosenParticipants.RemoveAt(itemIndex);
            tripParticipants.ItemsSource = null;
            tripParticipants.ItemsSource = choosenParticipants;
        }
    }
}
