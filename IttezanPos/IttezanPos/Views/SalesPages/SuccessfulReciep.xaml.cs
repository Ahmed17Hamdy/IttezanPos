using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using IttezanPos.Views.PurchasingPages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulReciep : ContentPage
    {
      
        private OrderItem products;
        private Purchaseitem products1;

    
        public SuccessfulReciep(OrderItem products)
        {
            InitializeComponent();
            this.products = products;
            Totallbl.Text = products.total_price;
            Salestk.IsVisible = true;
            Purchasestk.IsVisible = false;
        }

        public SuccessfulReciep(Purchaseitem products1)
        {
            InitializeComponent();
            this.products1 = products1;
            Totallbl.Text = products1.total_price;
            Salestk.IsVisible = false;
            Purchasestk.IsVisible = true;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage =new NavigationPage( new MainSalesPage());
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new PurchasePage());
        }
    }
}