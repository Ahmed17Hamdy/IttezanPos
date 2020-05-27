using IttezanPos.Resources;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierAddedPopup : PopupPage
    {
        private string edite;
        private int id;

       
        public SupplierAddedPopup()
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
        }

        public SupplierAddedPopup(string edite)
        {
            InitializeComponent();
            if (edite == "قيمة البريد الالكتروني مُستخدمة من قبل")
            {
                Frame.BorderColor = Color.Red;
                Supplierlbl.Text = AppResources.UsedEmail;
            }
            else
            {
                Frame.BorderColor = Color.FromHex("#33b54b");
                Supplierlbl.Text = AppResources.UpdatedSupplier;
               
            }
         
        }

        public SupplierAddedPopup(int id)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            Supplierlbl.Text = AppResources.DeletedSupplier;
            this.id = id;
        }
    }
}