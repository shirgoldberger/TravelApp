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
    public class AttractionProperties_Controller
    {
        public List<string> getTypes(string begin)
        {
            return AttractionsModel.Instance.getTypes(begin);
        }

        public List<City> getCitiesByContinent(string continent, string begin)
        {
            return LocationsModel.Instance.getCitiesByContinent(continent, begin);
        }

        public List<string> getContinents(string begin)
        {
            return LocationsModel.Instance.getContinents(begin);
        }


        public List<string> getCountries(string begin)
        {
            return LocationsModel.Instance.getCountries(begin);
        }

        public List<City> getCitiesByCountry(string country, string begin)
        {
            return LocationsModel.Instance.getCitiesByCountry(country, begin);
        }
    }
}
