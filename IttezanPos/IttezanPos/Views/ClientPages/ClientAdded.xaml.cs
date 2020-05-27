using IttezanPos.Models;
using IttezanPos.Resources;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientAdded : PopupPage
    {
        private string edite;
        private int id;

        public ClientAdded()
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
        }

       
        public ClientAdded(string edite)
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
                Supplierlbl.Text = AppResources.UpdatedClient;
                this.edite = edite;
            }
           
        }
        public ClientAdded(int id)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            Supplierlbl.Text = AppResources.DeletedClient;
            this.id = id;
        }
    }
}