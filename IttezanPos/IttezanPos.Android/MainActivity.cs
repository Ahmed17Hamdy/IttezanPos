
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using System.Linq;
using FFImageLoading.Forms.Platform;

namespace IttezanPos.Droid
{
    [Activity(Label = "IttezanPos", Icon = "@drawable/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,ScreenOrientation =ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] permission =
          {
            Android.Manifest.Permission.AccessCheckinProperties,
            Android.Manifest.Permission.AccessFineLocation,
            Android.Manifest.Permission.AccessCoarseLocation,
            Android.Manifest.Permission.AccessMockLocation,
            Android.Manifest.Permission.AccessWifiState,
            Android.Manifest.Permission.Bluetooth,
            Android.Manifest.Permission.BluetoothAdmin,
            Android.Manifest.Permission.BluetoothPrivileged,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ReadExternalStorage,
            Android.Manifest.Permission.Internet
            };
        const int RequestId = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
           
     //       CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //ChechSdk();
            Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
            FormsMaterial.Init(this, savedInstanceState);
            XamEffects.Droid.Effects.Init();
            CachedImageRenderer.Init(true);
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");

       //     Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            //PdfSharp.Xamarin.Forms.Droid.Platform.Init();
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjEyOTEwQDMxMzcyZTM0MmUzMEhBclpMNTZSNlpLQTFzUVlhbjFIR3d2aXFlVUFXcmNURVVycTMxQUZOdFk9");

            LoadApplication(new App());
        }
        public void ChechSdk()
        {
            if ((int)Build.VERSION.SdkInt > 23)
            {
                RequestPermissions(permission, RequestId);
                return;
            }
            else
            {

                return;
            }
        }
        public async override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                if (Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopupStack.Any())
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                // Do something if there are some pages in the `PopupStack`
             
            }
            else
            {
                return;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Shiny.AndroidShinyHost.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}