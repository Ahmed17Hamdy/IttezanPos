using IttezanPos.Models;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Settings = IttezanPos.Helpers.Settings;
using System.IO;
using SQLite;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Grid;

using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Color = Syncfusion.Drawing.Color;
using System.Data;
using IttezanPos.DependencyServices;
using System.Reflection;
using Syncfusion.Pdf.Tables;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuppliersPage : ContentPage
    {
        private ObservableCollection<Supplier> suppliers = new ObservableCollection<Supplier>();
        public ObservableCollection<Supplier> Suppliers
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(suppliers));
            }
        }
        public SuppliersPage()
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
        }

        public SuppliersPage(ObservableCollection<Supplier> suppliers)
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
            this.suppliers = suppliers;
            listviewwww.ItemsSource = suppliers;
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = Suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = SearchBar.Text;
            listviewwww.ItemsSource = Suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        private async void Listviewwww_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Supplier;
            await Navigation.PushAsync(new AddingSupplierPage(content));
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();

            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            PdfTemplate header = PdfHelper.AddHeader(doc, "تقرير الموردين", "Ittezan Pos" + " " + DateTime.Now.ToString());
           
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
            foreach (var item in suppliers)
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
            pdfGridRowHeader.Cells[0].Value = "رقم المورد";
            pdfGridRowHeader.Cells[1].Value = "إسم المورد";
            pdfGridRowHeader.Cells[2].Value = "عنوان المورد";
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
            pdfGrid.Draw(page,  new Syncfusion.Drawing.Point(0, (int)header.Height + 10), format1);
            MemoryStream stream = new MemoryStream();

            //Save the document.
            doc.Save(stream);
            //close the document
            doc.Close(true);
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("تقرير الموردين.pdf", "application/pdf", stream);

        }
    }
}