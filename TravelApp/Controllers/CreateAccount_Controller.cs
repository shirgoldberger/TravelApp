using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TravelApp
{
    class CreateAccount_Controller
    {
        private CreateAccoutModel cam;

        public CreateAccount_Controller()
        {
            cam = new CreateAccoutModel();
        }
        public Tuple<bool, string> createAccount(string username, string phone, string email, string password, string passwordConfirm,
        string address, string stringAge, bool male_box, bool female_box, List<String> friends, List<String> languages)
        {
            if (username.Length == 0)
            {
                return new Tuple<bool, string>(false, "Please insert username");
            }
            if (languages.Count == 0)
            {
                return new Tuple<bool, string>(false, "Please choose languages");
            }
            if (phone.Length != 10)
            {
                return new Tuple<bool, string>(false, "Please insert correct phone number");
            }
            if (!male_box && !female_box)
            {
                return new Tuple<bool, string>(false, "Choose Gender");
            }
            if (email.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter an email");
            }
            if (!Regex.IsMatch(email, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            {
                return new Tuple<bool, string>(false, "Enter a valid email");
            }
            int age;
            try
            {
                age = int.Parse(stringAge);
            }
            catch
            {
                return new Tuple<bool, string>(false, "Enter valid age");
            }
            char male;
            if (female_box)
            {
                male = '0';
            }
            else
            {
                male = '1';
            }
            if (password.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter password");
            }
            if (passwordConfirm.Length == 0)
            {
                return new Tuple<bool, string>(false, "Enter Confirm password");
            }
            if (password != passwordConfirm)
            {
                return new Tuple<bool, string>(false, "Confirm password must be same as password");
            }
            else
            {
                Tuple<bool, string> t = cam.createAccount(username, phone,
                    email, password, address, age, male, friends, languages);
                return t;
            }
        }

        public List<string> getLanguages()
        {
            return cam.getLanguages();
        }

        public List<string> getUsers()
        {
            return cam.getUsers();
        }
    }
}
