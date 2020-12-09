using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class add_new_att_for_trip_Controller
    {
        editTheTrip_Model model;
        Trip trip;
        public add_new_att_for_trip_Controller(Trip trip)
        {
            model = new editTheTrip_Model(trip);
            this.trip = trip;
        }
        public List<Attraction> getAllAttractionNotInThisTrip(Trip trip)
        {
            return model.getAllAttractionNotInThisTrip(trip);

        }
        public (int, string) click_add(Attraction att)
        {
            int i;
            string msg;
            bool a = model.add_new_Att_for_trip(trip, att);
            if (a == true)
            {
                msg = "add sucses";
                i = 0;
            }
            else
            {
                msg = "add failed";
                i = 1;
            }
            return (i, msg);

        }
    }
}

