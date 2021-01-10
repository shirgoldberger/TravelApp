using System;
using System.Collections.Generic;
using TravelApp.Models;

namespace TravelApp.Controllers
{
    class AddNewAttForTrip_Controller
    {
        public Tuple<bool, List<Attraction>> getAttractions(string cityId, string type, string name, List<Attraction> drop)
        {
            return AttractionsModel.Instance.getAttractions(cityId, type, name, drop);
        }
    }
}
