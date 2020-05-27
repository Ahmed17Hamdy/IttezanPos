using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using IttezanPos.Helpers;
using IttezanPos.Resources;
using IttezanPos.Views.ClientPages;
using IttezanPos.Views.ExpensesPages;
using IttezanPos.Views.InventoryPages;
using IttezanPos.Views.PurchasingPages;
using IttezanPos.Views.ReportsPages;
using IttezanPos.Views.ReservoirPages;
using IttezanPos.Views.SalesPages;
using IttezanPos.Views.SupplierPages;
using Plugin.Connectivity;

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
        private void FlowDirectionPage()
        {


            FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
         : FlowDirection.LeftToRight;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().
         First(element => element.EnglishName.Contains(Settings.LastUserGravity));
            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            GravityClass.Grav();
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

        private async void Sales_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainSalesPage());
        }

        private async void Reports_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Reportpage());
        }
    }
}