using Plugin.Connectivity;
using Plugin.Multilingual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IttezanPos.Views.MainPage;
using IttezanPos.Views.Master;

namespace IttezanPos.Views.SettingsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralSettings : ContentPage
    {
        public GeneralSettings()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList()
                    .First(element => element.EnglishName.Contains("English"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
                Settings.LastUserGravity = "English";
                App.Current.MainPage = new MasterPage();
            }
            else await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);

        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList()
                    .First(element => element.EnglishName.Contains("Arabic"));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo ;

                Settings.LastUserGravity = "Arabic";
                GravityClass.Grav();

                App.Current.MainPage = new MasterPage();
            }
            else
            {
                await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);
            }
        }
    }
}