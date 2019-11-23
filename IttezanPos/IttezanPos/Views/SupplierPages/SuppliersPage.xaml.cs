using IttezanPos.Models;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Settings = IttezanPos.Helpers.Settings;
using System.IO;
using SQLite;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuppliersPage : ContentPage
    {
        private ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();
        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(suppliers));
            }
        }
        public SuppliersPage()
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
        }

        public SuppliersPage(ObservableCollection<Supplier> suppliers)
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
            this.suppliers = suppliers;
            listviewwww.ItemsSource = suppliers;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = Suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = Suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        private async void Listviewwww_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Supplier;
            await Navigation.PushAsync(new AddingSupplierPage(content));
        }
    }
}