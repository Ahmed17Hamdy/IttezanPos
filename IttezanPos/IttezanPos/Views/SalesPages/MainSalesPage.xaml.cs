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
using IttezanPos.Models;
using Plugin.Connectivity;
using IttezanPos.Services;
using Refit;
using System.Collections.ObjectModel;
using DLToolkit.Forms.Controls;
using static IttezanPos.Services.OfflineDataBase;
using System.IO;
using SQLite;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSalesPage : ContentPage
    {

        string discountt;
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
        private List<Category> categories1 = new List<Category>();
        public List<Category> Categories1
        {
            get { return categories1; }
            set
            {
                categories1 = value;
                OnPropertyChanged(nameof(categories));
            }
        }
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
        private List<Product> saleproducts = new List<Product>();
        public List<Product> SaleProducts
        {
            get { return saleproducts; }
            set
            {
                products = value;
                OnPropertyChanged(nameof(saleproducts));
            }
        }
        private ObservableCollection<Product> salesro = new ObservableCollection<Product>();
        public ObservableCollection<Product> Salesro
        {
            get { return salesro; }
            set
            {
                salesro = value;
                OnPropertyChanged(nameof(categories));
            }
        }
        public MainSalesPage()
        {
            InitializeComponent();          
        }
        protected override bool OnBackButtonPressed()
        {
            CategoryList.Unfocus();
            return base.OnBackButtonPressed();
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
                    var eachCategories = new ObservableCollection<Category>(data.message.categories);
                    Categories = eachCategories;
                    foreach (var item in eachCategories)
                    {
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
                        db.DropTable<Product>();
                        db.InsertAll(Products);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        if (!info.Any())
                        {
                            db.CreateTable<Product>();
                            db.CreateTable<Category>();
                        }
                        else
                        {
                            db.DropTable<Product>();
                           db.DropTable<Category>();
                            db.CreateTable<Product>();
                            db.CreateTable<Category>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                      
                       db.InsertAll(Products);
                        db.InsertAll(Categories);
                    }
                    ProductsList.FlowItemsSource = products;
                    CategoryList.ItemsSource = Categories;
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
                        Categories1 = (db.Table<Category>().ToList());

                    }
                    ProductsList.FlowItemsSource = Products;
                    CategoryList.ItemsSource = Categories;
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
        private void searchbarvisible_Tapped(object sender, EventArgs e)
        {
            searchbar.IsVisible = true;
            searchstk.IsVisible = false;
            pickergrd.IsVisible = false;
        }

        private void searchbarinvisible_Tapped(object sender, EventArgs e)
        {
            searchbar.IsVisible = false;
            searchstk.IsVisible = true;
            pickergrd.IsVisible = true;
        }

       

        private  void ProductsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var saleproduct = ProductsList.FlowLastTappedItem as Product;
            saleproduct.quantity++;
            saleproduct.total_price = saleproduct.sale_price*saleproduct.quantity;
            if (SaleProducts.Count() == 0)
            {
               
                SaleProducts.Add(saleproduct);

              cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
               
                double subtotal = (double.Parse(subtotallbl.Text) + 
                    double.Parse(saleproduct.sale_price.ToString()));
               double total = subtotal -
                   double.Parse(Disclbl.Text);
                totallbl.Text = total.ToString("0.00");
                subtotallbl.Text = subtotal.ToString("0.00"); 
              
            }
            else
            {                
                 var obj = SaleProducts.Find(x => x.id == saleproduct.id);
                if (obj != null)
                {
                   
                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                    double subtotal = (double.Parse(subtotallbl.Text) +
                        double.Parse(saleproduct.sale_price.ToString()));
                    double total = subtotal -
                   double.Parse(Disclbl.Text);
                    totallbl.Text = total.ToString("0.00");
                    subtotallbl.Text = subtotal.ToString("0.00");
                }
                else
                {
                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                    double subtotal = (double.Parse(subtotallbl.Text) + 
                        double.Parse(saleproduct.sale_price.ToString()));
                    double total = subtotal -
                  double.Parse(Disclbl.Text);
                    totallbl.Text = total.ToString("0.00");
                    subtotallbl.Text = subtotal.ToString("0.00");
                    SaleProducts.Add(saleproduct);
                }
            }
            Salesro= new ObservableCollection<Product>(SaleProducts);
            SalesList.ItemsSource = Salesro;
        }

        //private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        //{
        //    if (((sender as Grid).BindingContext is Product _selectedUser))
        //    {

        //    }
        //}

        private void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryList.SelectedItem as Category;
            ProductsList.FlowItemsSource = Products.Where(product => product.catname.Contains(category.category.name)).ToList();
        }

     

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            ProductsList.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
            ProductsList.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();
        }

        private void Master_Tapped(object sender, EventArgs e)
        {
            (App.Current.MainPage as MasterDetailPage).IsPresented = true;
        }

        private async void BAck_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Cart_Tapped(object sender, EventArgs e)
        {
            Productstk.IsVisible = false;
            SalesList.IsVisible = true;
            backtosales.IsVisible = true;
        }

        private void Backtostk_Tapped(object sender, EventArgs e)
        {
            Productstk.IsVisible = true;
            SalesList.IsVisible = false;
            backtosales.IsVisible = false;
        }

     

        //private void discEntry_TextChanged(object sender, TextChangedEventArgs e)
        //{          
        //        if (((sender as Entry).BindingContext is Product _selectedUser))
        //        {
            
        //        if (e.NewTextValue != "")
        //                {
        //                subtotallbl.Text = (double.Parse(subtotallbl.Text) - _selectedUser.total_price).ToString();
        //                totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString();
        //                _selectedUser.discount = e.NewTextValue;
        //            double discPerc = double.Parse(e.NewTextValue) / 100;
        //            double disc = double.Parse(_selectedUser.total_price.ToString()) * discPerc;
        //                double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
        //                subtotallbl.Text = subtotal2.ToString("0.00");
        //                double usertotal = _selectedUser.total_price- disc;
        //                _selectedUser.total_price = usertotal;
        //            discountt= Disclbl.Text =(double.Parse(Disclbl.Text)+ disc).ToString("0.00");
        //                double total2 = subtotal2 -
        //                    double.Parse(Disclbl.Text);
        //                totallbl.Text = total2.ToString("0.00");
        //          //  SaleProducts.Insert(_selectedUser.id, obj);
                  
        //        }
        //         else
        //            {
        //                subtotallbl.Text = (double.Parse(subtotallbl.Text) - _selectedUser.total_price).ToString();
        //                totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString();
        //            Disclbl.Text = (double.Parse(Disclbl.Text) - double.Parse(discountt)).ToString() ;
        //            _selectedUser.discount = "0";
        //            double discPerc =0;
        //            double disc = double.Parse(_selectedUser.total_price.ToString()) * discPerc;
        //            double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
        //            subtotallbl.Text = subtotal2.ToString("0.00");
        //            double usertotal = _selectedUser.sale_price * _selectedUser.quantity;
        //            _selectedUser.total_price = usertotal;
        //            Disclbl.Text = (double.Parse(Disclbl.Text) + disc).ToString("0.00");
        //            double total2 = subtotal2 -
        //                double.Parse(Disclbl.Text);
        //            totallbl.Text = total2.ToString("0.00");

        //        }
               
        //    }
        //    Salesro = new ObservableCollection<Product>(SaleProducts);
        //    SalesList.ItemsSource = Salesro;

        //}

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
                            subtotallbl.Text = (double.Parse(subtotallbl.Text) - _selectedUser.total_price).ToString("0.00");
                            totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                        }


                        //   totallbl.Text = total.ToString();
                        if (e.NewTextValue != "")
                        {
                            if (e.NewTextValue != "1")
                            {

                                cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                                _selectedUser.quantity = int.Parse(e.NewTextValue);
                                _selectedUser.total_price = _selectedUser.sale_price * _selectedUser.quantity;


                                double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
                                double total2 = subtotal2 -
                                    double.Parse(Disclbl.Text);
                                totallbl.Text = total2.ToString("0.00");
                                subtotallbl.Text = subtotal2.ToString("0.00");
                                Salesro = new ObservableCollection<Product>(SaleProducts);
                                SalesList.ItemsSource = Salesro;
                            }

                            else
                            {
                                cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                                _selectedUser.quantity = int.Parse(e.NewTextValue);
                                _selectedUser.total_price = _selectedUser.sale_price * _selectedUser.quantity;
                                double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
                                double total2 = subtotal2 -
                                    double.Parse(Disclbl.Text);
                                totallbl.Text = total2.ToString("0.00");
                                subtotallbl.Text = subtotal2.ToString("0.00");
                                Salesro = new ObservableCollection<Product>(SaleProducts);
                                SalesList.ItemsSource = Salesro;
                            }
                        }
                    }
                }         
        }

        private async void Payment_Tapped(object sender, EventArgs e)
        {
            if (SaleProducts.Count() == 0)
            {
                await DisplayAlert(AppResources.Alert, AppResources.SelectProduct, AppResources.Ok);
            }
            else
            {
                await Navigation.PushAsync(new CheckOutPage(saleproducts, Disclbl.Text,totallbl.Text,subtotallbl.Text));
            }
        }
    }
}   