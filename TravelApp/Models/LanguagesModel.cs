﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<String> getLanguages()
        {
            List<String> languages = new List<String>();
            string command = "SELECT * FROM Language;";
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
            return languages;
        }
    }
}
