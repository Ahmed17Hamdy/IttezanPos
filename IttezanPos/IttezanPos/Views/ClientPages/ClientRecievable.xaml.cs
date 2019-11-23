﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientRecievable : ContentPage
    {
        private ObservableCollection<Client> clients;

        public ClientRecievable()
        {
            InitializeComponent();
        }

        public ClientRecievable(ObservableCollection<Client> clients)
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Helpers.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
            this.clients = clients;
            listviewwww.ItemsSource = clients;
        }
        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = Searchbar.Text;
            listviewwww.ItemsSource = clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = searchBar.Text;
            listviewwww.ItemsSource = clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
    }
}