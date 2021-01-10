

namespace TravelApp
{
    
    public class User
    {
        private string username;
        private string password;
        private string mail;
        private char is_male;
        private string phone;
        private int age;


        public User(string _username, string _password, string _mail, char _is_male, int _age, string _phone)
        {
            username = _username;
            password = _password;
            mail = _mail;
            is_male = _is_male;
            phone = _phone;
            age = _age;
        }
        public string Username
        {
            get { return username; }
        }
        public string Password
        {
            get { return password; }
        }
        public string Mail
        {
            get { return mail; }
        }
        public char Is_male
        {
            get { return is_male; }
        }
        public int Age
        {
            get { return age; }
        }
        public string Phone
        {
            get { return phone; }
        }
    }
}
