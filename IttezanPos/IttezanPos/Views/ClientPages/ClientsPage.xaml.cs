
using IttezanPos.Models;
using IttezanPos.Services;
using Refit;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using IttezanPos.Helpers;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientsPage : ContentPage
    {
        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
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
            var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com");
            RootObject data = await nsAPI.GetSettings();
            Clients = new ObservableCollection<Client>(data.message.clients);
            listviewwww.ItemsSource = Clients;
        }
    }
}