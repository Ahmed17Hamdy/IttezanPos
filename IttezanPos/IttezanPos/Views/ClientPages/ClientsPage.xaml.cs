
using IttezanPos.Models;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Tables;
using Syncfusion.Drawing;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using System.Linq;
using System.IO;
using Settings = IttezanPos.Helpers.Settings;
using IttezanPos.DependencyServices;
using System.Collections.Generic;
using System.Reflection;
using Syncfusion.Pdf.Grid;
using System.Data;
using IttezanPos.Helpers;

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

        }

        public ClientsPage(ObservableCollection<Client> clients)
        {
            InitializeComponent();
            this.Clients = clients;
            ArabicListView.ItemsSource  = Clients;
            Englishlistview.ItemsSource = Clients;
            if (IttezanPos.Helpers.Settings.LastUserGravity == "Arabic")
            {
               // listheaderlistv.FlowDirection = FlowDirection.RightToLeft;
                ArabicListView.IsVisible = true;
                Englishlistview.IsVisible = false;
            }
            else
            {
                //   listheaderlistv.FlowDirection = FlowDirection.LeftToRight;
                ArabicListView.IsVisible = false;
                Englishlistview.IsVisible = true;
            }
           
        }

       
       

        private async void Listviewwww_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Client;
            await Navigation.PushAsync(new AddingClientPage(content));
        }
        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
           Englishlistview.ItemsSource= 
                clients.Where(product => product.enname.ToLower().Contains(keyword.ToLower()));
            ArabicListView.ItemsSource =
                clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            Englishlistview.ItemsSource  = clients.Where(product => product.enname.ToLower().Contains(keyword.ToLower()));
            ArabicListView.ItemsSource =
               clients.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }

        private async void Reprtbtn_Clicked(object sender, EventArgs e)
        
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();

            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            PdfTemplate header = PdfHelper.AddHeader(doc, "تقرير العملاء", "Ittezan Pos" + " " + DateTime.Now.ToString());

            PdfCellStyle headerStyle = new PdfCellStyle();
            headerStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            page.Graphics.DrawPdfTemplate(header, new PointF());

            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //String format 

            //  PdfFont pdfFont = new PdfTrueTypeFont(fontStream1, 12);

            //Create a DataTable.
            DataTable dataTable = new DataTable("EmpDetails");
            List<Customer> customerDetails = new List<Customer>();
            //Add columns to the DataTable           
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Address");

            //Add rows to the DataTable.
            foreach (var item in Clients)
            {
                Customer customer = new Customer();
                customer.ID = item.id;
                customer.Name = item.name;
                customer.Address = item.address;
                customerDetails.Add(customer);
                dataTable.Rows.Add(new string[] { customer.ID.ToString(), customer.Name, customer.Address });
            }

            //Assign data source.
            pdfGrid.DataSource = dataTable;
            pdfGrid.Headers.Add(1);
            PdfGridRow pdfGridRowHeader = pdfGrid.Headers[0];
            pdfGridRowHeader.Cells[0].Value = "رقم العميل";
            pdfGridRowHeader.Cells[1].Value = "إسم العميل";
            pdfGridRowHeader.Cells[2].Value = "عنوان العميل";
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
            pdfGrid.Style = pdfGridStyle;
            //Draw grid to the page of PDF document.
            pdfGrid.Draw(page, new Syncfusion.Drawing.Point(0, (int)header.Height + 10), format1);
            MemoryStream stream = new MemoryStream();

            //Save the document.
            doc.Save(stream);
            //close the document
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("تقرير العملاء.pdf", "application/pdf", stream);

        }

       
    }
}

         
   
    
