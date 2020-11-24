using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class Language
    {
        string id;
        string name;

        public Language(string _id, string _name)
        {
            id = _id;
            name = _name;
        }
        public string Id
        {
            get { return id; }
        }
        public string Name
        {
            get { return name; }
        }
        public Boolean Check_Status
        {
            get;
            set;
        }

        public static List<Language> getLanguages()
        {
            List<Language> languages = new List<Language>();
            string command = "SELECT * FROM Language;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            while (dr.Read())
            {
                string name = dr.GetString("name");
                string id = dr.GetString("language_code");
                Language l = new Language(id, name);
                languages.Add(l);
            }
            dr.Close();
            return languages;
        }

    }
}
