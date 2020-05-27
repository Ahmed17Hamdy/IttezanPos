using IttezanPos.Resources;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.InventoryPages.InventoryPopups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductAddedPage : PopupPage
    {
        private int id;
        private string edite;
        public ProductAddedPage()
        {
            InitializeComponent();
        }

        public ProductAddedPage(int id)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            Supplierlbl.Text = AppResources.DeletedProduct;
            this.id = id;
        }
        public ProductAddedPage(string edite)
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
            Supplierlbl.Text = AppResources.UpdatedProduct;
            this.edite = edite;
        }
    }
}