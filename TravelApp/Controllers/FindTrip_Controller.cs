using System;
using System.Collections.Generic;
using TravelApp.Models;

namespace TravelApp
{
    public class FindTrip_Controller
    {

        public Tuple<bool, List<Trip>> FindTrip(string username, int age, List<string> members, List<string> languages, List<Attraction> attractions, List<City> cities, DateTime start, DateTime end, string howToFilter, string is_male)
        {
            return TripsModel.Instance.FindTrip(username, age, members, languages, attractions, cities, start, end, howToFilter, is_male);
        }

        public Tuple<bool, List<Trip>> getTripForUser(string username, string is_male)
        {
            return TripsModel.Instance.getTripsForUser(username, "NOT IN", is_male);

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

        public Tuple<bool, string> isMale(string username)
        {
            return UsersModel.Instance.isMale(username);
        }
    }
}