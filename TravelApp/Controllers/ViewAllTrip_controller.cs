using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using TravelApp.Models;

namespace TravelApp
{
    class ViewAllTrip_controller
    {
        //viewAllTrip_Model model;
        string username;
        public ViewAllTrip_controller(string username )
        {
            this.username = username;
            //model = new viewAllTrip_Model(username);
        }
        public Tuple<bool, List<Trip>> getAllTrip()
        {
            return TripsModel.Instance.getTripsForUser(username, "IN"); 
        }
        public bool click_delete(Trip trip)
        {
            return TripsModel.Instance.deleteTrip(trip, username);
        }
        public Tuple<bool, List<User>> getAllMem(Trip trip)
        {
            return UsersModel.Instance.getAllMembers(trip, username);
        }
        public Tuple<bool, List<Attraction>> getAllAtt(Trip trip)
        {
            return AttractionsModel.Instance.getAllAttractionOfTrip(trip);
        }
    }
}
