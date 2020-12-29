using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp
{
    public class UserPage_Controller
    {

        public Tuple<bool, List<string>> getRestUsers(string username, string beginning)
        {
            return UsersModel.Instance.getRestUsers(username, beginning);
        }

        public bool addNewFriend(string user1, string user2)
        {
            return UsersModel.Instance.addNewFriend(user1, user2);
        }

        public Tuple<bool, string> getCityCode(string country, string city)
        {
            return LocationsModel.Instance.getCityCode(country, city);
        }

        public bool addNewAttraction(string name, string city_code, string type)
        {
            return AttractionsModel.Instance.addNewAttraction(name, city_code, type);
        }

        public Tuple<bool, bool> attractionAlreadyExist(string name, string city_code, string type)
        {
            return AttractionsModel.Instance.attractionAlreadyExist(name, city_code, type);
        }

    }
}