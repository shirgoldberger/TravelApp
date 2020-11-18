using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class Trip
    {
        string id;
        DateTime start_date;
        DateTime end_date;
        int min_age;
        int max_age;
        int max_participants;
        bool male_only;
        bool female_only;

        public Trip(string _id, DateTime _start_date, DateTime _end_date, int _min_age,
        int _max_age, int _max_participants, bool _male_only, bool _female_only)
        {
            id = _id;
            start_date = _start_date;
            end_date = _end_date;
            min_age = _min_age;
            max_age = _max_age;
            max_participants = _max_participants;
            male_only = _male_only;
            female_only = _female_only;
        }

    }
}
