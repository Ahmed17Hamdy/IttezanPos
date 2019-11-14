using IttezanPos.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.PurchasingPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class purchasePopUpPage : PopupPage
    {
        private Product selectedro;
        private DateTime expire_date;

        public purchasePopUpPage()
        {
            InitializeComponent();
        }

        public purchasePopUpPage(Product selectedro)
        {
            InitializeComponent();
            this.selectedro = selectedro;
            Old_Purchaselbl.Text = selectedro.purchase_price.ToString("0.00");
            Old_salelbl.Text = selectedro.sale_price.ToString("0.00");
            Stocklbl.Text = selectedro.stock.ToString();
            pronamelbl.Text = selectedro.name;
        }

     
        private async void Next_Tapped(object sender, EventArgs e)
        {
            if (expire_date != null)
            {
                MessagingCenter.Send(new ValuePercent() { Value = New_salelbl.Text, Percentage = New_Purchaselbl.Text, expiredate = expire_date }, "PopUpData");
                await Navigation.PopPopupAsync();
            }
            else
            {
                await DisplayAlert(AppResources.Alert, AppResources.SelectExireDate, AppResources.Ok);
                datepi.Focus();
            }
           
        }

        private async void Closelbl_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
          expire_date=  e.NewDate;
        }
    }
}