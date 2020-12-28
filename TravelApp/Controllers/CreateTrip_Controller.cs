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
        
        public void generateTrip(TripToAdd trip, List<User> choosenParticipants, List<Attraction> choosenAttractions)
        {
            TripsModel.Instance.generateTrip(trip, choosenParticipants, choosenAttractions);
        }

        public void updateTrip(Trip inputedtrip, string tripName, int minAge, int maxAge, int maxParts, string startConverted, string endConverted, bool maleOnly, bool femaleOnly, List<User> partsToRemove, List<User> partsToAdd, List<Attraction> attractionsToRemove, List<Attraction> attractionsToAdd)
        {
            TripsModel.Instance.updateTrip(inputedtrip, tripName, minAge, maxAge, maxParts, startConverted, endConverted, maleOnly, femaleOnly, partsToRemove, partsToAdd, attractionsToRemove, attractionsToAdd);
        }
    }
}
