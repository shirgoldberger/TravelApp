using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class viewAllTrip_controller
    {
        viewAllTrip_Model model;
        string username;
        public viewAllTrip_controller(string username)
        {
            this.username = username;
            model = new viewAllTrip_Model(username);
        }
        public List<Trip> getAllTrip()
        {
           return model.getAllTrip();
        }
        public Tuple<int, string> click_delete(Trip trip)
        {
            int i=0;
            string msg="";
            //if the username is the admin he need to choose - so he make the decision in tjr deleteTrip page
            if (username == trip.Admin)
            {
                deleteTrip delete = new deleteTrip(trip, username);
                delete.Show();
            }
            //just delete this username from trip - using the model.
            else
            {
                bool a = model.deleteTrip(trip, username);
                if (a == false)
                {
                    i = 1;
                    msg = "delete failed";

                }
                if (a == true)
                {
                    i = 0;
                   msg ="delete sucses";

                }
            }
            return Tuple.Create(i, msg);
        }
        public List<User> getAllMem(Trip trip)
        {
           return this.model.getAllMembers(trip);
        }
        public List<Attraction> getAllAtt(Trip trip)
        {
            return this.model.getAllAttraction(trip);
        }
    }
}
