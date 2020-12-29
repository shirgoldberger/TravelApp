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
        public  string Click_All()
        {
            string msg = "";
            bool a = TripsModel.Instance.delteAllTripMember(trip);
            if (a == false)
            {
                //"delete fauled"
                Utils.errorAndExit("delete failed");

            }
            if (a == true)
            {
                msg = "delete sucses";
            }
            return  msg;
        }
        public string row_click(string mem)
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
                //אני לא בטוחה שהתוכנית צריכה להסתיים עכשיו, אולי עדיף רק לשים הודעה שהמחיקה נכשלה.
                Utils.errorAndExit("delete failed");
            }
            return  msg;

        }
    }
}

