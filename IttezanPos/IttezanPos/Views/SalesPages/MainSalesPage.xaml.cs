using Plugin.Permissions.Abstractions;
using Plugin.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IttezanPos.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using IttezanPos.Models;
using Plugin.Connectivity;
using IttezanPos.Services;
using Refit;
using System.Collections.ObjectModel;
using System.IO;
using SQLite;

using Rg.Plugins.Popup.Extensions;
using IttezanPos.Views.SalesPages.SalesPopups;
using SQLiteNetExtensions.Extensions;
using Settings = IttezanPos.Helpers.Settings;
using System.Threading;
using System.Globalization;
using IttezanPos.Resources;
using IttezanPos.Views.InventoryPages;
using Plugin.Toast;
using System.Text.RegularExpressions;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSalesPage : ContentPage
    {

        double Alldisc;
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
        private List<Category2> categories1 = new List<Category2>();
        public List<Category2> Categories1
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
        private List<SaleProduct> saleproducts = new List<SaleProduct>();
        public List<SaleProduct> SaleProducts
        {
            get { return saleproducts; }
            set
            {
                saleproducts = value;
                OnPropertyChanged(nameof(saleproducts));
            }
        }
        private ObservableCollection<SaleProduct> salesro = new ObservableCollection<SaleProduct>();
        public ObservableCollection<SaleProduct> Salesro
        {
            get { return salesro; }
            set
            {
                salesro = value;
                OnPropertyChanged(nameof(salesro));
            }
        }
        private List<Client> clients = new List<Client>();
        public List<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }
        private ObservableCollection<Payment> payments = new ObservableCollection<Payment>();
        public ObservableCollection<Payment> Payments
        {
            get { return payments; }
            set
            {
                payments = value;
                OnPropertyChanged(nameof(payments));
            }
        }

        public List<HoldProduct> HoldList { get; private set; }

        double discount = 0;
        public MainSalesPage()
        {
            InitializeComponent();

            if (IttezanPos.Helpers.Settings.LastUserGravity == "Arabic")
            {
                ProductsList.IsVisible = false;
                ProductsListAr.IsVisible = true;
                CategoryListar.IsVisible = true;
             
                CategoryListen.IsVisible = false;
            }
            else
            {
                ProductsList.IsVisible = true;
                ProductsListAr.IsVisible = false;
                CategoryListar.IsVisible = false;
              
                CategoryListen.IsVisible = true;
            }
            FlowDirectionPage();
            MessagingCenter.Subscribe<SaleHold>(this, "SaleHold", (value) =>
            {
                if (SaleProducts.Count()!=0)
                {
                    foreach (var item in SaleProducts)
                    {
                        item.quantity = 0;
                        item.total_price = 0;
                        item.discount = 0;
                    }
                    SaleProducts.Clear();
                    Salesro.Clear();
                }
               
                totallbl.Text = AppResources.Zero;
                subtotallbl.Text = AppResources.Zero;
                Disclbl.Text = AppResources.Zero;
                cartnolbl.Text = AppResources.Zero;
                SaleProducts.AddRange(value.product.saleProducts);
                
                subtotallbl.Text = value.product.subtotal;
                totallbl.Text = value.product.total;
                Disclbl.Text = value.product.discount;
                cartnolbl.Text = value.product.number;
                Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                SalesList.ItemsSource = Salesro;
                SalesListen.ItemsSource = Salesro;
            });
           MessagingCenter.Subscribe<ValuePercent>(this, "PopUpData", (value) =>
            {
                if (value.Value != 0)
                {
                    Alldisc = value.alldisc;
                    Disclbl.Text = (double.Parse(Disclbl.Text) - value.alldisc).ToString("0.00");
                    Disclbl.Text = (double.Parse(Disclbl.Text) + value.Value).ToString("0.00");
                    Alldisc = value.Value;
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");

                }
                else
                {
                    Alldisc = value.alldisc;
                    Disclbl.Text = (double.Parse(Disclbl.Text) - value.alldisc).ToString("0.00");
                    Disclbl.Text = ((value.Percentage*double.Parse(subtotallbl.Text))+ double.Parse(Disclbl.Text)).ToString("0.00");
                    Alldisc = value.Percentage * double.Parse(subtotallbl.Text);
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                }

            });
            MessagingCenter.Subscribe<ValueQuantity>(this, "PopUpData1", (value) =>
            {
            checkquantity(value);
            });
            MessagingCenter.Subscribe<ValuePercentitem>(this, "PopUpDataitem", (value) =>
            {
                checkdiscount(value);
            });

        }

        private void checkdiscount(ValuePercentitem value)
        {
            if (value.Value!=0)
            {

                    if (value.product.sale_price > value.Value)
                    {
                    Disclbl.Text = (double.Parse(Disclbl.Text) - value.product.discount).ToString("0.00");
                    value.product.discount = 0;
                    discount = value.Value;
                    value.product.discount = value.product.discount + discount;
                    Disclbl.Text = (double.Parse(Disclbl.Text) + value.product.discount).ToString("0.00");
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                        value.product.total_price = value.product.sale_price * value.product.quantity;
                        foreach (var item in SaleProducts)
                        {
                            if (item.id == value.product.id)
                            {
                                item.discount = value.product.discount;
                                item.total_price = value.product.total_price- item.discount;
                            }
                        }
                        Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                        SalesList.ItemsSource =  Salesro;
                    SalesListen.ItemsSource = Salesro;

                    }
                    else
                    {
                        CrossToastPopUp.Current.ShowToastError(AppResources.Valuediscerror, Plugin.Toast.Abstractions.ToastLength.Long);
                        //Disclbl.Text = (double.Parse(value.Percentage) * double.Parse(subtotallbl.Text)).ToString();
                        //totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString();
                    }
                
            }
            else
            {
               // discount = value.Percentage* value.product.sale_price;
                if (value.product.sale_price > value.Percentage)
                {

                    Disclbl.Text = (double.Parse(Disclbl.Text) - value.product.discount).ToString("0.00");
                    value.product.discount = 0;
                    discount = value.Percentage * value.product.total_price;
                    value.product.discount = value.product.discount + discount;
                    Disclbl.Text = (double.Parse(Disclbl.Text) + value.product.discount).ToString("0.00");
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                    value.product.total_price = value.product.sale_price * value.product.quantity;
                    foreach (var item in SaleProducts)
                    {
                        if (item.id == value.product.id)
                        {
                            item.discount = value.product.discount;
                            item.total_price = value.product.total_price - item.discount;
                        }
                    }
                    Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                    SalesList.ItemsSource = Salesro;
                    SalesListen.ItemsSource = Salesro;

                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(AppResources.Valuediscerror, Plugin.Toast.Abstractions.ToastLength.Long);
                    //Disclbl.Text = (double.Parse(value.Percentage) * double.Parse(subtotallbl.Text)).ToString();
                    //totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString();
                }
            }
           
        }

        private void checkquantity(ValueQuantity value)
        {
            if (value.Quantity != 0)
            {

                if (Convert.ToDouble(value.product.stock) > value.Quantity)
                {
                    Disclbl.Text = (double.Parse(Disclbl.Text) - value.product.discount).ToString();
                    cartnolbl.Text = (double.Parse(cartnolbl.Text) - value.product.quantity).ToString();
                    subtotallbl.Text = (double.Parse(subtotallbl.Text) - value.product.total_price).ToString("0.00");
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                    value.product.quantity = value.Quantity;
                    cartnolbl.Text = (double.Parse(cartnolbl.Text) + value.product.quantity).ToString();

                    value.product.total_price = value.product.sale_price * value.product.quantity;

                    subtotallbl.Text = (double.Parse(subtotallbl.Text) + value.product.total_price).ToString("0.00");
                    totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00"); ;
                    value.product.discount = 0;
                    foreach (var item in SaleProducts.Where(w => w.id == value.product.id))
                    {                       
                            item.quantity = value.product.quantity;
                            item.total_price = value.product.total_price;
                        item.discount = value.product.discount;
                    }
                    Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                    SalesList.ItemsSource = Salesro;
                    SalesListen.ItemsSource = Salesro;


                }
                else
                {
                    CrossToastPopUp.Current.ShowToastError(AppResources.Stockerror, Plugin.Toast.Abstractions.ToastLength.Long);
                    //Disclbl.Text = (double.Parse(value.Percentage) * double.Parse(subtotallbl.Text)).ToString();
                    //totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString();
                }
            }     
            else
            {
                CrossToastPopUp.Current.ShowToastError(AppResources.EnterQuantity, Plugin.Toast.Abstractions.ToastLength.Long);
            } 
        }
        private void FlowDirectionPage()
        {


            FlowDirection = (Helpers.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
         : FlowDirection.LeftToRight;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().
         First(element => element.EnglishName.Contains(Helpers.Settings.LastUserGravity));
            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            GravityClass.Grav();
        }
        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Unsubscribe<SaleHold>(this, "SaleHold");
            MessagingCenter.Unsubscribe<ValuePercentitem>(this, "PopUpDataitem");
            MessagingCenter.Unsubscribe<ValueQuantity>(this, "PopUpData1");
            MessagingCenter.Unsubscribe<ValuePercent>(this, "PopUpData");
            if (SaleProducts.Count() != 0)
            {
                foreach (var item in SaleProducts)
                {
                    item.quantity = 0;
                    item.total_price = 0;
                    item.discount = 0;
                }
                SaleProducts.Clear();
                Salesro.Clear();
                totallbl.Text = AppResources.Zero;
                subtotallbl.Text = AppResources.Zero;
                Disclbl.Text = AppResources.Zero;
                cartnolbl.Text = AppResources.Zero;
                Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                SalesList.ItemsSource = Salesro;
                SalesListen.ItemsSource = Salesro;
            }
            CategoryListen.Unfocus();
            CategoryListar.Unfocus();
            return base.OnBackButtonPressed();
        }
        protected override void OnAppearing()
        {
            _ = GetData();
            base.OnAppearing();
        }
        async Task GetData()
        {
           
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    try
                    {
                        var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                        RootObject data = await nsAPI.GetSettings();
                        Categories = new ObservableCollection<Category>(data.message.categories);
                        Clients = data.message.clients;
                   
                        Payments = new ObservableCollection<Payment>(data.message.payments);
                        foreach (var item in Categories)
                        {
                     //       item.category2Id = item.category.id;
                            foreach (var item2 in item.category.list_of_products)
                            {
                                item2.product_id = item2.id;
                            }
                        if (item.category.list_of_products.Count!=0)
                        {
                            Products.AddRange(item.category.list_of_products);
                        }
                           
                       
                        Categories1.Add(item.category);
                        }
                    Category2 all = new Category2();
                    all.name = "الكل";
                    all.enname = "All";
                    all.id = 0;
                    all.list_of_products = Products;
                    Categories1.Insert(0,all);
                    Category allL = new Category();
                    allL.category = all;
                    allL.Id = 0;
                    allL.category2Id = all.id;
                    
                    Categories.Insert(0, allL);
                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Product");
                        var info2 = db.GetTableInfo("Category");
                    var info5 = db.GetTableInfo("Category2");
                    var info3 = db.GetTableInfo("Payment");
                    var info4 = db.GetTableInfo("Client");
                    if (!info.Any() && !info2.Any() && !info3.Any() && !info5.Any() && !info4.Any())
                        {
                            db.CreateTable<Product>();
                        db.CreateTable<Client>();
                        db.CreateTable<Payment>();
                        db.CreateTable<Category>();
                        db.CreateTable<Category2>();
                        db.InsertAll(Clients);
                        db.InsertAll(Products);
                        db.InsertAll(Payments);
                        db.InsertAll(Categories1);
                        db.InsertAllWithChildren(Categories);
                        }
                        else
                        {
                       
                        db.DropTable<Category>();
                        db.DropTable<Category2>();
                        db.DropTable<Product>();
                        db.DropTable<Client>();
                        db.DropTable<Payment>();
                        db.CreateTable<Product>();
                        db.CreateTable<Category2>();
                        db.InsertAll(Categories1);
                        db.CreateTable<Client>();
                        db.CreateTable<Payment>();
                        db.CreateTable<Category>();
                        db.InsertAll(Clients);
                        db.InsertAll(Products);
                        db.InsertAll(Payments);
                        db.InsertAllWithChildren(Categories);
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
                else
                {
                try
                {
                    ActiveIn.IsVisible = false;

                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                    var db = new SQLiteConnection(dbpath);
                    var info = db.GetTableInfo("Product");
                    if (!info.Any())
                        db.CreateTable<Product>();
                    db.CreateTable<Category>();
                    db.CreateTable<Category2>();
                    db.CreateTable<Payment>();

                    Products = (db.Table<Product>().ToList());
                    Categories = new ObservableCollection<Category>(db.GetAllWithChildren<Category>().ToList());
                    Payments = new ObservableCollection<Payment>(db.Table<Payment>().ToList());
                }
                    
                catch (Exception e)
                {

                }
                    
                }
            ActiveIn.IsVisible = false;
            ProductsList.FlowItemsSource = Products;
            ProductsListAr.FlowItemsSource = Products;
            CategoryListar.ItemsSource = Categories;
            CategoryListen.ItemsSource = Categories;
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
                   //         ProductsList.FlowItemsSource = Products.Where(product => product.barcode==result.Text).ToList();
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

        private   void ProductsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var saleproduct = ProductsList.FlowLastTappedItem as Product;
            SaleProduct sale = new SaleProduct
            {
                
                id = saleproduct.id,
                catname = saleproduct.catname,
                category_id = saleproduct.category_id,
                category2Id = saleproduct.category2Id,
                created_at = saleproduct.created_at,
                description = saleproduct.description,
                discount = saleproduct.discount,
                Enname = saleproduct.Enname,
                expiration_date = saleproduct.expiration_date,
                image = saleproduct.image,
                locale = saleproduct.locale,
                product_id = saleproduct.product_id,
                profit_percent = saleproduct.profit_percent,
                quantity = saleproduct.quantity,
                sale_price = saleproduct.sale_price,
                stock = saleproduct.stock,
                total_price = saleproduct.total_price,
                updated_at = saleproduct.updated_at,
                translations = saleproduct.translations,
                user = saleproduct.user,
                user_id = saleproduct.user_id,
                name = saleproduct.name,
            };
            if (sale.stock != 0)
            {
                sale.quantity++;
                sale.total_price = (sale.sale_price * sale.quantity);
                if (SaleProducts.Count() == 0)
                {

                    SaleProducts.Add(sale);

                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();

                    double subtotal = (double.Parse(subtotallbl.Text) +
                        double.Parse(sale.sale_price.ToString()));
                    Disclbl.Text = (double.Parse(Disclbl.Text) - sale.discount).ToString("0.00");
                    double total = subtotal -
                        double.Parse(Disclbl.Text);
                    totallbl.Text = total.ToString("0.00");
                    subtotallbl.Text = subtotal.ToString("0.00");

                }
                else
                {
                    var obj = SaleProducts.Find(x => x.id == sale.id);
                    if (obj != null)
                    {

                        cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                        double subtotal = (double.Parse(subtotallbl.Text) +
                            double.Parse(sale.sale_price.ToString()));
                        Disclbl.Text = (double.Parse(Disclbl.Text) - obj.discount).ToString("0.00");
                        double total = subtotal -
                       double.Parse(Disclbl.Text);
                        totallbl.Text = total.ToString("0.00");
                        subtotallbl.Text = subtotal.ToString("0.00");
                        sale.discount = 0;
                        foreach (var tom in SaleProducts.Where(w => w.id == obj.id))
                        {
                            tom.discount = sale.discount;
                            tom.quantity = tom.quantity + sale.quantity;
                            tom.total_price = tom.total_price + sale.total_price;
                        }

                    }
                    else
                    {
                        cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                        double subtotal = (double.Parse(subtotallbl.Text) +
                            double.Parse(sale.sale_price.ToString()));
                        Disclbl.Text = (double.Parse(Disclbl.Text) - sale.discount).ToString("0.00");
                        double total = subtotal -
                      double.Parse(Disclbl.Text);
                        totallbl.Text = total.ToString("0.00");
                        subtotallbl.Text = subtotal.ToString("0.00");
                        SaleProducts.Add(sale);

                    }
                }
                CrossToastPopUp.Current.ShowToastSuccess(sale.name + " " + AppResources.Added, Plugin.Toast.Abstractions.ToastLength.Short);

            }
            else
            {
                CrossToastPopUp.Current.ShowToastError(AppResources.Stockerror, Plugin.Toast.Abstractions.ToastLength.Long);
            }
            Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
            SalesList.ItemsSource = Salesro;
            SalesListen.ItemsSource = Salesro;
        }
        private void ProductsListAr_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
           
            var saleproduct = ProductsListAr.FlowLastTappedItem as Product;
            SaleProduct sale = new SaleProduct
            {
                id = saleproduct.id,
                catname = saleproduct.catname,
                category_id = saleproduct.category_id,
                category2Id = saleproduct.category2Id,
                created_at = saleproduct.created_at,
                description = saleproduct.description,
                discount = saleproduct.discount,
                Enname = saleproduct.Enname,
                expiration_date = saleproduct.expiration_date,
                image = saleproduct.image,
                locale = saleproduct.locale,
                product_id = saleproduct.product_id,
                profit_percent = saleproduct.profit_percent,
                quantity = saleproduct.quantity,
                sale_price = saleproduct.sale_price,
                stock = saleproduct.stock,
                total_price = saleproduct.total_price,
                updated_at = saleproduct.updated_at,
                translations = saleproduct.translations,
                user = saleproduct.user,
                user_id = saleproduct.user_id,
                name = saleproduct.name,
            };
            if (sale.stock != 0)
            {
                sale.quantity++;
                sale.total_price = (sale.sale_price * sale.quantity);
                if (SaleProducts.Count() == 0)
                {
                    
                    SaleProducts.Add(sale);

                    cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();

                    double subtotal = (double.Parse(subtotallbl.Text) +
                        double.Parse(sale.sale_price.ToString()));
                    Disclbl.Text = (double.Parse(Disclbl.Text) - sale.discount).ToString("0.00");
                    double total = subtotal -
                        double.Parse(Disclbl.Text);
                    totallbl.Text = total.ToString("0.00");
                    subtotallbl.Text = subtotal.ToString("0.00");

                }
                else
                {
                    var obj = SaleProducts.Find(x => x.id == sale.id);
                    if (obj != null)
                    {

                        cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                        double subtotal = (double.Parse(subtotallbl.Text) +
                            double.Parse(sale.sale_price.ToString()));
                        Disclbl.Text = (double.Parse(Disclbl.Text) - obj.discount).ToString("0.00");
                        double total = subtotal -
                       double.Parse(Disclbl.Text);
                        totallbl.Text = total.ToString("0.00");
                        subtotallbl.Text = subtotal.ToString("0.00");
                        sale.discount = 0;
                        foreach (var tom in SaleProducts.Where(w => w.id == obj.id))
                        {
                            tom.discount = sale.discount;
                            tom.quantity = tom.quantity+ sale.quantity;
                            tom.total_price = tom.total_price+ sale.total_price;
                        }

                    }
                    else
                    {
                        cartnolbl.Text = (int.Parse(cartnolbl.Text) + 1).ToString();
                        double subtotal = (double.Parse(subtotallbl.Text) +
                            double.Parse(sale.sale_price.ToString()));
                        Disclbl.Text = (double.Parse(Disclbl.Text) - sale.discount).ToString("0.00");
                        double total = subtotal -
                      double.Parse(Disclbl.Text);
                        totallbl.Text = total.ToString("0.00");
                        subtotallbl.Text = subtotal.ToString("0.00");
                        SaleProducts.Add(sale);

                    }
                }
                CrossToastPopUp.Current.ShowToastSuccess(sale.name + " " + AppResources.Added, Plugin.Toast.Abstractions.ToastLength.Short);

            }
            else
            {
                CrossToastPopUp.Current.ShowToastError(AppResources.Stockerror, Plugin.Toast.Abstractions.ToastLength.Long);
            }
            Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
            SalesList.ItemsSource  = Salesro;
            SalesListen.ItemsSource = Salesro;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (((sender as Grid).BindingContext is Product _selectedprp))
            {
                await Navigation.PushAsync(new AddingProductPage(_selectedprp));
            }
        }

        private void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            var category = CategoryListar.SelectedItem as Category;
            if (category.category.name == "الكل")
            {
                ProductsList.FlowItemsSource = Products;
                ProductsListAr.FlowItemsSource = Products;

            }
            else
            {
                ProductsList.FlowItemsSource = Products.Where(product => product.category_id==category.category.id).ToList();
                ProductsListAr.FlowItemsSource = Products.Where(product => product.category_id == category.category.id).ToList();

            }
        }
        private void CategoryListen_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryListen.SelectedItem as Category;

            if (category.category.name == "الكل")
            {
                ProductsList.FlowItemsSource = Products;
                ProductsListAr.FlowItemsSource = Products;
            }
            else
            {
                ProductsList.FlowItemsSource = Products.Where(product => product.category_id == category.category.id).ToList();
                ProductsListAr.FlowItemsSource = Products.Where(product => product.category_id == category.category.id).ToList();

            }
        }



        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            if(Regex.IsMatch(keyword, "^[a-zA-Z0-9]*$"))
            {
                ProductsList.FlowItemsSource = Products.Where(product => product.Enname.Contains(keyword.ToLower())).ToList();
                ProductsListAr.FlowItemsSource = Products.Where(product => product.Enname.Contains(keyword.ToLower())).ToList();

            }
            else
            {
                ProductsListAr.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();
                ProductsList.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();

            }
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
           
            if (Regex.IsMatch(keyword, "^[a-zA-Z0-9]*$"))
            {
                ProductsList.FlowItemsSource = Products.Where(product => product.Enname.Contains(keyword.ToLower())).ToList();
                ProductsListAr.FlowItemsSource = Products.Where(product => product.Enname.Contains(keyword.ToLower())).ToList();

            }
            else
            {
                ProductsListAr.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();
                ProductsList.FlowItemsSource = Products.Where(product => product.name.Contains(keyword.ToLower())).ToList();

            }
        }

        private void Master_Tapped(object sender, EventArgs e)
        {
           
            var page = (App.Current.MainPage as NavigationPage).CurrentPage;
            (page as MasterDetailPage).IsPresented = true;
        }

        private async void BAck_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private void Cart_Tapped(object sender, EventArgs e)
        {
            if (Settings.LastUserGravity == "Arabic")
            {
                SalesList.IsVisible = true;
                listheaderlistv.FlowDirection = FlowDirection.RightToLeft;
                SalesListen.IsVisible = false;
                Productstk.IsVisible = false;
                backtosales.IsVisible = true;
            }
            else
            {
                listheaderlistv.FlowDirection = FlowDirection.LeftToRight;

                SalesList.IsVisible = false;
                SalesListen.IsVisible = true;
                Productstk.IsVisible = false;
                backtosales.IsVisible = true;
            }
        }

        private void Backtostk_Tapped(object sender, EventArgs e)
        {
            if (Settings.LastUserGravity == "Arabic")
            {
                Productstk.IsVisible = true;
                SalesList.IsVisible = false;
                SalesListen.IsVisible = false;
                backtosales.IsVisible = false;
            }
            else
            {
                Productstk.IsVisible = true;
                SalesList.IsVisible = false;
                SalesListen.IsVisible = false;
                backtosales.IsVisible = false;
            }
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

         
                //if (((sender as Entry).BindingContext is Product _selectedUser))
                //{

                //    if (e.OldTextValue != null)
                //    {
                //        if (e.OldTextValue == "")
                //        {
                //            cartnolbl.Text = int.Parse(cartnolbl.Text).ToString();

                //        }
                //        else
                //        {
                //            cartnolbl.Text = (int.Parse(cartnolbl.Text) - int.Parse(e.OldTextValue)).ToString();
                //            subtotallbl.Text = (double.Parse(subtotallbl.Text) - _selectedUser.total_price).ToString("0.00");
                //            totallbl.Text = (double.Parse(subtotallbl.Text) - double.Parse(Disclbl.Text)).ToString("0.00");
                //        }


                //        //   totallbl.Text = total.ToString();
                //        if (e.NewTextValue != "")
                //        {
                //        if (_selectedUser.stock > int.Parse(e.NewTextValue))
                //        {
                //            if (e.NewTextValue != "1")
                //            {

                //                cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                //                _selectedUser.quantity = int.Parse(e.NewTextValue);
                //                _selectedUser.total_price = _selectedUser.sale_price * _selectedUser.quantity;


                //                double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
                //                double total2 = subtotal2 -
                //                    double.Parse(Disclbl.Text);
                //                totallbl.Text = total2.ToString("0.00");
                //                subtotallbl.Text = subtotal2.ToString("0.00");
                //                Salesro = new ObservableCollection<Product>(SaleProducts);
                //                SalesList.ItemsSource = Salesro;
                //            }

                //            else
                //            {
                //                cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                //                _selectedUser.quantity = int.Parse(e.NewTextValue);
                //                _selectedUser.total_price = _selectedUser.sale_price * _selectedUser.quantity;
                //                double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
                //                double total2 = subtotal2 -
                //                    double.Parse(Disclbl.Text);
                //                totallbl.Text = total2.ToString("0.00");
                //                subtotallbl.Text = subtotal2.ToString("0.00");
                //                Salesro = new ObservableCollection<Product>(SaleProducts);
                //                SalesList.ItemsSource = Salesro;
                //            }
                //        }
                //        else
                //        {
                //           //  e.NewTextValue == e.OldTextValue;
                           
                //            _selectedUser.quantity = int.Parse(e.OldTextValue);
                //            cartnolbl.Text = (int.Parse(cartnolbl.Text) + _selectedUser.quantity).ToString();
                //            _selectedUser.total_price = _selectedUser.sale_price * _selectedUser.quantity;
                //            double subtotal2 = _selectedUser.total_price + double.Parse(subtotallbl.Text);
                //            double total2 = subtotal2 -
                //                double.Parse(Disclbl.Text);
                //            totallbl.Text = total2.ToString("0.00");
                //            subtotallbl.Text = subtotal2.ToString("0.00");
                //            var newt  = e.OldTextValue;
                //        //    e.NewTextValue == newt;
                //              Salesro = new ObservableCollection<Product>(SaleProducts);
                //            SalesList.ItemsSource = Salesro;
                //            CrossToastPopUp.Current.ShowToastError(AppResources.Stockerror, Plugin.Toast.Abstractions.ToastLength.Long);
                //        }
                //    }
                //    }
                //}         
        }

        private async void Payment_Tapped(object sender, EventArgs e)
        {
            if (SaleProducts.Count() == 0)
            {
                CrossToastPopUp.Current.ShowToastWarning(AppResources.SelectProduct, Plugin.Toast.Abstractions.ToastLength.Long);

            }
            else
            {
                await Navigation.PushAsync(new CheckOutPage(saleproducts, Disclbl.Text,totallbl.Text,subtotallbl.Text,Payments,Products));
            }
        }

        private async void Discard_Tapped(object sender, EventArgs e)
        {
            if (SaleProducts.Count() == 0)
            {
                CrossToastPopUp.Current.ShowToastWarning(AppResources.SelectProduct, Plugin.Toast.Abstractions.ToastLength.Long);

            }
            else
            {
                var action = await DisplayAlert(AppResources.Alert, AppResources.SureTodiscard, AppResources.Yes, AppResources.No);
                if (action)
                {
                    foreach (var item in SaleProducts)
                    {
                        item.quantity = 0;
                        item.total_price = 0;
                        item.discount = 0;
                    }
                    SaleProducts.Clear();
                    Salesro.Clear();
                    totallbl.Text = AppResources.Zero;
                    subtotallbl.Text = AppResources.Zero;
                    Disclbl.Text = AppResources.Zero;
                    cartnolbl.Text = AppResources.Zero;
                    Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                    SalesList.ItemsSource = Salesro;
                    SalesListen.ItemsSource = Salesro;
                }
            }
        }

        private void delete_Clicked(object sender, EventArgs e)
        {
            if (((sender as Button).BindingContext is Product _selectedro))
            {
                var obj = SaleProducts.Find(x => x.id == _selectedro.id);
               
                cartnolbl.Text = (int.Parse(cartnolbl.Text) - _selectedro.quantity).ToString();
                subtotallbl.Text = (double.Parse(subtotallbl.Text) - _selectedro.total_price).ToString("0.00");
                Disclbl.Text = (double.Parse(Disclbl.Text)- _selectedro.discount).ToString("0.00");
                totallbl.Text = (double.Parse(subtotallbl.Text) - (double.Parse(Disclbl.Text))).ToString("0.00");
                _selectedro.quantity = 0;
                _selectedro.total_price = 0;
                _selectedro.discount = 0;
                saleproducts.Remove(obj);
                Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                SalesList.ItemsSource =  Salesro;
                SalesListen.ItemsSource = Salesro;
            }

        }

        private async void Discount_Tapped(object sender, EventArgs e)
        {
            if (SaleProducts.Count() == 0)
            {
                CrossToastPopUp.Current.ShowToastWarning(AppResources.SelectProduct, Plugin.Toast.Abstractions.ToastLength.Long);

            }
            else
            {
                await Navigation.PushPopupAsync(new CalculatorPage(Alldisc));
            }

        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            if (((sender as Frame).BindingContext is Product saleproduct))
            {
                SaleProduct sale = new SaleProduct
                {
                    id = saleproduct.id,
                    catname = saleproduct.catname,
                    category_id = saleproduct.category_id,
                    category2Id = saleproduct.category2Id,
                    created_at = saleproduct.created_at,
                    description = saleproduct.description,
                    discount = saleproduct.discount,
                    Enname = saleproduct.Enname,
                    expiration_date = saleproduct.expiration_date,
                    image = saleproduct.image,
                    locale = saleproduct.locale,
                    product_id = saleproduct.product_id,
                    profit_percent = saleproduct.profit_percent,
                    quantity = saleproduct.quantity,
                    sale_price = saleproduct.sale_price,
                    stock = saleproduct.stock,
                    total_price = saleproduct.total_price,
                    updated_at = saleproduct.updated_at,
                    translations = saleproduct.translations,
                    user = saleproduct.user,
                    user_id = saleproduct.user_id,
                    name = saleproduct.name,
                };
                await Navigation.PushPopupAsync(new Quantitypop(sale));
            }
        }

        private async void discountitem_Tapped(object sender, EventArgs e)
        {
            if (((sender as Frame).BindingContext is Product saleproduct))
            {
                SaleProduct sale = new SaleProduct
                {
                    id = saleproduct.id,
                    
                    catname = saleproduct.catname,
                    category_id = saleproduct.category_id,
                    category2Id = saleproduct.category2Id,
                    created_at = saleproduct.created_at,
                    description = saleproduct.description,
                    discount = saleproduct.discount,
                    Enname = saleproduct.Enname,
                    expiration_date = saleproduct.expiration_date,
                    image = saleproduct.image,
                    locale = saleproduct.locale,
                    product_id = saleproduct.product_id,
                    profit_percent = saleproduct.profit_percent,
                    quantity = saleproduct.quantity,
                    sale_price = saleproduct.sale_price,
                    stock = saleproduct.stock,
                    total_price = saleproduct.total_price,
                    updated_at = saleproduct.updated_at,
                    translations = saleproduct.translations,
                    user = saleproduct.user,
                    user_id = saleproduct.user_id,
                    name = saleproduct.name,
                };
                await Navigation.PushPopupAsync(new CalculatorPage(sale));
            }
        }

        private async void Extra_Tapped(object sender, EventArgs e)
        {
            await  Navigation.PushPopupAsync(new ExtraPopupPage());
        }

        private  void Hold_Tapped(object sender, EventArgs e)
        {
            if (SaleProducts.Count() == 0)
            {
                CrossToastPopUp.Current.ShowToastWarning(AppResources.SelectProduct, Plugin.Toast.Abstractions.ToastLength.Long);

            }
            else
            {
                HoldProduct Holdsale = new HoldProduct
                {
                    saleProducts = SaleProducts,
                    created_at = DateTime.Now,
                    total = totallbl.Text,
                    subtotal = subtotallbl.Text,
                    discount = Disclbl.Text,
                    number = cartnolbl.Text
                };
            //    HoldList.Add(Holdsale);
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                var db = new SQLiteConnection(dbpath);
                var info = db.GetTableInfo("HoldProduct");
                var info1 = db.GetTableInfo("SaleProduct");

                if (!info.Any()&& !info1.Any())
                {
                    db.CreateTable<HoldProduct>();
                    db.CreateTable<SaleProduct>();
                    //   db.InsertAll(saleproducts);
                    db.InsertWithChildren(Holdsale);
                }
                else if (!info.Any())
                {
                    db.CreateTable<HoldProduct>();
                    db.InsertWithChildren(Holdsale);
                }
                else if (!info1.Any())
                {
                    db.CreateTable<SaleProduct>();
                    //   db.InsertAll(saleproducts);
                    db.InsertWithChildren(Holdsale);
                }
                else
                {   
                    db.InsertWithChildren(Holdsale);
                }
                foreach (var item in SaleProducts)
                {
                    item.quantity = 0;
                    item.total_price = 0;
                    item.discount = 0;
                }
               
                SaleProducts.Clear();
                Salesro.Clear();
                totallbl.Text = AppResources.Zero;
                subtotallbl.Text = AppResources.Zero;
                Disclbl.Text = AppResources.Zero;
                cartnolbl.Text = AppResources.Zero;
                Salesro = new ObservableCollection<SaleProduct>(SaleProducts);
                SalesList.ItemsSource = Salesro;
                SalesListen.ItemsSource = Salesro;
                CrossToastPopUp.Current.ShowToastSuccess(AppResources.AddedtoHold, Plugin.Toast.Abstractions.ToastLength.Long);

                //   await Navigation.PushAsync(new  HoldListPage(HoldList));
            }
        }
    }
}   