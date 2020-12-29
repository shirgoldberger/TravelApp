using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp.Controllers
{
    class AddNewMembers_Controller
    {
        public Tuple<bool, List<User>> getRestFriends(List<User> drop, string username, int minAge, int maxAge, bool femaleOnly, bool maleOnly)
        {
            return UsersModel.Instance.getRestFriends(drop, username, minAge, maxAge, femaleOnly, maleOnly);
        }
    }
}
