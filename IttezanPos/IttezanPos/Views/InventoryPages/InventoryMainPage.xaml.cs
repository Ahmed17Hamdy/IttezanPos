using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamEffects;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryMainPage : ContentPage
    {
        public InventoryMainPage()
        {
            InitializeComponent();
            Commandss();
        }
        private void Commandss()
        {
            Commands.SetTap(AddProductGrid, new Command(() => {
                AddingProductPageNav();
            }));
            Commands.SetTap(ViewClientGrid, new Command(() => {
                ProductsPageNav();
            }));
            Commands.SetTap(AddingNewClassificationGrid, new Command(() => {
                AddingClassificationPageNav();
            }));
            Commands.SetTap(EdittingSalariesGrid, new Command(() => {
                EdditingSalariesNav();
            }));
        }
 
        private async void AddingProductPageNav()
        {
            await Navigation.PushAsync(new AddingProductPage());
        }
        private async void ProductsPageNav()
        {
            await Navigation.PushAsync(new ViewProductsPage());
        }
        private async void AddingClassificationPageNav()
        {
            await Navigation.PushAsync(new AddingClassificationPage());
        }
        private async void EdditingSalariesNav()
        {
            await Navigation.PushAsync(new EditingProductSalaryPage());
        }
    }
}