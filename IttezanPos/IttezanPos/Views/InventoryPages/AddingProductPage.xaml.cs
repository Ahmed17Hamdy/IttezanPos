using IttezanPos.Models;
using IttezanPos.Services;
using IttezanPos.Views.InventoryPages.InventoryPopups;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Refit;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
using ZXing.Net.Mobile.Forms;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingProductPage : ContentPage
    {
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
        public AddingProductPage()
        {
            InitializeComponent();
         
            MessagingCenter.Subscribe<PopUpPassParameter>(this, "PopUpData", (value) =>
            {
                Stream receivedData = value.Myvalue;
                Color color = value.productcolor;
                productimg.BackgroundColor = color;
                productimg.Source = ImageSource.FromStream(() => { return receivedData; });
            });
        }
        protected override bool OnBackButtonPressed()
        {
            Categorylist.Unfocus();
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
                    var eachCategories = new ObservableCollection<EachCategory>(data.message.each_category);
                    foreach (var item in eachCategories)
                    {
                        
                        Categories.Add(item.category);
                        //foreach (var i2tem in Categories)
                        //{
                        //    Categorylist.Items.Add(i2tem.name);
                        //}
                    }
                    Categorylist.ItemsSource = Categories;
                }
                else
                {
                    await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
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
        private void Categorylist_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = Categorylist.SelectedItem as Category;
            Preferences.Set("category_id", category.id);
           
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
    }
}