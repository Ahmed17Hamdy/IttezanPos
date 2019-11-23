using IttezanPos.Models;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamEffects;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainClientsPage : ContentPage
    {
        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }
        public MainClientsPage()
        {
            InitializeComponent();
            _ = GetData();
            Commandss();
          
        }
       
        async Task GetData()
        {
            try
            {
              //  ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();
                    Clients = new ObservableCollection<Client>(data.message.clients);
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                        {
                            db.CreateTable<Client>();
                        }
                        else
                        {
                            db.DropTable<Client>();
                            db.CreateTable<Client>();
                        }

                        db.InsertAll(Clients);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                        {
                            db.CreateTable<Client>();
                        }
                        else
                        {
                            db.DropTable<Client>();
                            db.CreateTable<Client>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                        db.InsertAll(Clients);
                    }
            //        listviewwww.ItemsSource = Clients;
             //       ActiveIn.IsRunning = false;
                }
                else
                {
                //    ActiveIn.IsRunning = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();

                        Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                    }
               //     listviewwww.ItemsSource = Clients;
                    //  await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
             //   ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
           //     ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {

            //    ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }


        }
        private void Commandss()
        {
            Commands.SetTap(AddClientGrid, new Command(() => {
                AddingClientPageNav();
            }));
            Commands.SetTap(ViewClientGrid, new Command(() => {
                ClientsPageNav();
            }));
            Commands.SetTap(CustomeropeningbalancesGrid, new Command(() => {
                OpeningbalancesPageNav();
            }));
            Commands.SetTap(ViewCustomerreceivablesGrid, new Command(() => {
                ClientRecievableNav();
            }));
        }
        private async void AddingClientPageNav()
        {
            await Navigation.PushAsync(new AddingClientPage());
        }
        private async void ClientsPageNav()
        {
            if (Clients != null)
            {
                await Navigation.PushAsync(new ClientsPage(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new ClientsPage(Clients));
            }
        }
        private async void OpeningbalancesPageNav()
        {
            if (Clients != null)
            {
                await Navigation.PushAsync(new OpeningbalancesPage(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new OpeningbalancesPage(Clients));
            }
        }
        private async void ClientRecievableNav()
        {
            if (Clients != null)
            {
                await Navigation.PushAsync(new ClientRecievable(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new ClientRecievable(Clients));
            }
           


        }


    }
}