using Plugin.Permissions;
using System;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using System.Threading.Tasks;
using Plugin.Connectivity;
using IttezanPos.Models;
using Refit;
using IttezanPos.Services;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using IttezanPos.Helpers;
using System.Linq;
using Settings = IttezanPos.Helpers.Settings;
using SQLite;
using System.IO;
using IttezanPos.Resources;
using IttezanPos.Models.OfflineModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProductsPage : ContentPage
    {
        private List<int> client_ids = new List<int>();
        private List<Product> products = new List<Product>();
        public List<Product> Products
        {
            get { return products; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(products));
            }
        }

        public ObservableCollection<UpdateOfflineProduct> UpdatedClients { get; private set; }
        public ObservableCollection<DeleteOfflineProduct> DeletedClients { get; private set; }
        public ObservableCollection<OfflineProduct> AddedClients { get; private set; }

        public ViewProductsPage()
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
           : FlowDirection.LeftToRight;
            lisBydatetheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
           : FlowDirection.LeftToRight;
        }

        private void Scan_Tapped(object sender, EventArgs e)
        {
            Scanner();
        }
        async Task GetOffline()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("OfflineProduct");
            if (!info.Any())
                db.CreateTable<OfflineProduct>();

            AddedClients = new ObservableCollection<OfflineProduct>(db.Table<OfflineProduct>().ToList());
            if (AddedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();


                        var jsoncategoryArray = JsonConvert.SerializeObject(AddedClients);
                        var values = new Dictionary<string, string>
                        {
                            {"products",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-addproduct")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                            //             var json = JsonConvert.DeserializeObject<OfflineClientAdded>(serverResponse);

                            AddedClients.Clear();
                            db.DropTable<OfflineProduct>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        async Task GetOfflineUpdate()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("UpdateOfflineProduct");
            if (!info.Any())
                db.CreateTable<UpdateOfflineProduct>();

            UpdatedClients = new ObservableCollection<UpdateOfflineProduct>(db.Table<UpdateOfflineProduct>().ToList());

            if (UpdatedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();


                        var jsoncategoryArray = JsonConvert.SerializeObject(UpdatedClients);
                        var values = new Dictionary<string, string>
                        {
                            {"products",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-updateproduct")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                            var json = JsonConvert.DeserializeObject<RootObject>(serverResponse);

                            UpdatedClients.Clear();
                          //  Clients = new ObservableCollection<Product>(json.message.pr);
                            db.DropTable<UpdateOfflineProduct>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));
                        //    await Navigation.PopModalAsync();
                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        async Task GetOfflineDelete()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("DeleteOfflineProduct");
            if (!info.Any())
                db.CreateTable<DeleteOfflineProduct>();

            DeletedClients = new ObservableCollection<DeleteOfflineProduct>(db.Table<DeleteOfflineProduct>().ToList());
            if (DeletedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();
                        foreach (var item in DeletedClients)
                        {
                            client_ids.Add(item.product_id);
                        }

                        var jsoncategoryArray = JsonConvert.SerializeObject(client_ids);
                        var values = new Dictionary<string, string>
                        {
                            {"product_ids",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-delproduct")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                            // var json = JsonConvert.DeserializeObject<OfflineClientAdded>(serverResponse);

                            DeletedClients.Clear();
                            db.DropTable<DeleteOfflineProduct>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        public async void Scanner()
        {
            var ScannerPage = new ZXingScannerPage();
            _ = Navigation.PushAsync(ScannerPage);
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                  
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert(AppResources.Alert, AppResources.NeedCamera,AppResources.Ok);
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    status = results[Permission.Location];
                }
                if (status == PermissionStatus.Granted)
                {
                    ScannerPage.OnScanResult += (result) =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _ = Navigation.PopAsync();
                        });
                    };
                }
                else
                {
                    await DisplayAlert(AppResources.Alert, AppResources.PermissionsDenied, AppResources.Ok);
                    //On iOS you may want to send your user to the settings screen.
                    CrossPermissions.Current.OpenAppSettings();
                }
            }
            catch(Exception ex)
            {

                await DisplayAlert(AppResources.Alert, AppResources.PermissionsDenied, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();

            }


        }
        protected override void OnAppearing()
        {
            _ = GetOfflineUpdate();
            _ = GetOfflineDelete();
            _ = GetData();

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
                    var eachCategories = new ObservableCollection<Category>(data.message.categories);
                    foreach (var item in eachCategories)
                    {
                      //  Products = item.category.list_of_products;
                       Products.AddRange(item.category.list_of_products);                       
                    }
                    
                   
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                       
                            db.DeleteAll<Product>();
                            db.CreateTable<Product>();
                        foreach (var item in Products)
                        {
                        item.product_id = item.id;
                            db.InsertOrReplace(item);
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                       
                  
                    ProductsList.ItemsSource = products;

                    ActiveIn.IsRunning = false;
                }
                else
                {
                    ActiveIn.IsRunning = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        Products = (db.Table<Product>().ToList());
                        ProductsList.ItemsSource = Products;
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        Products =(db.Table<Product>().ToList());
                        ProductsList.ItemsSource = Products;
                    }
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

        private void ClassifyByDatebtn_Clicked(object sender, EventArgs e)
        {
            ProductsByDateList.ItemsSource = Products.OrderBy(b => b.created_at).ThenBy(b => b.expiration_date).ToList();
            ProductsList.IsVisible = false;
            ProductsByDateList.IsVisible = true;          
            AllListbtn.IsVisible = true;
            ClassifyByDatebtn.IsVisible = false;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
        ProductsList.ItemsSource= Products.Where(product => product.name.Contains(keyword.ToLower()));
            
        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
          ProductsList.ItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower()));
          
        }

        private async void ProductsList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var product = e.Item as Product;
            await Navigation.PushAsync(new AddingProductPage(product));
        }

        private void AllListbtn_Clicked(object sender, EventArgs e)
        {
            ProductsList.IsVisible = true;
            ProductsByDateList.IsVisible = false;
            AllListbtn.IsVisible = false;
            ClassifyByDatebtn.IsVisible = true;
        }

       
    }
}