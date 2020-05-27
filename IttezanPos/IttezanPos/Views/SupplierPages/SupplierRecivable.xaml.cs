using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IttezanPos.DependencyServices;
using IttezanPos.Helpers;
using IttezanPos.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Tables;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierRecivable : ContentPage
    {
        private ObservableCollection<Supplier> suppliers;

        public SupplierRecivable()
        {
            InitializeComponent();
        }

        public SupplierRecivable(ObservableCollection<Supplier> suppliers)
        {
            InitializeComponent();
            listheaderlistv.FlowDirection = (Helpers.Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
           : FlowDirection.LeftToRight;
            this.suppliers = suppliers;
            listviewwww.ItemsSource = suppliers;
        }
        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = Searchbar.Text;
            listviewwww.ItemsSource = suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }
        void OnTextChanged(object sender, EventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            var keyword = searchBar.Text;
            listviewwww.ItemsSource = suppliers.Where(product => product.name.ToLower().Contains(keyword.ToLower()));

        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //Create a new PDF document.
            PdfDocument doc = new PdfDocument();
            //Add a page.
            PdfPage page = doc.Pages.Add();

            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            PdfTemplate header = PdfHelper.AddHeader(doc, "المبالغ المتبقية للموردين", "Ittezan Pos" + " " + DateTime.Now.ToString());

            PdfCellStyle headerStyle = new PdfCellStyle();
            headerStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Center);
            page.Graphics.DrawPdfTemplate(header, new PointF());

            //Create a PdfGrid.
            PdfGrid pdfGrid = new PdfGrid();

            //String format 

            //  PdfFont pdfFont = new PdfTrueTypeFont(fontStream1, 12);

            //Create a DataTable.
            DataTable dataTable = new DataTable("EmpDetails");
            List<SuplierTotalAmount> customerDetails = new List<SuplierTotalAmount>();
            //Add columns to the DataTable           
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Address");
            dataTable.Columns.Add("Total");

            //Add rows to the DataTable.
            foreach (var item in suppliers)
            {
                SuplierTotalAmount customer = new SuplierTotalAmount();
                customer.name = item.name;
                customer.remaining = item.remaining;
                customer.total_amount = item.total_amount;
                customer.paid_amount = item.paid_amount;
                customerDetails.Add(customer);
                dataTable.Rows.Add(new string[] { customer.remaining.ToString(), customer.paid_amount.ToString(), customer.total_amount.ToString(), customer.name });
            }

            //Assign data source.
            pdfGrid.DataSource = dataTable;

            pdfGrid.Headers.Add(1);
            PdfGridRow pdfGridRowHeader = pdfGrid.Headers[0];

            pdfGridRowHeader.Cells[3].Value = "الإسم";
            pdfGridRowHeader.Cells[2].Value = "إجمالي المبالغ";
            pdfGridRowHeader.Cells[1].Value = "المبلغ المدفوع";
            pdfGridRowHeader.Cells[0].Value = "الباقي";
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
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("المبالغ المتبقية للموردين .pdf", "application/pdf", stream);
        }
    }
}