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
      
        private List<OrderItem> products;
        private Purchaseitem products1;
        private List<Product> saleproducts;
        private string paymentname;


        public SuccessfulReciep(List<OrderItem> products, List<Product> saleproducts, string paymentname)
        {
            InitializeComponent();
            this.products = products;
            this.saleproducts = saleproducts;
            this.paymentname = paymentname;
            Totallbl.Text = products[0].total_price;
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

        private async void ViewReciept_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new RecieptPage(products,saleproducts,  paymentname)) );
        }
      
    }
}