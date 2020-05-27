using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecieptPage : ContentPage
    {
       
        private List<OrderItem> products;
       private List<Product> saleproducts;
        private string paymentname;

        public RecieptPage()
        {
            InitializeComponent();
        }

        public RecieptPage(List<OrderItem> products, List<Product> saleproducts, string paymentname)
        {
            InitializeComponent();
            this.products = products;
            this.saleproducts = saleproducts;
            this.paymentname = paymentname;
            Invoiceidlbl.Text = products[0].id.ToString();
            Datelbl.Text = DateTime.Now.ToLongDateString();
            paymenttypelbl.Text = paymentname;
            Amountlbl.Text= products[0].amount_paid; ;
            RecieptList.ItemsSource = saleproducts;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}