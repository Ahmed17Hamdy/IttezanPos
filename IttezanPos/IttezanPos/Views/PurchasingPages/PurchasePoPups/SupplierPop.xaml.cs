using IttezanPos.Models;
using IttezanPos.Services;
using IttezanPos.Views.SupplierPages;
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

namespace IttezanPos.Views.PurchasingPages.PurchasePoPups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierPop : ContentPage
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
        public SupplierPop()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            GetData();
            base.OnAppearing();
        }
        async Task GetData()
        {
            try
            {
                ActiveIn.IsRunning = true;
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
                    listviewwww.ItemsSource = Suppliers;
                    ActiveIn.IsRunning = false;
                }
                else
                {
                    ActiveIn.IsRunning = false;
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
                    listviewwww.ItemsSource = Suppliers;
                }
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }

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
            MessagingCenter.Send(new Supplier() { name = content.name }, "PopUpData");
            await Navigation.PopAsync();
        }

        private async void AddNewSupplier_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddingSupplierPage());
        }
    }
}