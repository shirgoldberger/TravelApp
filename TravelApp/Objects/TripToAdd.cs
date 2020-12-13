using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class TripToAdd
    {
        string name;
        string admin;
        string start_date;
        string end_date;
        int min_age;
        int max_age;
        int max_participants;
        bool male_only;
        bool female_only;
        public TripToAdd(string _name, string _admin, string _start_date, string _end_date, int _min_age,
        int _max_age, int _max_participants, bool _male_only, bool _female_only)
        {
            name = _name;
            admin = _admin;
            start_date = _start_date;
            end_date = _end_date;
            min_age = _min_age;
            max_age = _max_age;
            max_participants = _max_participants;
            male_only = _male_only;
            female_only = _female_only;
            
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
        public string Start_Date
        {
            get
            {
                return start_date;
            }
        }
        public string End_Date
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
