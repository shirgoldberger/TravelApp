using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp
{
    class watchTrup_Controller
    {
        Trip trip;
       // watchTrip_Model model;

        public watchTrup_Controller(Trip trip) {
            this.trip = trip;
           // model = new watchTrip_Model(trip);
        }
        public List<string> getMem()
        {
            return UsersModel.Instance.getAllMembersInTrip(trip);
        }
        public List<Attraction> getAtt()
        {
            return AttractionsModel.Instance.getAllAttractionOfTrip(trip);
        }
    }
}
