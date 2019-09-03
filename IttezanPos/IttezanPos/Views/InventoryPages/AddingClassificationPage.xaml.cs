using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
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
    public partial class AddingClassificationPage : ContentPage
    {
        public AddingClassificationPage()
        {
            InitializeComponent();
        }

        private async void AddCategory_Clicked(object sender, EventArgs e)
        {
             await Navigation.PushPopupAsync(new AddCategoryPopUpPage());
        }
       
    }
}