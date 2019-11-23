using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamEffects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IttezanPos.Views.SupplierPages;
using Plugin.Connectivity;
using Refit;
using IttezanPos.Services;
using IttezanPos.Models;
using System.Collections.ObjectModel;
using System.IO;
using SQLite;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSuppliersPage : ContentPage
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
        public MainSuppliersPage()
        {
            InitializeComponent();
            _ = GetData();
            Commandss();
        }
        async Task GetData()
        {
            try
            {
          //      ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();
                    Suppliers = new ObservableCollection<Supplier>(data.message.suppliers);
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                        {
                            db.CreateTable<Supplier>();
                        }
                        else
                        {
                            db.DropTable<Supplier>();
                            db.CreateTable<Supplier>();
                        }

                        db.InsertAll(Suppliers);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                        {
                            db.CreateTable<Supplier>();
                        }
                        else
                        {
                            db.DropTable<Supplier>();
                            db.CreateTable<Supplier>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                        db.InsertAll(Suppliers);
                    }
                  //  listviewwww.ItemsSource = Suppliers;
                //    ActiveIn.IsRunning = false;
                }
                else
                {
                   // ActiveIn.IsRunning = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                            db.CreateTable<Supplier>();

                        Suppliers = new ObservableCollection<Supplier>(db.Table<Supplier>().ToList());
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        Suppliers = new ObservableCollection<Supplier>(db.Table<Supplier>().ToList());
                    }
                //    listviewwww.ItemsSource = Suppliers;
                }
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            //    ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
             //   ActiveIn.IsRunning = false;
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
            await Navigation.PushAsync(new AddingSupplierPage());
        }
        private async void ClientsPageNav()
        {
            if (Suppliers != null)
            {
                await Navigation.PushAsync(new SuppliersPage(Suppliers));

            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new SuppliersPage(Suppliers));

            }
        }
        private async void OpeningbalancesPageNav()
        {
            if (Suppliers != null)
            {
                await Navigation.PushAsync(new SuppliersOpeningbalances(Suppliers));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new SuppliersOpeningbalances(Suppliers));
            }
        }
        private async void ClientRecievableNav()
        {
            if (Suppliers != null)
            {
                await Navigation.PushAsync(new SupplierRecivable(Suppliers));

            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new SupplierRecivable(Suppliers));

            }
        }
    }
}