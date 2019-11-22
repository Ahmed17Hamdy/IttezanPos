using IttezanPos.Helpers;
using IttezanPos.Models;
using Plugin.Connectivity;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;
using static IttezanPos.ViewModels.InventoryViewModel;

namespace IttezanPos.Views.ExpensesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensePage : ContentPage
    {
        public ExpensePage()
        {
            InitializeComponent();
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != "" )
            {
                ToWord toWord2 = new ToWord(Convert.ToDecimal(e.NewTextValue), new CurrencyInfo(Currencies.SaudiArabia));
                switch (Helpers.Settings.LastUserGravity)
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
                NumbertoTextlbl.Text = AppResources.ZeroText;
            }
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {

            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    addexpense client = new addexpense
                    {
                        date = DatePicker.Date.ToShortDateString(),
                        statement = Statmententry.Text,
                        user_id = 3,
                      
                        amount = double.Parse(amountentry.Text)

                    };
                    var nsAPI = RestService.For<IUpdateService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.addexpense(client);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ExpnseAdded, AppResources.Ok);
                            await Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveIn.IsRunning = false;
                        //     var data = await nsAPI.AddClientError(client);
                        // await Navigation.PushPopupAsync(new ClientAdded(data));
                        //  Emailentry.Focus();
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