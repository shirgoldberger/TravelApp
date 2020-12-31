using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;
using TravelApp.Objects;


namespace TravelApp
{
    class deleteTrip_Controller
    {
        //viewAllTrip_Model model;
        Trip trip;
        string username;
        public deleteTrip_Controller(Trip trip, string username)
        {
            //model = new viewAllTrip_Model(username);
            this.trip = trip;
            this.username = username;
        }
        public Tuple<bool, List<string>> getAllMembers()
        {
            return TripsModel.Instance.getAllMembersWithoutMe(trip, username);
        }
        public bool Click_All()
        {
            return TripsModel.Instance.delteAllTripMember(trip);
        }
        public bool row_click(string mem)
        {
            return TripsModel.Instance.setUserAdmin(trip, username, mem);
        }
    }
}

