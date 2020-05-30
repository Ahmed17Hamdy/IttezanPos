using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Services;
using IttezanPos.Views.InventoryPages.InventoryPopups;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Refit;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;
using static IttezanPos.ViewModels.ProductViewModel;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingProductPage : ContentPage
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private int category_Id;
        private MediaFile image ;
        private Product product;
        private int product_Id;
        private string text;

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(categories));
            }
        }
        public AddingProductPage()
        {
            InitializeComponent();
         
            MessagingCenter.Subscribe<PopUpPassParameter>(this, "PopUpData", (value) =>
            {
                Stream receivedData = value.Myvalue;
                image = value.mediaFile;
                Color color = value.productcolor;
                productimg.BackgroundColor = color;
                productimg.Source = ImageSource.FromStream(() => { return receivedData; });
            });
        }

        public AddingProductPage(Product product)
        {
            InitializeComponent();
            Savebtn.IsVisible = false;
            Detailsstk.IsVisible = true;
            MessagingCenter.Subscribe<PopUpPassParameter>(this, "PopUpData", (value) =>
            {
                Stream receivedData = value.Myvalue;
                image = value.mediaFile;
                Color color = value.productcolor;
                productimg.BackgroundColor = color;
                productimg.Source = ImageSource.FromStream(() => { return receivedData; });
            });
            this.product = product;
            EntryName.Text = product.name;
            NotesEntry.Text = product.description;
            category_Id = product.category_id;
            PurchaseEntry.Text = product.purchase_price.ToString();
            SellEntry.Text = product.sale_price.ToString();
            StockQuantity.Text = product.stock.ToString();
            CategoryListar.Title = product.catname;
            CategoryListar.Title = product.catname;
            //if (product.expiration_date.Value.DateTime != null)
            //{
            //    Datepicker.Date = product.expiration_date.Value.DateTime;
            //}
            //else
            //{
            //    Datepicker.Date = DateTime.Now;
            //}
            //   Datepicker.Date = product.expiration_date.Value.DateTime != null ? product.expiration_date.Value.DateTime : DateTime.Now;

            productimg.Source = "https://ittezanmobilepos.com/dashboard_files/imgss/"+ product.image;
            product_Id = product.id;
            EnglishEnnamentry.Text = product.Enname;
            

        }

        protected override bool OnBackButtonPressed()
        {
            CategoryListen.Unfocus();
            CategoryListar.Unfocus();
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
              
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();
                    var eachCategories = new ObservableCollection<Category>(data.message.categories);
                    Categories = eachCategories;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        db.CreateTable<Category>();
                        db.DeleteAll<Category>();
                        db.InsertAll(Categories);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        
                        db.DeleteAll<Category>();
                       
                        db.CreateTable<Category>();
                       
                        db.InsertAll(Categories);
                    }
                    CategoryListar.ItemsSource = Categories;
                    CategoryListar.ItemsSource = Categories;
                }
                else
                {
                    ActiveIn.IsRunning = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        Categories = new ObservableCollection<Category>(db.Table<Category>().ToList());
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        Categories = new ObservableCollection<Category>(db.Table<Category>().ToList());
                    }
                }
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
             
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
            
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {
               
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }
        
        private void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryListar.SelectedItem as Category;
            category_Id = category.category.id;
        }
        private void CategoryListen_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryListen.SelectedItem as Category;
            category_Id = category.category.id;
        }
        private void ByQuantity_Tapped(object sender, EventArgs e)
        {
            ByQuantitystk.BackgroundColor = Color.FromHex("#33b54b");
            ByQuantitylbl.TextColor = Color.White;
            ByQuantityimg.Source = "waitwhit.png";
           
            ByUnitstk.BackgroundColor = Color.Default;
            ByUnitlbl.TextColor = Color.FromHex("#33b54b");
            ByUniteimg.Source = "unitgreen.png";
        
        }

        private void ByUnit_Tapped(object sender, EventArgs e)
        {
            ByQuantitystk.BackgroundColor = Color.Default;
            ByQuantitylbl.TextColor = Color.FromHex("#33b54b");
            ByQuantityimg.Source = "waitgreen.png";

            ByUnitstk.BackgroundColor = Color.FromHex("#33b54b");
            ByUnitlbl.TextColor = Color.White;
            ByUniteimg.Source = "unitWhit.png";
        }

        private async void ChooseImage_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddProductImagePopUpPage());
        }

        private void Trackinstore_Toggled(object sender, ToggledEventArgs e)
        {
            Preferences.Set("stock", "my_value");
        }

        private async void AddCategorybtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AddCategoryPopUpPage());
        }

        private void SellEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PurchaseEntry.Text != null)
            {
                if (e.NewTextValue != "" && int.Parse(e.NewTextValue) < int.Parse(PurchaseEntry.Text))
                {
                    salsesErrorlbl.IsVisible = true;
                }
                else
                {
                    salsesErrorlbl.IsVisible = false;
                }
            }
            else
            {
                salsesErrorlbl.IsVisible = true;
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
        private bool AllNeeded()
        {
            if (image==null )
            {
                ActiveIn.IsRunning = false;
                DisplayAlert(AppResources.Error, AppResources.AddImage, AppResources.Ok);
                OpenImagePopup();
                return false;
            }
            else if (category_Id== 0)
            {
                ActiveIn.IsRunning = false;
                DisplayAlert(AppResources.Error, AppResources.ChooseCategory, AppResources.Ok);
                CategoryListen.Focus();
                CategoryListar.Focus();
                return false;
            }          
            return true;
        }

        private async void OpenImagePopup()
        {
            await Navigation.PushPopupAsync(new AddProductImagePopUpPage());
        }
        private async void Savebtn_Clicked(object sender, EventArgs e)
        {
            ActiveIn.IsRunning = true;
            if (AllNeeded() == true)
            {
                Product product = new Product
                {
                    name = EntryName.Text,    
                    Enname=EnglishEnnamentry.Text,
                    category_id = category_Id,
                    description = NotesEntry.Text,
                    sale_price = int.Parse(SellEntry.Text),
                    purchase_price = int.Parse(PurchaseEntry.Text),
                    locale = "ar",
                    stock = 35  , user_id="2"            
                };
                StringContent name = new StringContent(product.name);
                StringContent Enname = new StringContent(product.Enname);
                StringContent category_id = new StringContent(product.category_id.ToString());
                StringContent description = new StringContent(product.description);
                StringContent sale_price = new StringContent(product.sale_price.ToString());
                StringContent purchase_price = new StringContent(product.purchase_price.ToString());
                StringContent locale = new StringContent(product.locale);
                StringContent user_id = new StringContent(product.user_id);
                StringContent stock = new StringContent(product.stock.ToString());
                var content = new MultipartFormDataContent();
                content.Add(name, "name");
                content.Add(Enname, "Enname");
                content.Add(category_id, "category_id");
                content.Add(description, "description");
                content.Add(sale_price, "sale_price");
                content.Add(purchase_price, "purchase_price");
                content.Add(locale, "locale");
                content.Add(user_id, "user_id");
                content.Add(stock, "stock");
                content.Add(new StreamContent(image.GetStream()), "image", $"{image.Path}");                      
                HttpClient httpClient = new HttpClient();
                try
                {
                    var httpResponseMessage = await httpClient.PostAsync("https://ittezanmobilepos.com/api/addproduct",
                        content);
                    var serverResponse = httpResponseMessage.Content.ReadAsStringAsync().Result.ToString();
                    if (serverResponse == "false")
                    {
                        ActiveIn.IsRunning = false;
                    await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
                    else
                    {
                        try
                        {
                            try
                            {
                                ActiveIn.IsRunning = false;
                                var JsonResponse = JsonConvert.DeserializeObject<AddProduct>(serverResponse);
                                if (JsonResponse.success == true)
                                {
                                    await PopupNavigation.Instance.PushAsync(new ProductAddedPage() );
                                    await Navigation.PushAsync(new InventoryMainPage());
                                }

                            }
                            catch (Exception ex)
                            {
                                ActiveIn.IsRunning = false;
                                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                                //var JsonResponse = JsonConvert.DeserializeObject<RegisterResponse>(serverResponse);
                                //if (JsonResponse.success == false)
                                //{
                                //    ActiveIn.IsRunning = false;
                                //    await PopupNavigation.Instance.PushAsync(new RegisterPopup(JsonResponse.data));
                                //}
                            }
                        }
                        catch (Exception ex)
                        {
                            ActiveIn.IsRunning = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        return;
                        }

                    }

                }
                catch (Exception ex)
                {
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }

            }


        }

        private async void Updatebtn_Clicked(object sender, EventArgs e)
        {
            Product product = new Product
            {
                product_id= product_Id,
                name = EntryName.Text,
                Enname=EnglishEnnamentry.Text,
                category_id = category_Id,
                description = NotesEntry.Text,
                sale_price = int.Parse(SellEntry.Text),
                purchase_price = int.Parse(PurchaseEntry.Text),
                locale = "ar",
                stock = 35,
                user_id = "2"
            };
            StringContent product_id = new StringContent(product.product_id.ToString());
            StringContent name = new StringContent(product.name);
            StringContent Enname = new StringContent(product.Enname);
            StringContent category_id = new StringContent(product.category_id.ToString());
            StringContent description = new StringContent(product.description);
            StringContent sale_price = new StringContent(product.sale_price.ToString());
            StringContent purchase_price = new StringContent(product.purchase_price.ToString());
            StringContent locale = new StringContent(product.locale);
            StringContent user_id = new StringContent(product.user_id);
            StringContent stock = new StringContent(product.stock.ToString());
            var content = new MultipartFormDataContent();
            content.Add(product_id, "product_id");
            content.Add(name, "name");
            content.Add(Enname, "Enname");
            content.Add(category_id, "category_id");
            content.Add(description, "description");
            content.Add(sale_price, "sale_price");
            content.Add(purchase_price, "purchase_price");
            content.Add(locale, "locale");
            content.Add(user_id, "user_id");
            content.Add(stock, "stock");
            if (image != null)
            {
                content.Add(new StreamContent(image.GetStream()), "image", $"{image.Path}");
            }           
            HttpClient httpClient = new HttpClient();
            try
            {
                var httpResponseMessage = await httpClient.PostAsync("https://ittezanmobilepos.com/api/updateproduct",
                    content);
                var serverResponse = httpResponseMessage.Content.ReadAsStringAsync().Result.ToString();
                if (serverResponse == "false")
                {
                    ActiveIn.IsRunning = false;
                    await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
                else
                {
                    try
                    {
                        try
                        {
                            ActiveIn.IsRunning = false;
                            var JsonResponse = JsonConvert.DeserializeObject<RootObject>(serverResponse);
                            if (JsonResponse.success == true)
                            {
                                await PopupNavigation.Instance.PushAsync(new ProductAddedPage(text));
                                await Navigation.PushAsync(new InventoryMainPage());
                            }

                        }
                        catch (Exception ex)
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                            //var JsonResponse = JsonConvert.DeserializeObject<RegisterResponse>(serverResponse);
                            //if (JsonResponse.success == false)
                            //{
                            //    ActiveIn.IsRunning = false;
                            //    await PopupNavigation.Instance.PushAsync(new RegisterPopup(JsonResponse.data));
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveIn.IsRunning = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }

        }

        private async void Deletebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {

                    var nsAPI = RestService.For<IProductService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.DeleteClient(product.id);
                    if (data.success == true)
                    {

                        ActiveIn.IsRunning = false;
                        await Navigation.PushPopupAsync(new ProductAddedPage(product.id));
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
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
    }
}