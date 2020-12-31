using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp
{
    public class FindTrip_Controller
    {
        public FindTrip_Controller()
        {
        }

        public Tuple<bool, List<Trip>> FindTrip(string username, int age, List<string> members, List<string> languages, List<Attraction> attractions, List<City> cities, DateTime start, DateTime end, string howToFilter)
        {
            string op;
            if (howToFilter == "all")
            {
                op = "AND";
            } else
            {
                op = "OR";
            }
            return TripsModel.Instance.FindTrip(username, age, members, languages, attractions, cities, start, end, op);
        }

        public Tuple<bool, List<Trip>> getTripForUser(string username)
        {
            Tuple<bool, List<Trip>> tuple = TripsModel.Instance.getTripsForUser(username, "NOT IN");
            if (!tuple.Item1)
            {
                return tuple;
            }
            List<Trip> trips = tuple.Item2;
            List<Trip> correctTrips = new List<Trip>();
            foreach (Trip t in trips)
            {
                if (t.Member_Amount < t.Max_Participants)
                {
                    correctTrips.Add(t);
                }
            }
            return new Tuple<bool, List<Trip>>(true, correctTrips);

        }

        public Tuple<bool, List<string>> getLanguages()
        {
            return LanguagessModel.Instance.getLanguages();
        }

        public bool insertUserToTrip(string username, Trip trip)
        {
            return TripsModel.Instance.insertUserToTrip(username, trip);
        }

        public Tuple<bool, List<string>> getCountriesByBegin(string begin)
        {
            return LocationsModel.Instance.getCountries(begin);
        }

        public Tuple<bool, List<City>> getCitiesByBegin(string begin)
        {
            return LocationsModel.Instance.getCitiesByBegin(begin);
        }

        public Tuple<bool, List<Attraction>> getAttractionsByCity(City city, string begin)
        {
            return AttractionsModel.Instance.getAttractionsByCity(city, begin);
        }

        public Tuple<bool, List<string>> getFriendsForUser(string username)
        {
            return UsersModel.Instance.getFriendsForUser(username);
        }

        public Tuple<bool, List<City>> getCitiesByCountry(string country, string begin)
        {
            return LocationsModel.Instance.getCitiesByCountry(country, begin);
        }
    }
}