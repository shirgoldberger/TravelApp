using System;
using System.Collections.Generic;

namespace TravelApp.Models
{
    public class AttractionProperties_Controller
    {
        public Tuple<bool, List<string>> getTypes(string begin)
        {
            return AttractionsModel.Instance.getTypes(begin);
        }

        public Tuple<bool, List<City>> getCitiesByContinent(string continent, string begin)
        {
            return LocationsModel.Instance.getCitiesByContinent(continent, begin);
        }

        public Tuple<bool, List<string>> getContinents(string begin)
        {
            return LocationsModel.Instance.getContinents(begin);
        }


        public Tuple<bool, List<string>> getCountries(string begin)
        {
            return LocationsModel.Instance.getCountries(begin);
        }

        public Tuple<bool, List<City>> getCitiesByCountry(string country, string begin)
        {
            return LocationsModel.Instance.getCitiesByCountry(country, begin);
        }
    }
}
