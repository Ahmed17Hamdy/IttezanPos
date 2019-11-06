using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using IttezanPos.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Models;
using IttezanPos.Services;
using IttezanPos.Views.SalesPages.SalesPopups;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Services;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.ViewModels.SalesViewModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }
        private ObservableCollection<Payment> payments = new ObservableCollection<Payment>();
        public ObservableCollection<Payment> Payments
        {
            get { return payments; }
            set
            {
                payments = value;
                OnPropertyChanged(nameof(payments));
            }
        }
        public List<Product> saleproducts;
        private string text1;
        private string text2;
        private string text3;
        private string paymentid = "1";
        private string clienttid = null;
        public CheckOutPage(List<Product> saleproducts, string text1, string text2, string text3)
        {
            InitializeComponent();
            if (IttezanPos.Helpers.Settings.LastUserGravity == "Arabic")
            {
                PaymentListar.IsVisible = true;
                PaymentListen.IsVisible = false;
            }
            else
            {
                PaymentListar.IsVisible = false;
                PaymentListen.IsVisible = true;
            }
            MessagingCenter.Subscribe<Client>(this, "PopUpData", (value) =>
            {

                CustName.Text = value.name;
              clienttid=  value.id.ToString();
            });
            this.saleproducts = saleproducts;
          this.text2=  totallbl.Text = text2;
          this.text1=  Disclbl.Text = text1;
          this.text3=  subtotallbl.Text = text3;
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
                //   ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();

                    Payments = new ObservableCollection<Payment>(data.message.payments);
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Payment");
                        if (!info.Any())
                        {
                            db.CreateTable<Payment>();
                        }
                        else
                        {
                            db.DropTable<Payment>();
                            db.CreateTable<Payment>();
                        }

                        db.InsertAll(Clients);
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Payment");
                        if (!info.Any())
                        {
                            db.CreateTable<Payment>();
                        }
                        else
                        {
                            db.DropTable<Payment>();
                            db.CreateTable<Payment>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                        db.InsertAll(Clients);
                    }
                    PaymentListen.ItemsSource = Payments;
                    PaymentListar.ItemsSource = Payments;
                    //  ActiveIn.IsRunning = false;
                }
                else
                {
                    //   ActiveIn.IsRunning = false;
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Payment");
                        if (!info.Any())
                            db.CreateTable<Payment>();

                        Payments = new ObservableCollection<Payment>(db.Table<Payment>().ToList());
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Payment");
                        Payments = new ObservableCollection<Payment>(db.Table<Payment>().ToList());
                    }
                    PaymentListen.ItemsSource = Payments;
                    PaymentListar.ItemsSource = Payments;
                    //  await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                }
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                //  ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
                //    ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {

                //   ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }


        }


        private async void Customer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientPopup());
        }

        private void PaymentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListen.SelectedItem as Payment;

            paymentlbl.Text = payment.en_name;
            paymentid = payment.id.ToString();
        }
        private void PaymentListar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListar.SelectedItem as Payment;

            paymentlbl.Text = payment.name;
            paymentid = payment.id.ToString();
        }

        private async void Nextbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
            //    ActiveIn.IsRunning = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {


                        //ObservableCollection<sub> subs = new ObservableCollection<sub>();
                        //foreach (var item2 in saleproducts)
                        //{
                        //    subs.Add(new sub
                        //    {
                        //        id = item2.id,
                        //        quantity = item2.quantity
                        //    });
                        //}
                        OrderItem products = new OrderItem
                        {

                            discount = text1,
                            products = saleproducts,
                            total_price = text2,
                            amount_paid = Amountpaidentry.Text,
                            client_id = clienttid,
                            user_id = 3,
                            payment_type = paymentid
                            
                        };
                        var content = new MultipartFormDataContent();
                        var js = JsonConvert.SerializeObject(products);
                        content.Add(new StringContent(js, Encoding.UTF8, "text/json"), "item");
                        var response = await client.PostAsync("https://ittezanmobilepos.com/api/makeOrder", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            var json = JsonConvert.DeserializeObject<SaleObject>(serverResponse);
                            await DisplayAlert("", json.data, "OK");
                           // ser.DeleteAll();
                        //   await Navigation.PopAsync();
                        }
                        else
                        {
                     //       await DisplayAlert("Error", AppResources.ServerError, "OK");
                        }
                    }
                    //  var nsAPI = RestService.For<ISalesService>("https://ittezanmobilepos.com");
                    //  try
                    //  {
                    //      var data = await nsAPI.AddSale(item);
                    //      if (data.success == true)
                    //      {
                    //      //    ActiveIn.IsRunning = false;
                    ////          await Navigation.PushPopupAsync(new ClientAdded());

                    //      }
                    //  }
                    //  catch (Exception ex)
                    //  {
                    //     // ActiveIn.IsRunning = false;
                    ////      var data = await nsAPI.AddClientError(client);
                    // //     await Navigation.PushPopupAsync(new ClientAdded(data));
                    ////      Emailentry.Focus();
                    //  }
                }

            }
            catch (ValidationApiException validationException)
            {
              //  ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
             //   ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }
    }
}