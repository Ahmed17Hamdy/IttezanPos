using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewProductsPage : ContentPage
    {
        public ViewProductsPage()
        {
            InitializeComponent();
        }

        private void Scan_Tapped(object sender, EventArgs e)
        {
            Scanner();
        }
        public async void Scanner()
        {

            var ScannerPage = new ZXingScannerPage();

            _ = Navigation.PushAsync(ScannerPage);


            ScannerPage.OnScanResult += (result) =>
              {
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      _ = Navigation.PopAsync();
                  });
              };

        }
    }
}