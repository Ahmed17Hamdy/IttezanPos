using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Helpers;
using Plugin.Connectivity;
using Plugin.Multilingual;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SideMenuPage : ContentPage
    {
        public SideMenuPage()
        {
            InitializeComponent();
            FlowDirectionPage();
        }

        private async void FlowDirectionPage()
        {
           
            if (CrossConnectivity.Current.IsConnected)
            {
                FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
             : FlowDirection.LeftToRight;
                CrossMultilingual.Current.CurrentCultureInfo = CrossMultilingual.Current.NeutralCultureInfoList.ToList().
             First(element => element.EnglishName.Contains(Settings.LastUserGravity));
                AppResources.Culture = CrossMultilingual.Current.CurrentCultureInfo;
            }
            else
            {                
                    await DisplayAlert(AppResources.Error, AppResources.ErrorMessage, AppResources.Ok);                
            }
        }
    }
} 