using System;
using System.Collections.Generic;
using TravelApp.Models;


namespace TravelApp
{
    class DeleteTrip_Controller
    {
        Trip trip;
        string username;
        public DeleteTrip_Controller(Trip trip, string username)
        {
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

