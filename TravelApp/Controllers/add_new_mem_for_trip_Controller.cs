using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class add_new_mem_for_trip_Controller
    {
            editTheTrip_Model model;
            Trip trip;
            public add_new_mem_for_trip_Controller(Trip trip)
            {
                model = new editTheTrip_Model(trip);
                this.trip = trip;
            }
            public List<string> getAllMembersNOtInTHisTrip()
            {
                return model.getAllMembersNOtInTHisTrip(trip);
            }
            public (int, string) Click_add(string mem)
            {
                string msg;
                int i;

                bool a = model.add_new_Mem_for_trip(trip, mem);
                if (a == true)
                {
                    msg = "delete sucses";
                    i = 0;

                }
                else
                {
                    msg = "delete failed";
                    i = 1;

                }
                return (i, msg);

            }
        }
    }

