using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class FindTrip_Controller
    {
        private FindTripModel ftm;

        public FindTrip_Controller()
        {
            ftm = new FindTripModel();
        }

        public List<Trip> findTripByDates(DateTime startDate, DateTime endDate)
        {
            return ftm.findTripByDates(startDate, endDate);
        }

        public List<Trip> findTripByAttractions(List<Attraction> attractions)
        {
            return ftm.findTripByAttractions(attractions);
        }

        public List<Trip> findTripByLanguage(List<String> languages)
        {
            return findTripByLanguage(languages);
        }

        public List<Trip> findTripByMember(List<String> users)
        {
            return ftm.findTripByMember(users);
        }

        public List<Trip> findTripByAge(int age)
        {
            return ftm.findTripByAge(age);
        }
    }
}
