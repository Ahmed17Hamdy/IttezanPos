using System;
using System.Linq;
using IttezanPos.Helpers;
using IttezanPos.Views.ClientPages;
using IttezanPos.Views.ExpensesPages;
using IttezanPos.Views.InventoryPages;
using IttezanPos.Views.PurchasingPages;
using IttezanPos.Views.ReservoirPages;
using IttezanPos.Views.SupplierPages;
using Plugin.Connectivity;
using Plugin.Multilingual;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public HomePage()
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

        private async void Client_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainClientsPage());
        }

        private async void Supplier_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainSuppliersPage());
        }

        private async void Resrvoir_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ReservoirPage());

        }

        private async void Expanse_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ExpensePage());
        }

        private async void Inventory_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InventoryMainPage());
        }

        private async void Purchase_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PurchasePage());
        }
    }
}