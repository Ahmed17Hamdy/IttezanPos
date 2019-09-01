using IttezanPos.Views.Master;
using Plugin.Connectivity;
using Plugin.Multilingual;
using System.Linq;
using Xamarin.Forms;
using IttezanPos.Helpers;
using IttezanPos.Views.SettingsPages;
using IttezanPos.Views.MainPage;

namespace IttezanPos
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            FlowDirectionPage();
            MainPage = new MasterPage();
        }
        private void FlowDirectionPage()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().
             First(element => element.EnglishName.Contains(Settings.LastUserGravity));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            }

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
