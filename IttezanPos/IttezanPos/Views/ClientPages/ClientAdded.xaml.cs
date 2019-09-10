using IttezanPos.Models;
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
        public ClientAdded()
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
        }

        public ClientAdded(AddClientError data)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            if (data.data.email.First() == "قيمة البريد الالكتروني مُستخدمة من قبل")
            {
                Supplierlbl.Text = AppResources.UsedEmail;
            }
        }
    }
}