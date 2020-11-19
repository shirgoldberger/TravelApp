using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    class Language
    {
        string id;
        string name;

        public Language(string _id, string _name)
        {
            id = _id;
            name = _name;
        }
        public string Id
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
        }
        public Boolean Check_Status
        {
            get;
            set;
        }
    }
}
