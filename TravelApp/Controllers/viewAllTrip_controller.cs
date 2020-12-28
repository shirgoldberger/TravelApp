using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelApp.Models;

namespace TravelApp
{
    class viewAllTrip_controller
    {
        //viewAllTrip_Model model;
        string username;
        public viewAllTrip_controller(string username )
        {
            this.username = username;
            //model = new viewAllTrip_Model(username);
        }
        public Tuple<bool, List<Trip>> getAllTrip()
        {
            return TripsModel.Instance.getAllTripForUser(username); 
        }
        public Tuple<int, string> click_delete(Trip trip, Page pageToUpdate)
        {
            int i=0;
            string msg="";
            //if the username is the admin he need to choose - so he make the decision in tjr deleteTrip page
            if (username == trip.Admin)
            {
                deleteTrip delete = new deleteTrip(trip, username, pageToUpdate);
                delete.Show();

            }
            //just delete this username from trip - using the model.
            else
            {
                bool a = TripsModel.Instance.deleteTrip(trip, username); 
                if (a == false)
                {
                    i = 1;
                    msg = "delete failed";

                }
                if (a == true)
                {
                    i = 0;
                   msg ="delete sucses";
                   (pageToUpdate as viewAllTrip).Updated = true;


                }
            }
            return Tuple.Create(i, msg);
        }
        public List<User> getAllMem(Trip trip)
        {
            return UsersModel.Instance.getAllMembers(trip, username);
        }
        public List<Attraction> getAllAtt(Trip trip)
        {
            return AttractionsModel.Instance.getAllAttractionOfTrip(trip);
        }
    }
}
