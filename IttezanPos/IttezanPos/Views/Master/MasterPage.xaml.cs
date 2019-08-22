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
    public partial class MasterPage : MasterDetailPage
    {
        public MasterPage()
        {
            InitializeComponent();
            if (Device.Idiom == TargetIdiom.Tablet)
                MasterBehavior = MasterBehavior.Popover;
            FlowDirectionPage();          
            masterPage.listView.ItemSelected += OnItemSelected;
            masterPage.newlist.ItemSelected += OnItemSelected;
            masterPage.listhelp.ItemSelected += OnItemSelected;
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
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.listView.SelectedItem = null;
                masterPage.newlist.SelectedItem = null;
                masterPage.listhelp.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}