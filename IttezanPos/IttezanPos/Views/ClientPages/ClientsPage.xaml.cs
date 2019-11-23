
using IttezanPos.Models;
using IttezanPos.Services;
using Refit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using IttezanPos.Helpers;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;
using System;
using System.Linq;
using SQLite;
using System.IO;
using Settings = IttezanPos.Helpers.Settings;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientsPage : ContentPage
    {
        private ObservableCollection<Client> clients =new ObservableCollection<Client>();
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }
        public ClientsPage()
        {
            InitializeComponent();

        }

        public ClientsPage(ObservableCollection<Client> clients)
        {
            InitializeComponent();
            this.Clients = clients;
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
             : FlowDirection.LeftToRight;
            listviewwww.ItemsSource = clients;
        }

       
       

        private async void Listviewwww_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Client;
            await Navigation.PushAsync(new AddingClientPage(content));
        }
        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
    }
}