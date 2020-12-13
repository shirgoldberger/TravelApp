using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace TravelApp
{
    
    public class Attraction
    {
        string attraction_code;
        string name;
        string city_id;
        string type;
        private string attraction_string;
        private bool can_choose;


        public Attraction(string attraction_code,string name, string city_id, string type)
        {
            can_choose = true;
            this.attraction_code = attraction_code;
            this.name = name;
            this.city_id = city_id;
            this.type = type;
            this.attraction_string = "name: " + name + ", type: " + type;
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
        public string Attraction_String
        {
            get { return attraction_string; }
        }
        public bool Can_Choose
        {
            get { return can_choose; }
            set { can_choose = value; }
        }
    }
}
