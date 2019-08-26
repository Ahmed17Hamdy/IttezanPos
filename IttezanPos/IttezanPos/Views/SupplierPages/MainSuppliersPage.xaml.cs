using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamEffects;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IttezanPos.Views.SupplierPages;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainSuppliersPage : ContentPage
    {
        public MainSuppliersPage()
        {
            InitializeComponent();
            Commandss();
        }
        private void Commandss()
        {
            Commands.SetTap(AddClientGrid, new Command(() => {
                AddingClientPageNav();
            }));
            Commands.SetTap(ViewClientGrid, new Command(() => {
                ClientsPageNav();
            }));
            Commands.SetTap(CustomeropeningbalancesGrid, new Command(() => {
                OpeningbalancesPageNav();
            }));
            Commands.SetTap(ViewCustomerreceivablesGrid, new Command(() => {
                ClientRecievableNav();
            }));
        }
        private async void AddingClientPageNav()
        {
            await Navigation.PushAsync(new AddingSupplierPage());
        }
        private async void ClientsPageNav()
        {
            await Navigation.PushAsync(new SuppliersPage());
        }
        private async void OpeningbalancesPageNav()
        {
            await Navigation.PushAsync(new SuppliersOpeningbalances());
        }
        private async void ClientRecievableNav()
        {
            await Navigation.PushAsync(new SupplierRecivable());
        }
    }
}