using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace TravelApp.Models
{
    public sealed class LanguagessModel
    {
        private static readonly LanguagessModel instance = new LanguagessModel();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static LanguagessModel()
        {
        }

        private LanguagessModel()
        {
        }

        public static LanguagessModel Instance
        {
            get
            {
                return instance;
            }
        }

        public Tuple<bool, List<String>> getLanguages()
        {
            bool result = true;
            List<String> languages = new List<String>();
            string command = "SELECT * FROM Language;";
            lock (DbConnection.Locker)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string name = dr.GetString("name");
                        languages.Add(name);
                    }
                    dr.Close();
                }
                else
                {
                    result = false;
                }
            }
            return new Tuple <bool, List < String >>(result, languages);
        }
    }
}
