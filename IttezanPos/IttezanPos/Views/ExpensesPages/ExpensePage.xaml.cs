using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Models.OfflineModel;
using IttezanPos.Resources;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Refit;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;
using static IttezanPos.ViewModels.InventoryViewModel;

namespace IttezanPos.Views.ExpensesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensePage : ContentPage
    {
        public List<addexpense> Expenses { get; private set; }
        public Box box { get; private set; }

        public ExpensePage()
        {
            InitializeComponent();
            GetExpenses();
        }

        private async void GetExpenses()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("addexpense");
            if (!info.Any())
                db.CreateTable<addexpense>();
            Expenses = (db.Table<addexpense>().ToList());
            if (Expenses.Count!=0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();

                     
                        var jsoncategoryArray = JsonConvert.SerializeObject(Expenses);
                        var values = new Dictionary<string, string>
                        {
                            {"expenses",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-add-expense")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);
                       
                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                               ActiveIn.IsRunning = false;
                            var json = JsonConvert.DeserializeObject<OfflineExpense>(serverResponse);
                            Expenses.Clear();
                            db.DropTable<addexpense>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                             ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
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
               
            //    var jsoncategoryArray = JsonConvert.SerializeObject(Expenses);
                if (CrossConnectivity.Current.IsConnected)
                {
                    addexpense expense = new addexpense
                    {
                        date = DatePicker.Date.ToShortDateString(),
                        statement = Statmententry.Text,
                        user_id = 3,
                        amount = double.Parse(amountentry.Text)
                    };
                    var nsAPI = RestService.For<IUpdateService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.addexpense(expense);
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
                        await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        //     var data = await nsAPI.AddClientError(client);
                        // await Navigation.PushPopupAsync(new ClientAdded(data));
                        //  Emailentry.Focus();
                    }
                }
                else
                {

                    addexpense expense = new addexpense
                    {
                        date = DatePicker.Date.ToShortDateString(),
                        statement = Statmententry.Text,
                        user_id = 3,
                        amount = double.Parse(amountentry.Text)
                    };
                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                    var db = new SQLiteConnection(dbpath);
                    var info = db.GetTableInfo("addexpense");
                    if (!info.Any())
                        db.CreateTable<addexpense>();
                    db.Insert(expense);
                    await DisplayAlert(AppResources.Alert, AppResources.ExpnseAdded, AppResources.Ok);
                    var info2 = db.GetTableInfo("Box");
                    if (!info.Any())
                        db.CreateTable<Box>();
                  
                    ActiveIn.IsRunning = false;
                    var boxs = (db.Table<Box>().ToList());
                   
                  
                    if (boxs.Count!=0)
                    {
                        box = db.Get<Box>(0);
                        var disc_expenses = Preferences.Get("disc_expenses", 0);
                        if (disc_expenses == 1)
                        {
                            box.balance = (Convert.ToDouble(box.balance) - expense.amount).ToString("0.00");

                        }
                        db.Update(box);
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