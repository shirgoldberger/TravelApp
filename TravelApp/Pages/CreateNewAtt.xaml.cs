﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TravelApp.Models;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class CreateNewAtt : Window
    {
        private UserPageModel model;
        private string city_code;
        public CreateNewAtt(UserPageModel _model)
        {
            model = _model;
            InitializeComponent();
            reset();
        }

        private void reset()
        {
            attrationName.Text = "";
            continentName.Text = "";
            countryName.Text = "";
            cityName.Text = "";
            typeName.Text = "";
            city_code = "";
        }

        private void createClick(object sender, RoutedEventArgs e)
        {
            if(attrationName.Text == "" || attrationName.Text.Length > 300 || city_code == "" ||
               typeName.Text == "")
            {
                MessageBox.Show("There is problem with arguments for creating your attraction");
                return;
            }
            string name = attrationName.Text;
            string type = typeName.Text;
            if(model.attractionAlreadyExist(name, city_code, type))
            {
                MessageBox.Show("This attraction is already exist");
                return;
            }
            bool result = model.addNewAttraction(name, city_code, type);
            string message = "Creating the attraction '" + name + "'";
            if(result)
            {
                message += " succeed";
                MessageBox.Show(message);
                GetWindow(this).Close();
            }
            else
            {
                message += " failed";
                MessageBox.Show(message);
            }  
        }


        private void setPropertiesClick(object sender, RoutedEventArgs e)
        {
            AttractionProperties ap = new AttractionProperties(model, true);
            ap.AttractionReturned = null;
            ap.ContinentReturned = null;
            ap.CountryReturned = null;
            ap.CityReturned = null;
            ap.TypeReturned = null;
            ap.City_code = null;
            ap.ShowDialog();
            if(ap.AttractionReturned != null)
            {
                attrationName.Text = ap.AttractionReturned;
            }
            if (ap.ContinentReturned != null)
            {
                continentName.Text = ap.ContinentReturned;
            }
            if (ap.CountryReturned != null)
            {
                countryName.Text = ap.CountryReturned;
            }
            if (ap.CityReturned != null)
            {
                cityName.Text = ap.CityReturned;
            }
            if (ap.TypeReturned != null)
            {
                typeName.Text = ap.TypeReturned;
            }
            if (ap.City_code != null)
            {
                city_code = ap.City_code;
            }
        }
    }
}