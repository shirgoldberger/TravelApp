using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class City
    {
        private string id;
        private string name;
        private string country;
        private string continent;
        private string city_string; 

        public City(string _id, string _name, string _country, string _continent)
        {
            id = _id;
            name = _name;
            country = _country;
            continent = _continent;
            city_string = "name: " + name + ", country: " + country + ", continent: " + continent;
        }

        public string Id
        {
            get
            {
                return id;
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
        }
        public string Country
        {
            get
            {
                return country;
            }
        }
        public string Continent
        {
            get
            {
                return continent;
            }
        }
        public string City_String
        {
            get
            {
                return city_string;
            }
        }
    }
}