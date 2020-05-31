using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Services;
using IttezanPos.Views.PurchasingPages.PurchasePoPups;
using IttezanPos.Views.SalesPages;
using IttezanPos.Views.SalesPages.SalesPopups;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.PurchasingPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PurchaseCheckout : ContentPage
    {
        private ObservableCollection<Payment> payments = new ObservableCollection<Payment>();
        private ObservableCollection<Product> purchasero;
        private string text;
        private string supplier_id= null;
        private string paymentid="1";
        private List<Product> purchaseProducts;

        public ObservableCollection<Payment> Payments
        {
            get { return payments; }
            set
            {
                payments = value;
                OnPropertyChanged(nameof(payments));
            }
        }
      

        public PurchaseCheckout(List<Product> purchaseProducts, string text)
        {
            InitializeComponent();
            this.purchaseProducts = purchaseProducts;
            this.text = text;
            totallbl.Text = text;
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
            MessagingCenter.Subscribe<ValuePercent>(this, "PopUpData", (value) =>
            {
                if (value.Value != 0)
                {
                    Disclbl.Text = value.Value.ToString("0.00");
                    totallbl.Text = (double.Parse(totallbl.Text) - double.Parse(Disclbl.Text)).ToString();
                }
                else
                {
                    Disclbl.Text = (value.Percentage * double.Parse(totallbl.Text)).ToString();
                    totallbl.Text = (double.Parse(totallbl.Text) - double.Parse(Disclbl.Text)).ToString();

                }

            });
            MessagingCenter.Subscribe<Client>(this, "PopUpData", (value) =>
            {

                CustName.Text = value.name;
                supplier_id = value.id.ToString();
            });
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

                        db.InsertAll(Payments);
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
                        db.InsertAll(Payments);
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
        private async void Discount_Tapped(object sender, EventArgs e)
        {       
                await Navigation.PushPopupAsync(new CalculatorPage());
        }

        private async void Supplier_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SupplierPop());
        }
        private void PaymentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListen.SelectedItem as Payment;

            paymentlbl.Text = payment.PaymentTypeEnname;
            paymentid = payment.Id.ToString();
        }
        private void PaymentListar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListar.SelectedItem as Payment;

            paymentlbl.Text = payment.PaymentType;
            paymentid = payment.Id.ToString();
        }
        private async void Nextbtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsRunning = true;
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
                        Purchaseitem products = new Purchaseitem
                        {

                            discount = Disclbl.Text,
                            products = purchaseProducts,
                            total_price = totallbl.Text,
                          
                            supplier_id = supplier_id,
                            user_id = 3,
                            payment_type = paymentid

                        };
                        var content = new MultipartFormDataContent();
                        var js = JsonConvert.SerializeObject(products);
                        content.Add(new StringContent(js, Encoding.UTF8, "text/json"), "purchasing_Order");
                        var response = await client.PostAsync("https://ittezanmobilepos.com/api/make_purchasing_Order", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsRunning = false;
                            var json = JsonConvert.DeserializeObject<SaleObject>(serverResponse);
                           App.Current.MainPage = new SuccessfulReciep(products);

                        }
                        else
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }
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