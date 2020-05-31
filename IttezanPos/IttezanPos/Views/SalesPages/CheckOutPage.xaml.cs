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
using IttezanPos.Resources;
using Plugin.Toast;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
       
       
        public List<Product> saleproducts;
        private string text1;
        private string text2;
        private string text3;
        private string paymentid = "1";
        private string paymentname= "cash";
        private string clienttid = null;
        private string amount_paid;
        public ObservableCollection<Payment> payments;
        private List<OrderItem> orderitems;
        private List<Product> Products;

        public CheckOutPage(List<Product> saleproducts, string text1, string text2, string text3, ObservableCollection<Payment> payments, List<Product> Products)
        {
            InitializeComponent();
            PaymentListar.ItemsSource = PaymentListen.ItemsSource = payments;
            Amountpaidentry.Text = text2;
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
            this.Products = Products;
        }
       
     


        private async void Customer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientPopup());
        }

        private void PaymentList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListen.SelectedItem as Payment;

            paymentname = paymentlbl.Text = payment.PaymentTypeEnname;
            paymentid = payment.Id.ToString();
        }
        private void PaymentListar_SelectedIndexChanged(object sender, EventArgs e)
        {
            var payment = PaymentListar.SelectedItem as Payment;

           paymentname =   paymentlbl.Text = payment.PaymentType;
            paymentid = payment.Id.ToString();
        }

        private async void Nextbtn_Clicked(object sender, EventArgs e)
        {
            if (paymentid == "3" && clienttid == null)
            {
                await DisplayAlert(AppResources.Alert, AppResources.ChooseCustomer, AppResources.Ok);
                await Navigation.PushAsync(new ClientPopup());
            }
            else if (paymentid == "3" && clienttid != null)
            {
                try
                {
                    ActiveIn.IsRunning = true;
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        using (var client = new HttpClient())
                        {

                            if (Amountpaidentry.Text != "" )
                            {
                                amount_paid = Amountpaidentry.Text;                              
                            }
                            else
                            {
                                amount_paid = "0";
                            //    CrossToastPopUp.Current.ShowToastWarning(AppResources.SelectProduct, Plugin.Toast.Abstractions.ToastLength.Long);
                            }
                            OrderItem products = new OrderItem
                            {
                                discount = text1,
                                products = saleproducts,
                                total_price = text2,
                                amount_paid = amount_paid,
                                client_id = clienttid,
                                user_id = 3,
                                payment_type = paymentid
                            };
                            var content = new MultipartFormDataContent();
                            var js = JsonConvert.SerializeObject(products);
                            content.Add(new StringContent(js, Encoding.UTF8, "text/json"), "products");
                            var response = await client.PostAsync("https://ittezanmobilepos.com/api/makeOrder", content);
                            if (response.IsSuccessStatusCode)
                            {
                                var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                                ActiveIn.IsRunning = false;
                                var json = JsonConvert.DeserializeObject<SaleObject>(serverResponse);

                                await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                            }
                            else
                            {
                                ActiveIn.IsRunning = false;
                                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                            }

                        }

                    }
                    else
                    {
                        if (Amountpaidentry.Text != "")
                        {
                            amount_paid = Amountpaidentry.Text;
                        }
                        else
                        {
                            amount_paid = "0";
                        }

                        OrderItem product = new OrderItem
                        {
                            discount = text1,
                            products = saleproducts,
                            total_price = text2,
                            amount_paid = amount_paid,
                            client_id = clienttid,
                            user_id = 3,

                            payment_type = paymentid
                        };
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("SaleObject");

                        if (!info.Any())
                        {
                            db.CreateTable<OrderItem>();

                            db.Insert(product);
                        }
                        else
                        {
                            db.Insert(product);
                        }
                        orderitems = (db.Table<OrderItem>().ToList());
                        
                        var client = (db.Table<Client>().ToList().Where(clien => clien.id == int.Parse(clienttid)).FirstOrDefault());
                        client.paid_amount = client.paid_amount + double.Parse(amount_paid);
                        var amount = (double.Parse(text2) - double.Parse(amount_paid));
                        if (amount >= 0)
                        {
                            client.remaining = client.remaining + amount;
                        }
                        else
                        {
                            client.creditorit = client.creditorit + amount;
                        }
                      
                        client.total_amount = client.total_amount + double.Parse(text2);   
                        client.updated_at = DateTime.Now.ToString();
                        foreach (var item in Products)
                        {
                            foreach (var itemp in saleproducts)
                            {
                                if (item.id == itemp.id)
                                {
                                    item.stock = item.stock - int.Parse(itemp.quantity.ToString());
                                }
                            }
                        }
                        db.Update(client);
                        db.UpdateAll(Products);
                        await Navigation.PushAsync(new SuccessfulReciep(orderitems, saleproducts, paymentname));

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
            else
            {
                try
                {
                    ActiveIn.IsRunning = true;
                    if (CrossConnectivity.Current.IsConnected)
                    {
                        using (var client = new HttpClient())
                        {

                            if (Amountpaidentry.Text != "" &&  double.Parse(Amountpaidentry.Text) >= double.Parse(text2.ToString()))
                            {
                                amount_paid = Amountpaidentry.Text;
                                OrderItem orderItem = new OrderItem();
                                orderItem.amount_paid = amount_paid;
                                orderItem.client_id = clienttid;
                                orderItem.created_at = DateTime.Now;
                                orderItem.discount = text1;
                                orderItem.products = saleproducts;
                                orderItem.total_price = text2;


                                orderItem.user_id = 3;
                                orderItem.payment_type = paymentid;
                                //OrderItem products = new OrderItem
                                //{
                                //    discount = text1,
                                //    products = saleproducts,
                                //    total_price = text2,
                                //    amount_paid = amount_paid,
                                //    client_id = clienttid,
                                //    user_id = 3,
                                //    payment_type = paymentid
                                //};
                                MultipartFormDataContent content = new MultipartFormDataContent();
                                var js = JsonConvert.SerializeObject(orderItem);
                                content.Add(new StringContent(js, Encoding.UTF8, "text/json"), "products");
                                var response = await client.PostAsync("https://ittezanmobilepos.com/api/makeOrder", content);
                                if (response.IsSuccessStatusCode)
                                {

                                    var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                                    ActiveIn.IsRunning = false;
                                    var json = JsonConvert.DeserializeObject<SaleObject>(serverResponse);

                                    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                                }
                                else
                                {
                                    ActiveIn.IsRunning = false;
                                    await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                                }
                            }
                          
                            else
                            {
                            //    amount_paid = "0";
                                CrossToastPopUp.Current.ShowToastWarning(AppResources.Addsaleserror, Plugin.Toast.Abstractions.ToastLength.Long);
                                Amountpaidentry.Focus();
                            }


                        }

                    }
                    else
                    {
                        if (Amountpaidentry.Text != "" && double.Parse(Amountpaidentry.Text) >= double.Parse(text2.ToString()))
                        {
                            amount_paid = Amountpaidentry.Text;
                            OrderItem orderItem = new OrderItem();
                            orderItem.amount_paid = amount_paid;
                            orderItem.client_id = clienttid;
                            orderItem.created_at = DateTime.Now;
                            orderItem.discount = text1;
                            orderItem.products = saleproducts;
                            orderItem.total_price = text2;


                            orderItem.user_id = 3;
                            orderItem.payment_type = paymentid;
                            //OrderItem product = new OrderItem
                            //{
                            //    created_at = DateTime.Now,
                            //    discount = text1,
                            //    products = saleproducts,
                            //    total_price = text2,
                            //    amount_paid = amount_paid,
                            //    client_id = clienttid,
                            //    user_id = 3,
                            //    payment_type = paymentid

                            //};
                            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                            var db = new SQLiteConnection(dbpath);
                            var info = db.GetTableInfo("SaleObject");

                            if (!info.Any())
                            {
                                db.CreateTable<OrderItem>();

                                db.Insert(orderItem);
                            }
                            else
                            {
                                db.Insert(orderItem);
                            }
                            orderitems = (db.Table<OrderItem>().ToList());
                            if (clienttid != null)
                            {
                                var client = (db.Table<Client>().ToList().Where(clien => clien.id == int.Parse(clienttid)).FirstOrDefault());
                                if (client != null)
                                {
                                    client.paid_amount = client.paid_amount + double.Parse(amount_paid);
                                    //  client.remaining = client.remaining+ 0;
                                    client.total_amount = client.total_amount + double.Parse(text2);
                                    //  client.creditorit = client.creditorit
                                    client.updated_at = DateTime.Now.ToString();
                                    db.Update(client);
                                }
                            }
                           
                           
                            foreach (var item in Products)
                            {
                                foreach (var itemp in saleproducts)
                                {
                                    if (item.id == itemp.id)
                                    {
                                        item.stock = item.stock - int.Parse(itemp.quantity.ToString());
                                    }
                                }
                            } 
                          
                            db.UpdateAll(Products);
                            await Navigation.PushAsync(new SuccessfulReciep(orderitems, saleproducts, paymentname));
                        }
                        else
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.Addsaleserror, AppResources.Ok);

                            Amountpaidentry.Focus();
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
}