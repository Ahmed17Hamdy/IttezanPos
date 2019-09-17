using IttezanPos.Models;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.ViewModels.ClientViewModel;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingClientPage : ContentPage
    {
        private Client content;
        private string edite;

        public AddingClientPage()
        {
            InitializeComponent();
        }

        public AddingClientPage(Client content)
        {
            InitializeComponent();
            this.content = content;
            ClientNameentry.Text = content.name;
            ClientAddressentry.Text = content.address;
            Emailentry.Text = content.email;
            phoneentry.Text = content.phone;
            Notesentry.Text = content.note;
            Limitentry.Text = content.limitt.ToString();
            Detailsstk.IsVisible = true;
            Savebtn.IsVisible = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Client client = new Client
                    {                        
                       name = ClientNameentry.Text,
                       address=ClientAddressentry.Text,
                       email=Emailentry.Text,
                       phone=phoneentry.Text,
                       note=Notesentry.Text,
                       limitt=int.Parse(Limitentry.Text)
                    };
                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.AddClient(client);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;                         
                            await Navigation.PushPopupAsync(new ClientAdded());
                        }
                    }
                    catch
                    {
                        ActiveIn.IsRunning = false;
                        var data = await nsAPI.AddClientError(client);
                        await Navigation.PushPopupAsync(new ClientAdded(data));
                        Emailentry.Focus();
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
        private async void Update_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Client client = new Client
                    {
                        client_id = content.id,
                        name = ClientNameentry.Text,
                        address = ClientAddressentry.Text,
                        email = Emailentry.Text,
                        phone = phoneentry.Text,
                        note = Notesentry.Text,
                        limitt=int.Parse(Limitentry.Text)
                    };
                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.UpdateClient(client);
                    if (data.success == true)
                    {

                        ActiveIn.IsRunning = false;
                        await Navigation.PushPopupAsync(new ClientAdded(edite));
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

        private async void Deletebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {

                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.DeleteClient(content.id);
                    if (data.success == true)
                    {

                        ActiveIn.IsRunning = false;
                        await Navigation.PushPopupAsync(new ClientAdded(content.id));
                        await Navigation.PopAsync();
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