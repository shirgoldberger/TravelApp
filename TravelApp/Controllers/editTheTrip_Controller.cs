using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class editTheTrip_Controller
    {

        editTheTrip_Model model;
        Trip trip;
        public editTheTrip_Controller(Trip trip)
        {
            this.trip = trip;
            model = new editTheTrip_Model(trip);

        }
        public List<string> getAllMembers()
        {
            return model.getAllMembers();
        }
        public List<Attraction> getAllAttraction()
        {
            return model.getAllAttraction();
        }
        public Tuple<int, string> Button_Click_Submit( string admin, string start_date, string end_date, string min_age, string max_age, string max_part)
        {
            int i;
            string msg;
            bool ans = model.update_submit(trip.Id,admin, start_date, end_date, min_age, max_age, max_part);
            if (ans == true)
            {
                i = 0;
                msg = "edit sucses";

            }
            else
            {
                i = 1;
                msg = "edit faild";
            }
            return Tuple.Create(i, msg);
        }
        public Tuple<int, string> delete_mem(string user)
        {
            int i;
            string msg;
            bool delete = model.deleteMem(trip, user);
            if (delete == true)
            {
                i = 0;
               msg = "delete sucses";
            }
            else
            {
                i = 1;
                msg ="delete failed";

            }
            return Tuple.Create(i, msg);

        }
        public Tuple<int, string> delete_att(Attraction att)
        {
            int i;
            string msg;
            bool delete = model.deleteAtt(trip, att);
            if (delete == true)
            {
                i = 0;
                msg = "delete sucses";
            }
            else
            {
                i = 1;
                msg = "delete failed";

            }
            return Tuple.Create(i, msg);

        }


    }
}
