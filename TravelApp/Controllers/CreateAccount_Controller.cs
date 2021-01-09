using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TravelApp.Models;

namespace TravelApp
{
    class CreateAccount_Controller
    {
        public CreateAccount_Controller()
        {
        }
        public Tuple<bool, string> createAccount(string username, string phone, string email, string password, string passwordConfirm,
            string stringAge, bool male_box, bool female_box, List<String> friends, List<String> languages)
        {
            Tuple<bool, string> t = UsersModel.Instance.createAccount(username, phone, email, password, passwordConfirm,
                    stringAge, male_box, female_box, friends, languages);
            return t;
        }

        public Tuple<bool, List<string>> getLanguages()
        {
            return LanguagessModel.Instance.getLanguages();
        }

        public Tuple<bool, List<string>> getUsers()
        {
            return UsersModel.Instance.getUsers();
        }
    }
}
