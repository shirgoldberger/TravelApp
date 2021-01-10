using System;
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
