using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class Trip
    {
        string id;
        string admin;
        DateTime start_date;
        DateTime end_date;
        int min_age;
        int max_age;
        int max_participants;
        bool male_only;
        bool female_only;
        public Trip(string _id, string _admin, DateTime _start_date, DateTime _end_date, int _min_age,
        int _max_age, int _max_participants, bool _male_only, bool _female_only)
        {
            id = _id;
            admin = _admin;
            start_date = _start_date;
            end_date = _end_date;
            min_age = _min_age;
            max_age = _max_age;
            max_participants = _max_participants;
            male_only = _male_only;
            female_only = _female_only;
        }

        public string Id
        {
            get
            {
                return id;
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

    }
}
