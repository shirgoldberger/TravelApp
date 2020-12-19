using K4os.Compression.LZ4.Internal;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelApp
{
    public class FindTripModel
    {
        private Object my_lock;

        public FindTripModel()
        {
            my_lock = new object();
        }
        public Trip createTrip(MySqlDataReader dr)
        {
            int id = int.Parse(dr.GetString("trip_code"));
            string name = dr.GetString("name");
            string admin = dr.GetString("admin");
            DateTime start_date = DateTime.Parse(dr.GetString("start_date"));
            DateTime end_date = DateTime.Parse(dr.GetString("end_date"));
            int min_age = int.Parse(dr.GetString("min_age"));
            int max_age = int.Parse(dr.GetString("max_age"));
            int max_participants = int.Parse(dr.GetString("max_participants"));
            bool male_only = dr.GetString("male_only")[0] == '1';
            bool female_only = dr.GetString("female_only")[0] == '1';

            Trip t = new Trip(id, name, admin, start_date, end_date, min_age,
                max_age, max_participants, male_only, female_only);
            return t;
        }

        public List<Trip> getTripsByCommand(string command)
        {
            List<Trip> trips = new List<Trip>();
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }  
            return trips;
        }

        public List<Trip> getAllTrip()
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM trip;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            return trips;
        }

        public List<Trip> getTripForUser(string username)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * from trip WHERE trip_code NOT IN" +
                "(SELECT t.trip_code FROM trip INNER JOIN member AS t WHERE t.username = '"
                + username + "');";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    Trip t = createTrip(dr);
                    trips.Add(t);
                }
                dr.Close();
            }
            foreach (Trip t in trips) {
                AddMemberAmount(t);
            }
            return trips;
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

        public Trip getTripById(string id)
        {
            Trip t = null;
            string command = "SELECT * FROM Trip WHERE trip_code = '" + id + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    t = createTrip(dr);
                }
                dr.Close();
            }
            return t;
        }

        public bool insertUserToTrip(string username, Trip trip)
        {
            string command = "insert into member values('"
                + trip.Id + "', '" + username + "');";
            if (DbConnection.ExecuteNonQuery(command))
            {
                trip.Member_Amount += 1;
                return true;
            }
            return false;
        }

        public List<City> getAllCities()
        {
            List<City> cities = new List<City>();
            string command = "SELECT * FROM City JOIN Country WHERE City.country=Country.name;";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string id = dr.GetString("city_id");
                    string name = dr.GetString("name");
                    string country = dr.GetString("country");
                    string continent = dr.GetString("continent");
                    City c = new City(id, name, country, continent);
                    cities.Add(c);
                }
                dr.Close();
            }
            return cities;

        }

        public List<string> getCountriesByBegin(string begin)
        {
            List<string> countries = new List<string>();
            string command = "SELECT name FROM country WHERE name LIKE '" + begin + "%';";
            lock (my_lock)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        countries.Add(dr.GetString("name"));
                    }
                    dr.Close();
                }
            }
            return countries;
        }

        public List<City> getCitiesByBegin(string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT * FROM city WHERE name LIKE '" + begin + "%';";
            lock (my_lock)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string id = dr.GetString("city_id");
                        string name = dr.GetString("name");
                        string country = dr.GetString("country");
                        City c = new City(id, name, country);
                        cities.Add(c);
                    }
                    dr.Close();
                }
            }
            return cities;
        }

        public List<City> getCitiesByCountry(string country, string begin)
        {
            List<City> cities = new List<City>();
            string command = "SELECT city_id, city.name, country, continent " +
                          "FROM city join country ON city.country = country.name " +
                          "WHERE city.name LIKE '" + begin + "%'";
            if (country != null)
            {
                country = "'" + country + "'";
                command += " AND country=" + country;
            }

            command += ";";
            lock (my_lock)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string city = dr.GetString("name");
                        string countryArg = dr.GetString("country");
                        string city_id = dr.GetString("city_id");
                        string continent = dr.GetString("continent");
                        City cityObj = new City(city_id, city, countryArg, continent);
                        cities.Add(cityObj);
                    }
                    dr.Close();
                }
            }
            return cities;
        }

        public List<Attraction> getAttractionsByCity(City city, string begin)
        {
            List<Attraction> attractions = new List<Attraction>();
            string command = "SELECT * " +
                          "FROM city join attraction ON city.city_id = attraction.city_id " +
                          "WHERE city.name LIKE '" + begin + "%'";
            if (city != null)
            {
                string c = "'" + city.Name + "'";
                command += " AND city.name=" + c;
            }

            command += ";";
            lock (my_lock)
            {
                MySqlDataReader dr = DbConnection.ExecuteQuery(command);
                if (dr != null)
                {
                    while (dr.Read())
                    {
                        string id = dr.GetString("attraction_code");
                        string name = dr.GetString("name");
                        string city_id = dr.GetString("city_id");
                        string type = dr.GetString("type");
                        Attraction a = new Attraction(id, name, city_id, type);
                        attractions.Add(a);
                    }
                    dr.Close();
                }
            }
            return attractions;
        }

        public void AddMemberAmount(Trip t)
        {
            string command = "SELECT count(username) FROM member WHERE trip_code = '"
                + t.Id + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string count = dr.GetString("count(username)");
                    t.Member_Amount = int.Parse(count);
                }
                dr.Close();
            }
        }

        public List<Attraction> GetAttractionsByCities(List<City> cities)
        {
            List<Attraction> attractions = new List<Attraction>();
            string allCities = "";
            int i = 0;
            for(i = 0; i < cities.Count(); i++)
            {
                allCities += ("'" + cities[i].Id + "'");
                if (i != cities.Count - 1)
                {
                    allCities += ",";
                }
            }
            string command = "SELECT * FROM Attraction WHERE city_id IN(" + allCities + ");";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string attraction_code = dr.GetString("attraction_code");
                    string name = dr.GetString("name");
                    string type = dr.GetString("type");
                    string city_id = dr.GetString("city_id");
                    Attraction a = new Attraction(attraction_code, name, city_id, type);
                    attractions.Add(a);
                }
                dr.Close();
            }
            return attractions;
        }

        public List<String> getFriendsForUser(string username)
        {
            List<String> friends = new List<string>();
            string command = "SELECT * FROM Friends WHERE username1='" + username
                + "' OR username2='" + username + "';";
            MySqlDataReader dr = DbConnection.ExecuteQuery(command);
            if (dr != null)
            {
                while (dr.Read())
                {
                    string name1 = dr.GetString("username1");
                    string name2 = dr.GetString("username2");
                    if (name1 != username)
                    {
                        friends.Add(name1);
                    } 
                    else if (name2 != username)
                    {
                        friends.Add(name2);
                    }
                }
                dr.Close();
            }
            return friends;
        }

        public List<Trip> findTripByDates(DateTime startDate, DateTime endDate)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT * FROM Trip WHERE start_date >= '" +
                startDate.ToString("yyyy-MM-dd") + "' AND end_date <= '" + endDate.ToString("yyyy-MM-dd") + "';";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByAttractions(List<Attraction> attractions)
        {
            List<Trip> trips = new List<Trip>();
            string allAttractions = "";
            int i;
            for (i = 0; i < attractions.Count; i++)
            {
                allAttractions += ("'" + attractions[i].Attraction_code + "'");
                if (i != attractions.Count - 1)
                {
                    allAttractions += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM trip_attractions" +
                " WHERE attraction_code IN(" + allAttractions + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + attractions.Count;
            string command = "SELECT * FROM Trip WHERE trip_code IN(" +
                innerTable + ");";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByLanguage(List<String> languages)
        {
            List<Trip> trips = new List<Trip>();
            string allLanguages = "";
            int i;
            for (i = 0; i < languages.Count; i++)
            {
                allLanguages += ("'" + languages[i] + "'");
                if (i != languages.Count - 1)
                {
                    allLanguages += ",";
                }
            }
            string createTable = "(select language.name, trip.trip_code" +
                " from user join user_languages join language join trip join member" +
                " where user.username = user_languages.username and" +
                " user.username = member.username and" +
                " trip.trip_code = member.trip_code and" +
                " language.name = user_languages.language_name) as Lan";
            string command = "select * from trip where trip_code in(select Lan.trip_code from "
                + createTable + " where name in (" + allLanguages + "));";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByMember(List<String> users)
        {
            List<Trip> trips = new List<Trip>();
            string allUsers = "";
            int i;
            for (i = 0; i < users.Count; i++)
            {
                allUsers += ("'" + users[i] + "'");
                if (i != users.Count - 1)
                {
                    allUsers += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM member" +
                " WHERE username IN(" + allUsers + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + users.Count;
            string command = "SELECT * FROM Trip WHERE trip_code IN(" +
                innerTable + ");";
            return getTripsByCommand(command);
        }

        public List<Trip> findTripByAge(int age)
        {
            string command = "SELECT * FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString() + ";";
            return getTripsByCommand(command);
        }

        public List<Trip> FindTrip(int age, List<string> members, List<string> languages, List<Attraction> attractions, List<City> cities, DateTime start, DateTime end, string op)
        {
            List<Trip> trips = new List<Trip>();
            string fullCommand = "";
            bool filterSelected = false;
            if (age != -1)
            {
                string ageCommand = "SELECT trip_code FROM Trip WHERE min_age <= " + age.ToString() + " AND max_age >= " + age.ToString();
                fullCommand = fullCommand + "trip_code IN(" + ageCommand + ")";
                filterSelected = true;
            }
            if (members.Count != 0)
            {
                string membersCommand = CreateMembersCommand(members);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + membersCommand + ")";
                filterSelected = true;
            }
            if (languages.Count != 0)
            {
                string languagesCommand = CreateLanguagesCommand(languages);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + languagesCommand + ")";
                filterSelected = true;
            }
            if (attractions.Count != 0)
            {
                string attractionsCommand = CreateAttractionsCommand(attractions);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + attractionsCommand + ")";
                filterSelected = true;
            }
            if (cities.Count != 0)
            {
                string citiesCommand = CreateCitiesCommand(cities);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + citiesCommand + ")";
                filterSelected = true;
            }
            if (checkDates(start, end))
            {
                string datesCommand = CreateDatesCommand(start, end);
                if (filterSelected)
                {
                    fullCommand += "\n" + op + "\n";
                }
                fullCommand = fullCommand + "trip_code IN(" + datesCommand + ")";
                filterSelected = true;
            }
            if (fullCommand == "") {
                return trips;
            }
            fullCommand += ";";
            fullCommand = "SELECT distinct * FROM trip WHERE\n" + fullCommand;
            return getTripsByCommand(fullCommand);
        }

        public string CreateMembersCommand(List<string> members)
        {
            string allUsers = "";
            int i;
            for (i = 0; i < members.Count; i++)
            {
                allUsers += ("'" + members[i] + "'");
                if (i != members.Count - 1)
                {
                    allUsers += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM member" +
                " WHERE username IN(" + allUsers + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + members.Count;
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        public string CreateLanguagesCommand(List<string> languages)
        {
            List<Trip> trips = new List<Trip>();
            string allLanguages = "";
            int i;
            for (i = 0; i < languages.Count; i++)
            {
                allLanguages += ("'" + languages[i] + "'");
                if (i != languages.Count - 1)
                {
                    allLanguages += ",";
                }
            }
            string createTable = "(select language.name, trip.trip_code" +
                " from user join user_languages join language join trip join member" +
                " where user.username = user_languages.username and" +
                " user.username = member.username and" +
                " trip.trip_code = member.trip_code and" +
                " language.name = user_languages.language_name) as Lan";
            string command = "select trip_code from trip where trip_code in(select Lan.trip_code from "
                + createTable + " where name in (" + allLanguages + "))";
            return command;
        }

        public string CreateAttractionsCommand(List<Attraction> attractions)
        {
            List<Trip> trips = new List<Trip>();
            string allAttractions = "";
            int i;
            for (i = 0; i < attractions.Count; i++)
            {
                allAttractions += ("'" + attractions[i].Attraction_code + "'");
                if (i != attractions.Count - 1)
                {
                    allAttractions += ",";
                }
            }
            string innerTable = "SELECT trip_code FROM trip_attractions" +
                " WHERE attraction_code IN(" + allAttractions + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + attractions.Count;
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        public string CreateCitiesCommand(List<City> cities)
        {
            List<Trip> trips = new List<Trip>();
            string allCities = "";
            int i;
            for (i = 0; i < cities.Count; i++)
            {
                allCities += ("'" + cities[i].Id + "'");
                if (i != cities.Count - 1)
                {
                    allCities += ",";
                }
            }
            string innerTable = "SELECT trip_attractions.trip_code FROM trip_attractions inner join attraction" +
                " WHERE attraction.city_id IN(" + allCities + ") " +
                "GROUP BY trip_code " +
                "HAVING COUNT(trip_code) =" + cities.Count;
            string command = "SELECT trip_code FROM Trip WHERE trip_code IN(" +
                innerTable + ")";
            return command;
        }

        public string CreateDatesCommand(DateTime start, DateTime end)
        {
            List<Trip> trips = new List<Trip>();
            string command = "SELECT trip_code FROM Trip WHERE start_date >= '" +
                start.ToString("yyyy-MM-dd") + "' AND end_date <= '" + end.ToString("yyyy-MM-dd") + "'";
            return command;
        }

        public bool checkDates(DateTime start, DateTime end)
        {
            if (start.ToString("yyyy-MM-dd") == "0001-01-01" &&
                end.ToString("yyyy-MM-dd") == "0001-01-01")
            {
                return false;
            }
            return true;
        }
    }
}


