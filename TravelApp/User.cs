using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
   public class User
    {
        string username;
        string password;
        string mail;
        bool is_male;
        int age;
        string phone;

        public User(string _username, string _password, string _mail, bool _is_male,
        int _age, string _phone)
        {
            username = _username;
            password = _password;
            mail = _mail;
            is_male = _is_male;
            age = _age;
            phone = _phone;
        }
        public User(string _username, string _password)
        {
            username = _username;
            password = _password;
        }
        public User(string _username)
        {
            username = _username;
        }

        public string Username
        {
            get
            {
                return username;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
        }
    }
}
