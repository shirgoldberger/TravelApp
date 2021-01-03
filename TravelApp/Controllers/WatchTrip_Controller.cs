using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp
{
    class WatchTrip_Controller
    {
        Trip trip;
       // watchTrip_Model model;

        public WatchTrip_Controller(Trip trip) {
            this.trip = trip;
           // model = new watchTrip_Model(trip);
        }
        public Tuple<bool, List<string>> getMem()
        {
            return UsersModel.Instance.getAllMembersInTrip(trip);
        }
        public Tuple<bool, List<Attraction>> getAtt()
        {
            return AttractionsModel.Instance.getAllAttractionOfTrip(trip);
        }
    }
}
