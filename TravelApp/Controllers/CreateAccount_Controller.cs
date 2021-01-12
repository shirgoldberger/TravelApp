using System;
using System.Collections.Generic;
using TravelApp.Models;

namespace TravelApp
{
    class CreateAccount_Controller
    {
        public CreateAccount_Controller()
        {
        }
        public Tuple<bool,bool, string> createAccount(string username, string phone, string email, string password, string passwordConfirm,
            string stringAge, bool male_box, bool female_box, List<String> friends, List<String> languages)
        {
            return UsersModel.Instance.createAccount(username, phone, email, password, passwordConfirm,
                    stringAge, male_box, female_box, friends, languages);
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
