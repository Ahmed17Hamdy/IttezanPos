using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamEffects;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainClientsPage : ContentPage
    {
        public MainClientsPage()
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
            await Navigation.PushAsync(new AddingClientPage());
        }
        private async void ClientsPageNav()
        {
            await Navigation.PushAsync(new ClientsPage());
        }
        private async void OpeningbalancesPageNav()
        {
            await Navigation.PushAsync(new OpeningbalancesPage());
        }
        private async void ClientRecievableNav()
        {
            await Navigation.PushAsync(new ClientRecievable());
        }


    }
}