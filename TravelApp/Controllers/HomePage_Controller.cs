using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelApp.Models;


namespace TravelApp
{
    class HomePage_Controller
    {
       
        public Tuple<int, string> click_Login(string name, string password)
        {

            return UsersModel.Instance.login(name, password);

        }
    }
}
