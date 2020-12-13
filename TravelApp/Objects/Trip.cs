using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class Trip : INotifyPropertyChanged
    {
        int id;
        string name;
        string admin;
        DateTime start_date;
        DateTime end_date;
        int min_age;
        int max_age;
        int max_participants;
        bool male_only;
        bool female_only;
        string trip_string;
        string temp_trip_string;
        int member_amount;
        public Trip(int _id, string _name, string _admin, DateTime _start_date, DateTime _end_date, int _min_age,
        int _max_age, int _max_participants, bool _male_only, bool _female_only)
        {
            id = _id;
            name = _name;
            admin = _admin;
            start_date = _start_date;
            end_date = _end_date;
            min_age = _min_age;
            max_age = _max_age;
            max_participants = _max_participants;
            male_only = _male_only;
            female_only = _female_only;
            trip_string = "Name:" + name + "Start date: " + start_date.ToString() + ", End date: " + end_date.ToString();
            if (male_only)
            {
                trip_string += ",\ntrip to male only";
            } else if (female_only)
            {
                trip_string += ",\ntrip to female only";
            }
            member_amount = 1;
            temp_trip_string = trip_string;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));

            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Admin
        {
            get
            {
                return admin;
            }
        }
        public DateTime Start_Date
        {
            get
            {
                return start_date;
            }
        }
        public DateTime End_Date
        {
            get
            {
                return end_date;
            }
        }
        public int Min_Age
        {
            get
            {
                return min_age;
            }
        }
        public int Max_Age
        {
            get
            {
                return max_age;
            }
        }
        public int Max_Participants
        {
            get
            {
                return max_participants;
            }
        }
        public bool Male_Only
        {
            get
            {
                return male_only;
            }
        }
        public bool Female_Only
        {
            get
            {
                return female_only;
            }
        }
        public string Trip_String
        {
            get
            {
                if (max_participants != null)
                {
                    return (temp_trip_string + ", free space: " + (max_participants - member_amount).ToString());
                } 
                else
                {
                    return trip_string;
                }
            }
            set { trip_string = temp_trip_string; }
        }
        public int Member_Amount
        {
            get { return member_amount; }
            set
            {
                member_amount = value;

                NotifyPropertyChanged("Trip_String");
            }
        }

    }
}
