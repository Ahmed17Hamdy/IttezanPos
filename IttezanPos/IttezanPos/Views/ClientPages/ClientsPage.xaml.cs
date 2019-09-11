
using IttezanPos.Models;
using IttezanPos.Services;
using Refit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using IttezanPos.Helpers;
using Xamarin.Forms.Xaml;

using Plugin.Connectivity;
using System;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientsPage : ContentPage
    {
        private ObservableCollection<Client> clients =new ObservableCollection<Client>();
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }
        public ClientsPage()
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
             : FlowDirection.LeftToRight;
        }

        protected override void OnAppearing()
        {
             GetData();
            base.OnAppearing();
        }
        async Task GetData()
        {
            try
            {
                ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com");
                    RootObject data = await nsAPI.GetSettings();
                    Clients = new ObservableCollection<Client>(data.message.clients);
                    listviewwww.ItemsSource = Clients;
                    ActiveIn.IsRunning = false;
                }
                else
                {
                    await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
            }
            
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
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

        private async void Listviewwww_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Client;
            await Navigation.PushAsync(new AddingClientPage(content));
        }
    }
}