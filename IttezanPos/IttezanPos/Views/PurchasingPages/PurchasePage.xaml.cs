using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using Plugin.Connectivity;
using Refit;
using IttezanPos.Models;
using System.Collections.ObjectModel;
using IttezanPos.Services;
using System.IO;
using SQLite;
using Rg.Plugins.Popup.Extensions;
using IttezanPos.Views.SalesPages.SalesPopups;
using IttezanPos.Resources;

namespace IttezanPos.Views.PurchasingPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchasePage : ContentPage
    {
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
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(categories));
            }
        }
        private List<Product> purchaseproducts = new List<Product>();
        public List<Product> PurchaseProducts
        {
            get { return purchaseproducts; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(purchaseproducts));
            }
        }
        private ObservableCollection<Product> purchasesro = new ObservableCollection<Product>();
        public ObservableCollection<Product> purchasero
        {
            get { return purchasesro; }
            set
            {
                purchasesro = value;
                OnPropertyChanged(nameof(purchasesro));
            }
        }
        public PurchasePage()
        {
            InitializeComponent();
        }
        private void Scan_Tapped(object sender, EventArgs e)
        {
            Scanner();
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
                        await DisplayAlert(AppResources.Alert, AppResources.NeedCamera, AppResources.Ok);
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
            catch (Exception ex)
            {

                await DisplayAlert(AppResources.Alert, AppResources.PermissionsDenied, AppResources.Ok);
                //On iOS you may want to send your user to the settings screen.
                CrossPermissions.Current.OpenAppSettings();

            }


        }
        private void Master_Tapped(object sender, EventArgs e)
        {
            var page = (App.Current.MainPage as NavigationPage).CurrentPage;
            (page as MasterDetailPage).IsPresented = true;
        }
        protected override void OnAppearing()
        {
            _ = GetData();
            base.OnAppearing();
        }
        async Task GetData()
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();
                    var eachCategories = new ObservableCollection<Category>(data.message.categories);
                    Categories = eachCategories;
                    foreach (var item in eachCategories)
                    {
                        foreach (var item2 in item.category.list_of_products)
                        {
                            item2.product_id = item2.id;
                        }
                        Products.AddRange(item.category.list_of_products);

                    }
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        if (!info.Any())
                        {
                            db.CreateTable<Product>();
                        }
                        else
                        {

                            db.CreateTable<Product>();
                        }
                        db.DeleteAll<Product>();
                        db.InsertAll(Products);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        db.DeleteAll<Product>();
                        db.CreateTable<Product>();
                        db.CreateTable<Category>();
                        foreach (var item in Products)
                        {
                            db.InsertOrReplace(item);
                        }

                        db.InsertAll(Categories);
                    }
                    ProductsList.FlowItemsSource = products;
                 
                    ActiveIn.IsVisible = false;
                }
                else
                {
                    ActiveIn.IsVisible = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        if (!info.Any())
                            db.CreateTable<Product>();
                        db.CreateTable<Category>();

                        Products = (db.Table<Product>().ToList());
                        Categories = new ObservableCollection<Category>(db.Table<Category>().ToList());
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        var info2 = db.GetTableInfo("Category");
                        Products = (db.Table<Product>().ToList());
                        Categories = new ObservableCollection<Category>(db.Table<Category>().ToList());

                    }
                    ProductsList.FlowItemsSource = Products;
                   
                }
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }
        private void ShowProductlistbtn_Clicked(object sender, EventArgs e)
        {
            if (ProductsList.IsVisible == true)
            {
                ProductsList.IsVisible = false;
            }
            else
            {
                ProductsList.IsVisible = true;
            }
           
        }
        private void ProductsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var saleproduct = ProductsList.FlowLastTappedItem as Product;
            saleproduct.total_price = saleproduct.purchase_price * saleproduct.quantity;
            totallbl.Text= (double.Parse(totallbl.Text) -
                    double.Parse(saleproduct.total_price.ToString())).ToString("0.00");
            saleproduct.quantity++;
            saleproduct.total_price = saleproduct.purchase_price * saleproduct.quantity;
            if (purchaseproducts.Count() == 0)
            {

                purchaseproducts.Add(saleproduct);

                cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                totallbl.Text= (double.Parse(totallbl.Text) +
                    double.Parse(saleproduct.total_price.ToString())).ToString("0.00");
            }
            else
            {
                var obj = PurchaseProducts.Find(x => x.id == saleproduct.id);
                if (obj != null)
                {

                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                    totallbl.Text = (double.Parse(totallbl.Text) +
                    double.Parse(saleproduct.total_price.ToString())).ToString("0.00");
                }
                else
                {
                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                    totallbl.Text = (double.Parse(totallbl.Text) +
                    double.Parse(saleproduct.total_price.ToString())).ToString("0.00");
                    PurchaseProducts.Add(saleproduct);
                }
            }
            purchasero = new ObservableCollection<Product>(PurchaseProducts);
            SalesList.ItemsSource = purchasero;
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (((sender as Entry).BindingContext is Product _selectedUser))
            {

                if (e.OldTextValue != null)
                {
                    if (e.OldTextValue == "")
                    {
                        cartnolbl.Text = int.Parse(cartnolbl.Text).ToString();
                       
                    }
                    else
                    {
                        cartnolbl.Text = (int.Parse(cartnolbl.Text) - int.Parse(e.OldTextValue)).ToString();
                        totallbl.Text = (double.Parse(totallbl.Text) -
                    double.Parse(_selectedUser.total_price.ToString())).ToString("0.00");
                    }


                    //   totallbl.Text = total.ToString();
                    if (e.NewTextValue != "")
                    {
                        if (e.NewTextValue != "1")
                        {

                            cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                            _selectedUser.quantity = int.Parse(e.NewTextValue);
                            _selectedUser.total_price = _selectedUser.purchase_price * _selectedUser.quantity;
                             totallbl.Text = (double.Parse(totallbl.Text) + _selectedUser.total_price).ToString("0.00");
                            
                            purchasero = new ObservableCollection<Product>(PurchaseProducts);
                            SalesList.ItemsSource = purchasero;
                        }

                        else
                        {
                            cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                            _selectedUser.quantity = int.Parse(e.NewTextValue);
                            _selectedUser.total_price = _selectedUser.purchase_price * _selectedUser.quantity;
                             totallbl.Text = (double.Parse(totallbl.Text) + _selectedUser.total_price).ToString("0.00");
                            purchasero = new ObservableCollection<Product>(PurchaseProducts);
                            SalesList.ItemsSource = purchasero;
                        }
                    }
                }
            }
        }

        private async void cost_Tapped(object sender, EventArgs e)
        {
            if (((sender as Label).BindingContext is Product _selectedro))
            {
                await Navigation.PushPopupAsync(new purchasePopUpPage(_selectedro));
                MessagingCenter.Subscribe<ValuePercent>(this, "PopUpData", (value) =>
                {
                    _selectedro.sale_price = value.Value;
                    _selectedro.purchase_price = value.Percentage;
                    _selectedro.total_price = _selectedro.purchase_price * _selectedro.quantity;
                    purchasero = new ObservableCollection<Product>(PurchaseProducts);
                    SalesList.ItemsSource = purchasero;
                });
              
            }
               
          
        }
        private void delete_Clicked(object sender, EventArgs e)
        {
            if (((sender as Button).BindingContext is Product _selectedro))
            {
                _selectedro.total_price = _selectedro.purchase_price * _selectedro.quantity;
                cartnolbl.Text = (int.Parse(cartnolbl.Text) - _selectedro.quantity).ToString();
                _selectedro.quantity = 0;
                totallbl.Text = (double.Parse(totallbl.Text) - _selectedro.total_price).ToString("0.00");               
                PurchaseProducts.Remove(_selectedro);
                purchasero.Remove(_selectedro);
                purchasero = new ObservableCollection<Product>(PurchaseProducts);
                SalesList.ItemsSource = purchasero;
            }

        }

        private async void Nextage_Tapped(object sender, EventArgs e)
        {
            if (PurchaseProducts.Count() == 0)
            {
                await DisplayAlert(AppResources.Alert, AppResources.SelectProduct, AppResources.Ok);

            }
            else
            {
                await Navigation.PushAsync(new PurchaseCheckout(PurchaseProducts, totallbl.Text));
            }

        }
    }
}