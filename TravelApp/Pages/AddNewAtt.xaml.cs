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
    public partial class AddNewAtt : Window
    {
        UserPageModel model;
        List<City> cities;
        List<string> types;
        private string cityBegin;
        private string typeBegin;
        private string city_code;
        City choosen;
        public AddNewAtt(UserPageModel _model)
        {
            model = _model;
            InitializeComponent();
            cityBegin = "";
            typeBegin = "";
            city_code = "";
            bindCities();
            bindTypes();
        }

        private async Task<List<City>> getCitiesAsync(string begin)
        {
            List<City> list = await Task.Run(() => model.getCitiesByContinent(null, begin));
            return list;
        }

        private async void bindCities()
        {
            cities = await getCitiesAsync(cityBegin);
            cityBox.ItemsSource = cities;
        }

        private void bindTypes()
        {
            types = model.getTypes(typeBegin);
            typeBox.ItemsSource = types;
        }

        private void resetClick(object sender, RoutedEventArgs e)
        {
            attrationName.Text = "";
            continentName.Text = "";
            countryName.Text = "";
            cityBox.Text = "";
            cityBox.SelectedIndex = -1;
            typeBox.Text = "";
            typeBox.SelectedIndex = -1;
            choosen = null;
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            if(attrationName.Text == "" || attrationName.Text.Length > 300 || typeBox.SelectedIndex == -1 ||
               cityBox.SelectedIndex == -1 || choosen == null)
            {
                MessageBox.Show("There is problem with arguments for creating your attraction");
                return;
            }
            string name = choosen.Name;
            string type = types[typeBox.SelectedIndex];
            if(model.attractionAlreadyExist(name, city_code, type))
            {
                MessageBox.Show("This attraction is already exist");
                return;
            }
            bool result = model.addNewAttraction(name, city_code, type);
            string message = "Enter the attraction '" + name + "'";
            if(result)
            {
                message += " succeed";
            }
            else
            {
                message += " failed";
            }
            MessageBox.Show(message);
        }

        private void citySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedIndex = cityBox.SelectedIndex;
            if (selectedIndex != -1)
            {
                choosen = cities[selectedIndex];
                changeFieldsByCity(choosen);
            }            
        }

        private void cityTextChanged(object sender, EventArgs e)
        {
            cityBegin = cityBox.Text;
            bindCities();
        }

        private void typeTextChanged(object sender, EventArgs e)
        {
            typeBegin = typeBox.Text;
            bindTypes();
        }

        private void findByContinent(object sender, RoutedEventArgs e)
        {
            FindCityByContinent fbc = new FindCityByContinent(model);
            fbc.ShowDialog();
            choosen = fbc.ReturenedCity;
            changeFieldsByCity(choosen);
        }

        private void findByCountry(object sender, RoutedEventArgs e)
        {
            FindCityByCountry fbc = new FindCityByCountry(model);
            fbc.ShowDialog();
            choosen = fbc.ReturenedCity;
            changeFieldsByCity(choosen);
        }

        private void changeFieldsByCity(City _city)
        {
            continentName.Text = _city.Continent;
            countryName.Text = _city.Country;
            city_code = _city.Id;
        }
    }
}
