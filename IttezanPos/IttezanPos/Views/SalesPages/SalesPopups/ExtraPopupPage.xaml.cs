using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Models.OfflineModel;
using IttezanPos.Resources;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages.SalesPopups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExtraPopupPage : PopupPage
    {
        private List<OrderItem> orderitems;
         List<string> rooo = new List<string>();
        private List<SaleProductoff> sales = new List<SaleProductoff>();
        private List<OfflineSalesOrder> OrdersOffline = new List<OfflineSalesOrder>();

        public ExtraPopupPage()
        {
            InitializeComponent();
            FlowDirectionPage();
        }
        private void FlowDirectionPage()
        {


            FlowDirection = (Helpers.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
         : FlowDirection.LeftToRight;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().
         First(element => element.EnglishName.Contains(Helpers.Settings.LastUserGravity));
            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            GravityClass.Grav();
        }
        private async void ViewHold_Tapped(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync((new HoldListPage()));
            await PopupNavigation.Instance.PopAllAsync();

        }

        private async void ViewPastReciepts_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync((new SalePreviousReciepts()));
            await PopupNavigation.Instance.PopAllAsync();
        }

        private async void sync_Tapped(object sender, EventArgs e)
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("OrderItem");
            if (!info.Any())
            {
                db.CreateTable<OrderItem>();
                db.CreateTable<SaleProduct>();
              
            }
            orderitems = (db.GetAllWithChildren<OrderItem>().ToList());
            if (orderitems.Count!=0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {


                        
                        //    var products = orderitems.ToArray();
                           var content = new MultipartFormDataContent();
                        foreach (var item in orderitems)
                        {
                            foreach (var item1 in item.products)
                            {
                                SaleProductoff sale = new SaleProductoff();

                                sale.id = item1.product_id;
                                //   sale.expiration_date = item1.expiration_date;
                                sale.expiration_date = DateTime.Now;
                                sale.purchase_price = item1.purchase_price;
                                sale.quantity = item1.quantity;
                                sale.sale_price = item1.sale_price;
                                sale.total_price = item1.total_price;
                               
                                sales.Add(sale);
                              
                            }
                            OfflineSalesOrder product = new OfflineSalesOrder
                            {

                                discount = double.Parse(item.discount),
                                products = sales,
                                total_price = double.Parse(item.total_price),
                                amount_paid = double.Parse(item.amount_paid),
                                client_id = item.client_id,
                                user_id = "3",
                                payment_type = item.payment_type
                            };
                            OrdersOffline.Add(product);
                        }
                        //foreach (var item in OrdersOffline)
                     //   {
                     //       var json = JsonConvert.SerializeObject(item);
                     //       rooo.Add(json);
                     //   }
                     //var f = rooo.ToArray();

                               var jsoncategoryArray = JsonConvert.SerializeObject(OrdersOffline);
                        var values = new Dictionary<string, string>
                        {
                            {"orders",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/offlineOrder")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);
                  //      content.Add(new StringContent(jsoncategoryArray, Encoding.UTF8, "text/json"), "orders");
                        //   var content = new StringContent(JsonConvert.SerializeObject(values), Encoding.UTF8, "text/json");
                  //    var response = await client.PostAsync("https://ittezanmobilepos.com/api/offlineOrder", content);
                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                         //   ActiveIn.IsRunning = false;
                            var json = JsonConvert.DeserializeObject<SaleObject>(serverResponse);

                        //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                         //   ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
    }
}}