using System;
using System.Linq;
using System.Windows;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class SetTripProperties : Window
    {
        private int minSoFar;
        private int maxSoFar;
        private int currParts;
        private bool genderOnly;
        private string minAgeProp;
        private string maxAgeProp;
        private string maxPartsProp;
        private string tripNameProp;
        private Tuple<DateTime, DateTime> tripDatesProp;

        public bool GenderOnly
        {
            set
            {
                genderOnly = value;
                gender.IsChecked = value;
            }
            get
            {
                return genderOnly;
            }
        }

        public string MinAge
        {
            set
            {
                minAgeProp = value;
                minAge.Text = value;
            }
            get
            {
                return minAgeProp;
            }
        }

        public string MaxAge
        {
            set
            {
                maxAgeProp = value;
                maxAge.Text = value;
            }
            get
            {
                return maxAgeProp;
            }
        }

        public string MaxParticipants
        {
            set
            {
                maxPartsProp = value;
                maxParts.Text = value;
            }
            get
            {
                return maxPartsProp;
            }
        }

        public string TripName
        {
            set
            {
                tripNameProp = value;
                tripName.Text = value;
            }
            get
            {
                return tripNameProp;
            }
        }

        public Tuple<DateTime, DateTime> TripDates
        {
            set
            {
                tripDatesProp = value;
                DateTime start = value.Item1;
                DateTime end = value.Item2;
                tripDates.SelectedDates.AddRange(start, end);
                tripDates.DisplayDate = start;
            }
            get
            {
                return tripDatesProp;
            }
        }

        public bool Changed { set; get; }

        public SetTripProperties(User admin, bool containBoth, int _minSoFar, int _maxSoFar, int _currParts)
        {
            InitializeComponent();
            minSoFar = _minSoFar;
            maxSoFar = _maxSoFar;
            currParts = _currParts;
            gender.Content = admin.Is_male == '1' ? "Is Male Only" : "Is Female Only";
            if(containBoth)
            {
                gender.IsEnabled = false;
            }
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            tripName.Text = TripName;
            maxAge.Text = MaxAge;
            minAge.Text = MinAge;
            gender.IsChecked = GenderOnly;
            maxParts.Text = MaxParticipants;
            Tuple<DateTime, DateTime> tuple = TripDates;
            DateTime start = tuple.Item1;
            DateTime end = tuple.Item2;
            tripDates.SelectedDates.Clear();
            tripDates.SelectedDates.AddRange(start, end);
        }

        private bool wrongValues()
        {
            int min = 0;
            try
            {
                min = int.Parse(minAge.Text);
                if(min < 0)
                {
                    MessageBox.Show("Please enter positive number for min age");
                    return true;
                }
                if (min > minSoFar)
                {
                    MessageBox.Show("There is already participant which his age is less than your input");
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a number in min age box");
                return true;
            }

            try
            {
                int max = int.Parse(maxAge.Text);
                if (max < 0)
                {
                    MessageBox.Show("Please enter positive number for max age");
                    return true;
                }
                if(max < min)
                {
                    MessageBox.Show("Max age value has cannot be less min age value");
                    return true;
                }
                if (max < maxSoFar)
                {
                    MessageBox.Show("There is already participant which his age is more than your input");
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a number in max age box");
                return true;
            }

            try
            {
                int max = int.Parse(maxParts.Text);
                if(max < 0)
                {
                    MessageBox.Show("please enter positive number for max participants");
                    return true;
                }
                if (max < currParts)
                {
                    MessageBox.Show("There is already more participants than your input");
                    return true;
                }
            }
            catch
            {
                MessageBox.Show("Please enter a number in max number of participants box");
                return true;
            }

            if(tripName.Text == "")
            {
                MessageBox.Show("Please enter a name for the trip");
                return true;
            }

            if(!tripDates.SelectedDate.HasValue)
            {
                MessageBox.Show("Please enter dates for the trip");
                return true;
            }
            return false;
        }

        private void setClick(object sender, RoutedEventArgs e)
        {
            if(wrongValues())
            {
                return;
            }
            TripName = tripName.Text;
            MaxAge = maxAge.Text;
            MinAge = minAge.Text;
            MaxParticipants = maxParts.Text;
            if(gender.IsChecked.HasValue)
            {
                GenderOnly = gender.IsChecked.Value;
            }
            if (tripDates.SelectedDate.HasValue)
            {
                DateTime start = tripDates.SelectedDates[0];
                int len = tripDates.SelectedDates.Count();
                DateTime end = tripDates.SelectedDates[len - 1];
                TripDates = new Tuple<DateTime, DateTime>(start, end);
            }
            MessageBox.Show("Trip's properties were updated sucessfully");
            Changed = true;
            GetWindow(this).Close();
        }
    }
}
