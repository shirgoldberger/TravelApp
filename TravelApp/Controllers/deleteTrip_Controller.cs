using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;


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
        public Tuple<int, string> Click_All()
        {
            int i = -1;
            string msg = "";
            bool a = TripsModel.Instance.delteAllTripMember(trip);
            if (a == false)
            {
                i = 0;
                msg = "delete failed";

            }
            if (a == true)
            {
                i = 1;
                msg = "delete sucses";
            }
            return Tuple.Create(i, msg);
        }
        public Tuple<int, string> row_click(string mem)
        {
            int i = -1;
            string msg = "";
            bool a = TripsModel.Instance.setUserAdmin(trip, username, mem);
            if (a == true)
            {
                i = 0;
                msg = "you deleted from trip and " + mem + " is the new admin.";
            }
            else
            {
                msg = "delete faild";
                i = 1;
            }
            return Tuple.Create(i, msg);

        }
    }
}

