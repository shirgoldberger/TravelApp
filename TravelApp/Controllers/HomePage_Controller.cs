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
        //Home_page_Model model;
        public HomePage_Controller()
        {
            //model = new Home_page_Model();
        }
        public Tuple<int, string> click_Login(string name, string password)
        {
            int i;
            string msg="";
            try
            {
                i = 0;
                string username = UsersModel.Instance.login(name, password);
            }
            catch (Exception error)
            {
                i = 1;
                msg = error.Message;
            }
            return Tuple.Create(i, msg);
        }
    }
}
