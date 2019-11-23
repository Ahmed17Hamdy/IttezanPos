using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuppliersOpeningbalances : ContentPage
    {
        private ObservableCollection<Supplier> suppliers;

        public SuppliersOpeningbalances()
        {
            InitializeComponent();
        }

        public SuppliersOpeningbalances(ObservableCollection<Supplier> suppliers)
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Helpers.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
           : FlowDirection.LeftToRight;
            this.suppliers = suppliers;
            listviewwww.ItemsSource = suppliers;
        }
        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = Searchbar.Text;
            listviewwww.ItemsSource = suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = searchBar.Text;
            listviewwww.ItemsSource = suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
    }
}