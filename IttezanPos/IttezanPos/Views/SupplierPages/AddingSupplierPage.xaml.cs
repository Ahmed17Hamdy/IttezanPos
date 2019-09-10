using IttezanPos.Models;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.ViewModels.SupplierViewModel;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingSupplierPage : ContentPage
    {
        public AddingSupplierPage()
        {
            InitializeComponent();
        }

      
        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Supplier supplier = new Supplier
                    {
                        name = SupplierNameentry.Text,
                        address = SupplierAddressentry.Text,
                        email = SupplierEmailentry.Text,
                        phone = SupplierPhoneentry.Text,
                        note = SupplierNotesentry.Text,
                    };
                    var nsAPI = RestService.For<ISupplierService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.AddSupplier(supplier);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;
                            await Navigation.PushPopupAsync(new SupplierAddedPopup());
                        }
                    }
                    catch
                    {
                        ActiveIn.IsRunning = false;
                        var data = await nsAPI.AddSupplierError(supplier);
                        await Navigation.PushPopupAsync(new SupplierAddedPopup(data));
                        SupplierEmailentry.Focus();
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
            catch (Exception ex)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }
    }
}