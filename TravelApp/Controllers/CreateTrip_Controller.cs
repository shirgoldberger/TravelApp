using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TravelApp.Models
{
    public class CreateTrip_Controller
    {


        public User getUserByName(string username)
        {
            return UsersModel.Instance.getUserByName(username);
        }

        public bool isExistTrip()
        {
            return TripsModel.Instance.isExistTrip();
        }
        
        public void generateTrip(TripToAdd trip, List<User> choosenParticipants, List<Attraction> choosenAttractions)
        {
            TripsModel.Instance.generateTrip(trip, choosenParticipants, choosenAttractions);
        }
    }
}
