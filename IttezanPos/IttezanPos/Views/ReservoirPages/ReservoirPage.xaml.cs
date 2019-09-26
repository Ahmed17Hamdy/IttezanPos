using IttezanPos.Helpers;
using IttezanPos.Models;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;
using static IttezanPos.ViewModels.BoxViewModel;
using Settings = IttezanPos.Helpers.Settings;

namespace IttezanPos.Views.ReservoirPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservoirPage : ContentPage
    {
        List<CurrencyInfo> currencies = new List<CurrencyInfo>();
        private int type_Operation;
        private int disc_purchasing_suppliers;
        private int disc_expenses;
        private int add_sales_clients;

        public ReservoirPage()
        {
            InitializeComponent();
            type_Operation = 1;
            disc_purchasing_suppliers = 0;
            disc_expenses=0;
            add_sales_clients = 0;
        }

    private  void Addrdbtn_Checked(object sender, EventArgs e)
        {
            Deductionbtn.IsVisible = false;
            Addingbtn.IsVisible = true;
            type_Operation = 1;
        }

        private void Deductrdbtn_Checked(object sender, EventArgs e)
        {
            Addingbtn.IsVisible = false;
            Deductionbtn.IsVisible = true;
            type_Operation = 0;
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
              if(e.NewTextValue!= "" )
            {
                ToWord toWord2 = new ToWord(Convert.ToDecimal(e.NewTextValue), new CurrencyInfo(Currencies.SaudiArabia));

                switch (Settings.LastUserGravity)
                {
                    case "Arabic":
                        NumbertoTextlbl.Text = toWord2.ConvertToArabic();
                        break;
                    case "English":
                        NumbertoTextlbl.Text = toWord2.ConvertToEnglish();
                        break;
                }
            }
            else
            {                
                    NumbertoTextlbl.Text = AppResources.ZeroText ;
            } 
        }

        private void Salescustswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                add_sales_clients = 1;
            }
            else
            {
                add_sales_clients = 0;
            }
        }

        private void Purchaseandsuppliersswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                disc_purchasing_suppliers = 1;
            }
            else
            {
                disc_purchasing_suppliers = 0;
            }
        }

        private void Expensesswitch_Toggled(object sender, ToggledEventArgs e)
        {
            string stateName = e.Value ? "ON" : "OFF";
            if (stateName == "ON")
            {
                disc_expenses = 1;
            }
            else
            {
                disc_expenses = 0;
            }
        }

        private async void Addingbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Box box = new Box
                    {
                        add_sales_clients = add_sales_clients,
                        disc_expenses = disc_expenses,
                        disc_purchasing_suppliers = disc_purchasing_suppliers,
                        note = NotesEntry.Text,
                        amount = AmountEntry.Text
                    };
                    var nsAPI = RestService.For<IBoxService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.Addinterproccess(box);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.AddedSucc, AppResources.Ok);
                            Balancelbl.Text = data.message.box.balance;
                        }
                    }
                    catch(Exception ex)
                    {
                        ActiveIn.IsRunning = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                    }
                }

            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }

        private async void Deductionbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Box box = new Box
                    {
                        add_sales_clients = add_sales_clients,
                        disc_expenses = disc_expenses,
                        disc_purchasing_suppliers = disc_purchasing_suppliers,
                        note = NotesEntry.Text,
                        amount = AmountEntry.Text
                    };
                    var nsAPI = RestService.For<IBoxService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.Addinterproccess(box);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.AddedSucc, AppResources.Ok);
                        }
                    }
                    catch
                    {
                        ActiveIn.IsRunning = false;
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                    }
                }

            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }
    }
}