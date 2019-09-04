using Plugin.Permissions;
using System;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProductsPage : ContentPage
    {
        public ViewProductsPage()
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
    }
}