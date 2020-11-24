using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class Attraction
    {
        string attraction_code;
        string name;
        string city_id;
        string type;

        public Attraction(string attraction_code,string name, string city_id, string type)
        {
            this.attraction_code = attraction_code;
            this.name = name;
            this.city_id = city_id;
            this.type = type;
        }
        public string Attraction_code
        {
            get { return attraction_code; }
        }
        public string Name
        {
            get { return name; }
        }
        public string City_id
        {
            get { return city_id; }
        }
        public string Type
        {
            get { return type; }
        }
    }
}
