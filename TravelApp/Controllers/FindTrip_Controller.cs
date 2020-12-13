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

        public List<Trip> filterTrips(int age, List<string> languages, List<Attraction> attractions,
            List<string> users, DateTime start, DateTime end)
        {
            List<Trip> trips = new List<Trip>();
            if (age != -1)
            {
                trips.AddRange(ftm.findTripByAge(age));
            }
            if (languages.Count() != 0)
            {
                trips.AddRange(ftm.findTripByLanguage(languages));
            }
            if (attractions.Count() != 0)
            {
                trips.AddRange(ftm.findTripByAttractions(attractions));
            }
            if (users.Count() != 0)
            {
                trips.AddRange(ftm.findTripByMember(users));

            }
            if (start != null && end != null)
            {
                trips.AddRange(ftm.findTripByDates(start, end));
            }
            return trips;
        }

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

        public void AddMemberAmount(Trip t)
        {
            ftm.AddMemberAmount(t);
        }

        public List<Attraction> GetAttractionsByCities(List<City> cities)
        {
            return ftm.GetAttractionsByCities(cities);
        }

        public List<String> getFriendsForUser(string username)
        {
            return ftm.getFriendsForUser(username);
        }
    }
}