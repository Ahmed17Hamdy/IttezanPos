using Plugin.Permissions;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using Plugin.Media;
using IttezanPos.Models;

namespace IttezanPos.Views.InventoryPages.InventoryPopups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductImagePopUpPage : PopupPage
    {
        private MediaFile ProfilePic;
        private Color procolor;
        public AddProductImagePopUpPage()
        {
            InitializeComponent();
        }
        private async void ClosePage_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
        private void RedColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Red;
            RedColorlbl.Text = "✔";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void BlueColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Blue;
            Bluelbl.Text = "✔";
            RedColorlbl.Text = "";

            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void VoiletColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Violet;
            Voiletlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void IndigoColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Indigo;
            IndigoColor.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";

            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void GreenColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Green;

            GreenColor.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";

            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void YellowColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Yellow;

            Yellowlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void OrangeColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Orange;

            Orangelbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";

            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void CyanColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Cyan;

            Cyanlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";

            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }
        private void BrownColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.Brown;

            Brownlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";

            DarkBlue.Text = "";
        }
        private void DarkBlueColor_Tapped(object sender, EventArgs e)
        {
            procolor = Color.DarkBlue;

            DarkBlue.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";

        }

        private async void Choosepic_Tapped(object sender, EventArgs e)
        {          
                var answer =
                await DisplayAlert(AppResources.Choose, AppResources.SelectpicMode,
                 AppResources.Gallery, AppResources.Camera);
                if (answer == true)
                {
                try
                {
                    var storageStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                    if (storageStatus != PermissionStatus.Granted)
                    {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        {
                            await DisplayAlert(AppResources.Alert, AppResources.NeedCamera, AppResources.Ok);
                        }
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Storage });
                        storageStatus = results[Permission.Storage];
                    }
                    if (storageStatus == PermissionStatus.Granted)
                    {
                        ProfilePic = await CrossMedia.Current.PickPhotoAsync();
                        if (ProfilePic == null)
                            return;
                        ProfImgSource.Source = ImageSource.FromStream(() =>
                        {
                            return ProfilePic.GetStream();
                        });
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
                    return;
                }
                }
            else
            {

                try
                {
                var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (cameraStatus != PermissionStatus.Granted)
                {
                        if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                        {
                            await DisplayAlert(AppResources.Alert, AppResources.NeedCamera, AppResources.Ok);
                        }
                        var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    cameraStatus = results[Permission.Camera];
                }
                if (cameraStatus == PermissionStatus.Granted)
                {
                    ProfilePic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Test",
                        SaveToAlbum = true,
                        CompressionQuality = 75,
                        CustomPhotoSize = 50,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 2000,
                        DefaultCamera = CameraDevice.Front
                    });
                    if (ProfilePic == null)
                        return;
                    ProfImgSource.Source = ImageSource.FromStream(() =>
                    {
                        return ProfilePic.GetStream();
                    });
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
                    return;
                }
            }

        }

        private async void Savebtn_Clicked(object sender, EventArgs e)
        {
            if (ProfilePic != null)
            {
                MessagingCenter.Send(new PopUpPassParameter() { Myvalue = ProfilePic.GetStream() }, "PopUpData");
                MessagingCenter.Send(new PopUpPassParameter() { mediaFile = ProfilePic }, "PopUpData");
            }
            else
            {
                MessagingCenter.Send(new PopUpPassParameter() { productcolor = procolor }, "PopUpData");

            }
            await Navigation.PopPopupAsync();
        }
    }
}