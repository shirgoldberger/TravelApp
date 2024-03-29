﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TravelApp.Controllers;
using TravelApp.Objects;

namespace TravelApp.Pages
{
    /// <summary>
    /// Interaction logic for AddNewAtt.xaml
    /// </summary>
    public partial class AddNewAttForTrip : Window
    {
        private List<Attraction> attractions;
        private List<Attraction> newAttractionsChoosed;
        private List<Attraction> choosed;
        private string type;
        private string city_code;
        private string attractionName;
        private AddNewAttForTrip_Controller controller;

        public List<Attraction> ReturenedAttractions { set; get; }


        public AddNewAttForTrip(List<Attraction> _choosed)
        {
            controller = new AddNewAttForTrip_Controller();
            choosed = _choosed;
            InitializeComponent();
            endLoadData();
            newAttractionsChoosed = new List<Attraction>();
            choosedAttractionsList.ItemsSource = null;
            attractionsList.ItemsSource = null;
            type = "";
            city_code = "";
            attractionName = "";
        }

        private void startLoadData()
        {
            progressBar.IsIndeterminate = true;
            progressBarText.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Visible;
        }

        private void endLoadData()
        {
            progressBar.IsIndeterminate = false;
            progressBarText.Visibility = Visibility.Hidden;
            progressBar.Visibility = Visibility.Hidden;
        }

        private void addClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = attractionsList.Items.IndexOf(item);
            newAttractionsChoosed.Add(attractions[itemIndex]);
            choosedAttractionsList.ItemsSource = null;
            choosedAttractionsList.ItemsSource = newAttractionsChoosed;
            bindAttractions();
        }
        

        private async Task<List<Attraction>> getAttractionsAsync(string cityId, string type, string name, List<Attraction> drop)
        {
            Tuple<bool, List<Attraction>> tuple = await Task.Run(() => controller.getAttractions(cityId, type, name, drop));
            if(!tuple.Item1)
            {
                Utils.Instance.errorAndExit("Error trying to access attractions records");
            }
            List<Attraction> list = tuple.Item2;
            return list;
        }

        private async void bindAttractions()
        {
            List<Attraction> toDrop = choosed.ToList();
            toDrop.AddRange(newAttractionsChoosed);
            startLoadData();
            attractionsList.ItemsSource = null;
            attractions = await getAttractionsAsync(city_code, type, attractionName, toDrop);
            attractionsList.ItemsSource = attractions;
            endLoadData();
        }

        private void filterClick(object sender, RoutedEventArgs e)
        {
            AttractionProperties ap = new AttractionProperties(false);
            ap.AttractionReturned = null;
            ap.TypeReturned = null;
            ap.City_code = null;
            ap.ShowDialog();
            if (ap.AttractionReturned != null)
            {
                attractionName = ap.AttractionReturned;
            }
            if (ap.TypeReturned != null)
            {
                type = ap.TypeReturned;
            }
            if (ap.City_code != null)
            {
                city_code = ap.City_code;
            }
            bindAttractions();
        }

        private void removeClick(object sender, RoutedEventArgs e)
        {
            var item = ((Button)sender).DataContext;
            int itemIndex = choosedAttractionsList.Items.IndexOf(item);
            newAttractionsChoosed.RemoveAt(itemIndex);
            choosedAttractionsList.ItemsSource = null;
            choosedAttractionsList.ItemsSource = newAttractionsChoosed;
            bindAttractions();
        }

        private void addChoosenClick(object sender, RoutedEventArgs e)
        {
            ReturenedAttractions = newAttractionsChoosed;
            GetWindow(this).Close();
        }
    }
}
