using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingProductPage : ContentPage
    {
        public AddingProductPage()
        {
            InitializeComponent();
        }

        private void ByQuantity_Tapped(object sender, EventArgs e)
        {
            ByQuantitystk.BackgroundColor = Color.FromHex("#33b54b");
            ByQuantitylbl.TextColor = Color.White;
            ByQuantityimg.Source = "waitwhit.png";
           
            ByUnitstk.BackgroundColor = Color.Default;
            ByUnitlbl.TextColor = Color.FromHex("#33b54b");
            ByUniteimg.Source = "unitgreen.png";
        
        }

        private void ByUnit_Tapped(object sender, EventArgs e)
        {
            ByQuantitystk.BackgroundColor = Color.Default;
            ByQuantitylbl.TextColor = Color.FromHex("#33b54b");
            ByQuantityimg.Source = "waitgreen.png";

            ByUnitstk.BackgroundColor = Color.FromHex("#33b54b");
            ByUnitlbl.TextColor = Color.White;
            ByUniteimg.Source = "unitWhit.png";
        }
    }
}