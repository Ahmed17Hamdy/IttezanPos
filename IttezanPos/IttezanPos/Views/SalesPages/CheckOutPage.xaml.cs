using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using IttezanPos.Services;
using IttezanPos.Views.SalesPages.SalesPopups;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
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
        private List<Product> saleproducts;
        private string text1;
        private string text2;
        private string text3;
        public CheckOutPage(List<Product> saleproducts, string text1, string text2,string text3)
        {
            InitializeComponent();
            this.saleproducts = saleproducts;
            totallbl.Text = text1;
            Disclbl.Text = text2;
            subtotallbl.Text = text3;
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
             //   ActiveIn.IsRunning = true;
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
                  //  listviewwww.ItemsSource = Clients;
                  //  ActiveIn.IsRunning = false;
                }
                else
                {
                 //   ActiveIn.IsRunning = false;
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
                 //   listviewwww.ItemsSource = Clients;
                    //  await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
              //  ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
            //    ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {

             //   ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }


        }

      
        private async void Customer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new ClientPopup());
        }
    }
}