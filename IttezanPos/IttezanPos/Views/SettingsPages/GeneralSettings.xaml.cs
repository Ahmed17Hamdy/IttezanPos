using Plugin.Connectivity;

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
using System.Threading;
using System.Globalization;
using IttezanPos.Resources;

namespace IttezanPos.Views.SettingsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeneralSettings : ContentPage
    {
        public GeneralSettings()
        {
            InitializeComponent();
        }

        private  void Button_Clicked(object sender, EventArgs e)
        {
           
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                    .First(element => element.EnglishName.Contains("English"));
                AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
                Settings.LastUserGravity = "English";
            Application.Current.MainPage = new NavigationPage(new MasterPage());
          

        }

        private  void Button_Clicked_1(object sender, EventArgs e)
        {
           
            var language =   Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList()
                    .First(element => element.EnglishName.Contains("Arabic"));
            Thread.CurrentThread.CurrentUICulture = language;
            AppResources.Culture = language;

                Settings.LastUserGravity = "Arabic";
                GravityClass.Grav();

            Application.Current.MainPage = new NavigationPage(new MasterPage());
        }
    }
}