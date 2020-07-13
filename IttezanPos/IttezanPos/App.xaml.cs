using IttezanPos.Views.Master;
using Plugin.Connectivity;

using System.Linq;
using Xamarin.Forms;
using IttezanPos.Helpers;
using IttezanPos.Views.SettingsPages;
using IttezanPos.Views.MainPage;
using DLToolkit.Forms.Controls;

namespace IttezanPos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjEyOTIxQDMxMzcyZTM0MmUzMEhBclpMNTZSNlpLQTFzUVlhbjFIR3d2aXFlVUFXcmNURVVycTMxQUZOdFk9");


FlowListView.Init();
            FlowDirectionPage();
            MainPage = new NavigationPage( new MasterPage());
        }
        private void FlowDirectionPage()
        {
                _ = Settings.LastUserGravity == "English";
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
