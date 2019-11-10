using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulReciep : ContentPage
    {
      
        private OrderItem products;

        public SuccessfulReciep()
        {
           
        }

      

        public SuccessfulReciep(OrderItem products)
        {
            InitializeComponent();
            this.products = products;
            Totallbl.Text = products.total_price;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage =new NavigationPage( new MainSalesPage());
        }
    }
}