using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;
using static IttezanPos.ViewModels.BoxViewModel;
using Settings = IttezanPos.Helpers.Settings;

namespace IttezanPos.Views.ReservoirPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservoirPage : ContentPage
    {
        List<CurrencyInfo> currencies = new List<CurrencyInfo>();
        private int type_Operation;
        private int disc_purchasing_suppliers;
        private int disc_expenses;
        private int add_sales_clients;

        public Box Box { get; private set; }

        public ReservoirPage()
        {
            InitializeComponent();
            _ = GetData();
            type_Operation = 1;
            disc_purchasing_suppliers = 0;
            disc_expenses=0;
            add_sales_clients = 0;
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
                    Box = (data.message.box);
                    Balancelbl.Text = Box.balance;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Box");
                        if (!info.Any())
                        {
                            db.CreateTable<Box>();
                        }
                        else
                        {
                            db.DropTable<Box>();
                            db.CreateTable<Box>();
                        }

                        db.Insert(Box);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Box");
                        if (!info.Any())
                        {
                            db.CreateTable<Box>();
                        }
                        else
                        {
                            db.DropTable<Box>();
                            db.CreateTable<Box>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                      //  db.InsertAll(Suppliers);
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
                        var info = db.GetTableInfo("Box");
                        if (!info.Any())
                            db.CreateTable<Box>();

                        Box = (db.Table<Box>().First());
                        Balancelbl.Text = Box.balance;
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Box");
                        if (!info.Any())
                            db.CreateTable<Box>();

                        Box = (db.Table<Box>().First());
                        Balancelbl.Text = Box.balance;
                        //   Suppliers = new ObservableCollection<Supplier>(db.Table<Supplier>().ToList());
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
        private  void Addrdbtn_Checked(object sender, EventArgs e)
        {
            Deductionbtn.IsVisible = false;
            Addingbtn.IsVisible = true;
            type_Operation = 1;
        }

        private void Deductrdbtn_Checked(object sender, EventArgs e)
        {
            Addingbtn.IsVisible = false;
            Deductionbtn.IsVisible = true;
            type_Operation = 0;
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
              if(e.NewTextValue!= "" )
            {
                ToWord toWord2 = new ToWord(Convert.ToDecimal(e.NewTextValue), new CurrencyInfo(Currencies.SaudiArabia));

                switch (Settings.LastUserGravity)
                {
                    case "Arabic":
                        NumbertoTextlbl.Text = toWord2.ConvertToArabic();
                        break;
                    case "English":
                        NumbertoTextlbl.Text = toWord2.ConvertToEnglish();
                        break;
                }
            }
            else
            {                
                    NumbertoTextlbl.Text = AppResources.ZeroText ;
            } 
        }

        private void Salescustswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                add_sales_clients = 1;
                Preferences.Set("add_sales_clients", 1);
            }
            else
            {
                add_sales_clients = 0;
                Preferences.Set("add_sales_clients", 0);
            }
        }

        private void Purchaseandsuppliersswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                disc_purchasing_suppliers = 1;
                Preferences.Set("disc_purchasing_suppliers", 1);
            }
            else
            {
                disc_purchasing_suppliers = 0;
                Preferences.Set("disc_purchasing_suppliers", 0);
            }
        }

        private void Expensesswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                disc_expenses = 1;
                Preferences.Set("disc_expenses", 1);
            }
            else
            {
                disc_expenses = 0;
                Preferences.Set("disc_expenses", 0);
            }
        }

        private async void Addingbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Box box = new Box
                    {
                        add_sales_clients = add_sales_clients,
                        disc_expenses = disc_expenses,
                        disc_purchasing_suppliers = disc_purchasing_suppliers,
                        note = NotesEntry.Text,
                        amount = AmountEntry.Text
                    };
                    var nsAPI = RestService.For<IBoxService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.Addinterproccess(box);
                        if (data.success == true)
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.AddedSucc, AppResources.Ok);
                            Balancelbl.Text = data.message.box.balance;
                        }
                    }
                    catch(Exception ex)
                    {
                        ActiveIn.IsVisible = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                    }
                }

            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }

        private async void Deductionbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Box box = new Box
                    {
                        add_sales_clients = add_sales_clients,
                        disc_expenses = disc_expenses,
                        disc_purchasing_suppliers = disc_purchasing_suppliers,
                        note = NotesEntry.Text,
                        amount = AmountEntry.Text
                    };
                    var nsAPI = RestService.For<IBoxService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.Addinterproccess(box);
                        if (data.success == true)
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.AddedSucc, AppResources.Ok);
                        }
                    }
                    catch
                    {
                        ActiveIn.IsVisible = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                    }
                }

            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }
    }
}