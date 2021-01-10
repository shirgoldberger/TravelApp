using System;
using System.Collections.Generic;
using TravelApp.Models;

namespace TravelApp
{
    public class WatchTrip_Controller
    {
        private Trip trip;

        public WatchTrip_Controller(Trip trip) {
            this.trip = trip;
        }
        public Tuple<bool, List<string>> getMem()
        {
            return UsersModel.Instance.getAllMembersInTrip(trip);
        }
        public Tuple<bool, List<Attraction>> getAtt()
        {
            return AttractionsModel.Instance.getAllAttractionOfTrip(trip);
        }

        public Tuple<bool, User> getUserDetails(string username)
        {
            return UsersModel.Instance.getUserByName(username);
        }
    }
}
