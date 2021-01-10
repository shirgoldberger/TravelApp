

namespace TravelApp
{
    public class City
    {
        private string id;
        private string name;
        private string country;
        private string continent;
        private string city_string;
        private bool can_choose;

        public City(string _id, string _name, string _country, string _continent)
        {
            can_choose = true;
            id = _id;
            name = _name;
            country = _country;
            continent = _continent;
            city_string = "name: " + name + ", country: " + country;
        }

        public City(string _id, string _name, string _country)
        {
            id = _id;
            name = _name;
            country = _country;
        }

        public bool Can_Choose {
            get { return can_choose; }
            set { can_choose = value; }
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