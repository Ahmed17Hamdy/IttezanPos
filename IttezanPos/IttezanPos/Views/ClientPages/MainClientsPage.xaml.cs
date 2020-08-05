using IttezanPos.DependencyServices;
using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Models.OfflineModel;
using IttezanPos.Resources;
using IttezanPos.Services;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Refit;
using SQLite;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamEffects;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainClientsPage : ContentPage
    {
        private ObservableCollection<Client> clients = new ObservableCollection<Client>();
        private List<int> client_ids= new List<int>();

        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
                OnPropertyChanged(nameof(clients));
            }
        }

        public ObservableCollection<AddClientOffline> AddedClients { get; private set; }
        public ObservableCollection<UpdateClientOffline> UpdatedClients { get; private set; }
        public ObservableCollection<DeleteClientOffline> DeletedClients { get; private set; }

        public MainClientsPage()
        {
            InitializeComponent();
            _ = GetOffline();
            _ = GetOfflineUpdate();
            _ = GetOfflineDelete();
            _ = GetData();
            Commandss();
          
        }
        async Task GetOfflineUpdate()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("UpdateClientOffline");
            if (!info.Any())
                db.CreateTable<UpdateClientOffline>();

            UpdatedClients = new ObservableCollection<UpdateClientOffline>(db.Table<UpdateClientOffline>().ToList());
            
            if (UpdatedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();


                        var jsoncategoryArray = JsonConvert.SerializeObject(UpdatedClients);
                        var values = new Dictionary<string, string>
                        {
                            {"clients",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-updateclient")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                           var json = JsonConvert.DeserializeObject<RootObject>(serverResponse);

                            UpdatedClients.Clear();
                            Clients = new ObservableCollection<Client>( json.message.clients);
                            db.DropTable<UpdateClientOffline>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        async Task GetOfflineDelete()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("DeleteClientOffline");
            if (!info.Any())
                db.CreateTable<DeleteClientOffline>();

            DeletedClients = new ObservableCollection<DeleteClientOffline>(db.Table<DeleteClientOffline>().ToList());
            if (DeletedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();
                        foreach (var item in DeletedClients)
                        {
                            client_ids.Add(item.client_id);
                        }

                        var jsoncategoryArray = JsonConvert.SerializeObject(client_ids);
                        var values = new Dictionary<string, string>
                        {
                            {"client_ids",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-delclient")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                            // var json = JsonConvert.DeserializeObject<OfflineClientAdded>(serverResponse);

                            DeletedClients.Clear();
                            db.DropTable<DeleteClientOffline>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        async Task GetOffline()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("AddClientOffline");
            if (!info.Any())
                db.CreateTable<AddClientOffline>();

            AddedClients = new ObservableCollection<AddClientOffline>(db.Table<AddClientOffline>().ToList());
            if (AddedClients.Count != 0)
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    using (var client = new HttpClient())
                    {
                        //    var products = orderitems.ToArray();
                        var content = new MultipartFormDataContent();


                        var jsoncategoryArray = JsonConvert.SerializeObject(AddedClients);
                        var values = new Dictionary<string, string>
                        {
                            {"clients",jsoncategoryArray }
                        };

                        var req = new HttpRequestMessage(HttpMethod.Post, "https://ittezanmobilepos.com/api/off-addclient")
                        { Content = new FormUrlEncodedContent(values) };
                        var response = await client.SendAsync(req);

                        if (response.IsSuccessStatusCode)
                        {
                            var serverResponse = response.Content.ReadAsStringAsync().Result.ToString();
                            ActiveIn.IsVisible = false;
                    //             var json = JsonConvert.DeserializeObject<OfflineClientAdded>(serverResponse);

                            AddedClients.Clear();
                            db.DropTable<AddClientOffline>();
                            //    await Navigation.PushAsync(new SuccessfulReciep(json.message, saleproducts, paymentname));

                        }
                        else
                        {
                            ActiveIn.IsVisible = false;
                            await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                        }

                    }

                }
            }
        }
        async Task GetData()
        {
            try
            {
               ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    var nsAPI = RestService.For<IApiService>("https://ittezanmobilepos.com/");
                    RootObject data = await nsAPI.GetSettings();
                    Clients = new ObservableCollection<Client>(data.message.clients);
                    foreach (var item in Clients)
                    {
                        item.client_id = item.id;
                    }
                   
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                        {
                            db.CreateTable<Client>();
                        }
                        else
                        {
                            db.DropTable<Client>();
                            db.CreateTable<Client>();
                        }
                        //    Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                        // db.CreateTable<Client>();
                        db.InsertAll(Clients);
                   
            //        listviewwww.ItemsSource = Clients;
                    ActiveIn.IsVisible = false;
                }
                else
                {
                //    ActiveIn.IsRunning = false;
                    
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Clients = new ObservableCollection<Client>(db.Table<Client>().ToList());
                    }
                    ActiveIn.IsVisible = false;
                    //     listviewwww.ItemsSource = Clients;
                    //  await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            
            }

            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {

                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }


        }
        private void Commandss()
        {
            Commands.SetTap(AddClientGrid, new Command(() => {
                AddingClientPageNav();
            }));
            Commands.SetTap(ViewClientGrid, new Command(() => {
                ClientsPageNav();
            }));
            Commands.SetTap(CustomeropeningbalancesGrid, new Command(() => {
                OpeningbalancesPageNav();
            }));
            Commands.SetTap(ViewCustomerreceivablesGrid, new Command(() => {
                ClientRecievableNav();
            }));
            Commands.SetTap(CustomerreceivablesGrid, new Command(() => {
                CustomerreceivablesGridRe();
            }));
        }

        private async void CustomerreceivablesGridRe()
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();

            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            PdfTemplate header = PdfHelper.AddHeader(doc, "تقرير ذمم العملاء", "Ittezan Pos" + " " + DateTime.Now.ToString());

            PdfCellStyle headerStyle = new PdfCellStyle();
            headerStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            page.Graphics.DrawPdfTemplate(header, new PointF());

            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //String format 

            //  PdfFont pdfFont = new PdfTrueTypeFont(fontStream1, 12);

            //Create a DataTable.
            DataTable dataTable = new DataTable("EmpDetails");
            List<Client> customerDetails = new List<Client>();
            //Add columns to the DataTable           
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Address");
            dataTable.Columns.Add("Total");

            //Add rows to the DataTable.
            foreach (var item in Clients)
            {
                Client customer = new Client();
                customer.name = item.name;
                customer.remaining = item.remaining;
                customer.creditorit = item.creditorit;
                customer.total_amount = item.total_amount;
                customerDetails.Add(customer);
                dataTable.Rows.Add(new string[] { customer.total_amount.ToString(), customer.remaining.ToString(), customer.creditorit.ToString(), customer.name });
            }

            //Assign data source.
            pdfGrid.DataSource = dataTable;

            pdfGrid.Headers.Add(1);
            PdfGridRow pdfGridRowHeader = pdfGrid.Headers[0];

            pdfGridRowHeader.Cells[3].Value = "الإسم";
            pdfGridRowHeader.Cells[2].Value = "الباقي من فواتير الآجل";
            pdfGridRowHeader.Cells[1].Value = "الباقي من الرصيد الإفتتاحي";
            pdfGridRowHeader.Cells[0].Value = "الإجمالي";
            PdfGridStyle pdfGridStyle = new PdfGridStyle();
            pdfGridStyle.Font = new PdfTrueTypeFont(fontStream, 12);

            PdfGridLayoutFormat format1 = new PdfGridLayoutFormat();
            format1.Break = PdfLayoutBreakType.FitPage;
            format1.Layout = PdfLayoutType.Paginate;

            PdfStringFormat format = new PdfStringFormat();

            format.TextDirection = PdfTextDirection.RightToLeft;
            format.Alignment = PdfTextAlignment.Center;
            format.LineAlignment = PdfVerticalAlignment.Middle;

            pdfGrid.Columns[0].Format = format;
            pdfGrid.Columns[1].Format = format;
            pdfGrid.Columns[2].Format = format;
            pdfGrid.Columns[3].Format = format;
            pdfGrid.Style = pdfGridStyle;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.Point(0, (int)header.Height + 10), format1);
            MemoryStream stream = new MemoryStream();

            //Save the document.
            doc.Save(stream);
            //close the document
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("تقرير ذمم الموردين .pdf", "application/pdf", stream);
        }

        private async void AddingClientPageNav()
        {
            await Navigation.PushAsync(new AddingClientPage());
        }
        private async void ClientsPageNav()
        {
            if ( Clients.Count!=0)
            {
                await Navigation.PushAsync(new ClientsPage(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new ClientsPage(Clients));
            }
        }
        private async void OpeningbalancesPageNav()
        {
            if ( Clients.Count != 0)
            {
                await Navigation.PushAsync(new OpeningbalancesPage(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new OpeningbalancesPage(Clients));
            }
        }
        private async void ClientRecievableNav()
        {
            if ( Clients.Count != 0)
            {
                await Navigation.PushAsync(new ClientRecievable(Clients));
            }
            else
            {
                await GetData();
                await Navigation.PushAsync(new ClientRecievable(Clients));
            }
           


        }


    }
}