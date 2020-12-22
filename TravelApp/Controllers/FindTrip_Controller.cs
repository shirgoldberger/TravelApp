using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class FindTrip_Controller
    {
        private FindTripModel ftm;

        public FindTrip_Controller()
        {
            ftm = new FindTripModel();
        }

        public List<Trip> findTripByAge(int age)
        {
            return ftm.findTripByAge(age);
        }

        public List<Trip> FindTrip(int age, List<string> members, List<string> languages, List<Attraction> attractions, List<City> cities, DateTime start, DateTime end, string howToFilter)
        {
            string op;
            if (howToFilter == "all")
            {
                op = "AND";
            } else
            {
                op = "OR";
            }
            return ftm.FindTrip(age, members, languages, attractions, cities, start, end, op);
        }

        //public List<Trip> filterTrips(int age, List<string> languages, List<Attraction> attractions,
        //    List<string> users, DateTime start, DateTime end)
        //{
        //    List<Trip> trips = new List<Trip>();
        //    if (age != -1)
        //    {
        //        trips.AddRange(ftm.findTripByAge(age));
        //    }
        //    if (languages.Count() != 0)
        //    {
        //        trips.AddRange(ftm.findTripByLanguage(languages));
        //    }
        //    if (attractions.Count() != 0)
        //    {
        //        trips.AddRange(ftm.findTripByAttractions(attractions));
        //    }
        //    if (users.Count() != 0)
        //    {
        //        trips.AddRange(ftm.findTripByMember(users));

        //    }
        //    if (start != null && end != null)
        //    {
        //        trips.AddRange(ftm.findTripByDates(start, end));
        //    }
        //    return trips;
        //}

        public List<Trip> getAllTrip()
        {
            return ftm.getAllTrip();
        }

        public List<Trip> getTripForUser(string username)
        {
            return ftm.getTripForUser(username);
        }

        public List<String> getLanguages()
        {
            return ftm.getLanguages();
        }

        public Trip getTripById(string id)
        {
            return ftm.getTripById(id);
        }

        public bool insertUserToTrip(string username, Trip trip)
        {
            return ftm.insertUserToTrip(username, trip);
        }

        public List<City> getAllCities()
        {
            return ftm.getAllCities();
        }
        public List<string> getCountriesByBegin(string begin)
        {
            return ftm.getCountriesByBegin(begin);
        }

        public List<City> getCitiesByBegin(string begin)
        {
            return ftm.getCitiesByBegin(begin);
        }

        public List<Attraction> getAttractionsByCity(City city, string begin)
        {
            return ftm.getAttractionsByCity(city, begin);
        }

        public void AddMemberAmount(Trip t)
        {
            ftm.AddMemberAmount(t);
        }

        public List<String> getFriendsForUser(string username)
        {
            return ftm.getFriendsForUser(username);
        }

        public List<City> getCitiesByCountry(string country, string begin)
        {
            return ftm.getCitiesByCountry(country, begin);
        }
    }
}