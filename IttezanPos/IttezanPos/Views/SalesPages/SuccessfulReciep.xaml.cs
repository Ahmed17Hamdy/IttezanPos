using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IttezanPos.DependencyServices;
using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Views.Master;
using IttezanPos.Views.PurchasingPages;
using SQLite;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;
using Syncfusion.Pdf.Interactive;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Color = Syncfusion.Drawing.Color;
using Font = Syncfusion.Drawing.Font;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SuccessfulReciep : ContentPage
    {
      
        private OrderItem products;
        private Purchaseitem products1;
      
        private string paymentname;


        public SuccessfulReciep(OrderItem products, string paymentname)
        {
            InitializeComponent();
            FlowDirectionPage();
            this.products = products;
          
            this.paymentname = paymentname;
            Totallbl.Text = products.total_price;
            Salestk.IsVisible = true;
            Purchasestk.IsVisible = false;
        }

        public SuccessfulReciep(Purchaseitem products1)
        {
            InitializeComponent();
            FlowDirectionPage();
            this.products1 = products1;
            Totallbl.Text = products1.total_price;
            Salestk.IsVisible = false;
            Purchasestk.IsVisible = true;
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
        private   void Button_Clicked(object sender, EventArgs e)
        {
            //  var mas=    App.Current.MainPage as MasterDetailPage;
            //  Navigation.();
            //  Application.Current.MainPage = new NavigationPage((Page)Activator.CreateInstance(master.TargetType));
             Application.Current.MainPage = new NavigationPage(new SalesMaster());
            //  await Navigation.PushModalAsync(new NavigationPage(new MainSalesPage()));
           // await ((App.Current.MainPage as MasterDetailPage).Detail as NavigationPage).Navigation.PushAsync(new MainSalesPage());
        }

        private async void Button_Clicked_1(object sender, EventArgs e)
        {
           await Navigation.PushModalAsync( new NavigationPage( new PurchasePage()));
        }

        private async void ViewReciept_Clicked(object sender, EventArgs e)
        {
            //  await Navigation.PushAsync(new NavigationPage(new RecieptPage(products,saleproducts,  paymentname)) );
            #region Fields
            //Create border color
            PdfColor borderColor = new PdfColor(Color.FromArgb(255, 51, 181, 75));
            PdfBrush lightGreenBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 218, 218, 221)));

            PdfBrush darkGreenBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 51, 181, 75)));

            PdfBrush whiteBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 255, 255, 255)));
            PdfPen borderPen = new PdfPen(borderColor, 1f);
            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            //Create TrueType font
            PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Bold);
            PdfTrueTypeFont arialRegularFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Regular);
            PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 11, PdfFontStyle.Bold);


            const float margin = 30;
            const float lineSpace = 7;
            const float headerHeight = 90;
            #endregion

            #region header and buyer infomation
            //Create PDF with PDF/A-3b conformance
            PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A3B);
            //Set ZUGFeRD profile
            document.ZugferdConformanceLevel = ZugferdConformanceLevel.Basic;

            //Add page to the PDF
            PdfPage page = document.Pages.Add();

            PdfGraphics graphics = page.Graphics;

            //Get the page width and height
            float pageWidth = page.GetClientSize().Width;
            float pageHeight = page.GetClientSize().Height;
            //Draw page border
            graphics.DrawRectangle(borderPen, new RectangleF(0, 0, pageWidth, pageHeight));

            //Fill the header with light Brush 
            graphics.DrawRectangle(lightGreenBrush, new RectangleF(0, 0, pageWidth, headerHeight));

            RectangleF headerAmountBounds = new RectangleF(400, 0, pageWidth - 400, headerHeight);

            graphics.DrawString("INVOICE", headerFont, whiteBrush, new PointF(margin, headerAmountBounds.Height / 3));
            Stream imageStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.ic_launcher.png");

            //Create a new PdfBitmap instance
            PdfBitmap image = new PdfBitmap(imageStream);

            //Draw the image    
            graphics.DrawImage(image, new PointF(margin +90, headerAmountBounds.Height / 2));
            graphics.DrawRectangle(darkGreenBrush, headerAmountBounds);
            
            graphics.DrawString("Amount", arialRegularFont, whiteBrush, headerAmountBounds,
                new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));
            graphics.DrawString("$" + products.amount_paid.ToString(), arialBoldFont, whiteBrush,
                new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
                PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));

         

            PdfTextElement textElement = new PdfTextElement("Invoice Number: " + products.id.ToString(), arialRegularFont);

            PdfLayoutResult layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - margin, 120));

            textElement.Text = "Date : " + DateTime.Now.ToString("dddd dd, MMMM yyyy");
            textElement.Draw(page, new PointF(layoutResult.Bounds.X, layoutResult.Bounds.Bottom + lineSpace));
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            if (products.client_id != null)
            {
                var client = (db.Table<Client>().ToList().Where(clien => clien.id == int.Parse(products.client_id)).FirstOrDefault());
                textElement.Text = "Bill To:";
                layoutResult = textElement.Draw(page, new PointF(margin, 120));

                textElement.Text = client.enname;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.address;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.email;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.phone;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            }
           
            #endregion

            #region Invoice data
            PdfGrid grid = new PdfGrid();
            DataTable dataTable = new DataTable("EmpDetails");
            List<Product> customerDetails = new List<Product>();
            //Add columns to the DataTable           
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Qty");
            dataTable.Columns.Add("Disc");
            dataTable.Columns.Add("Total");

            //Add rows to the DataTable.
            foreach (var item in products.products)
            {
                Product customer = new Product();
                customer.id = products.products.IndexOf(item) + 1;
                customer.Enname = item.Enname;
                customer.sale_price = item.sale_price;
                customer.quantity = item.quantity;
                customer.discount = item.discount;
                customer.total_price = item.total_price;
                customerDetails.Add(customer);
                dataTable.Rows.Add(new string[] { customer.id.ToString(), customer.Enname, customer.sale_price.ToString(),
                customer.quantity.ToString(), customer.discount.ToString(), customer.total_price.ToString()});
            }

            //Assign data source.
            grid.DataSource = dataTable;
          

            grid.Columns[1].Width = 150;
            grid.Style.Font = arialRegularFont;
            grid.Style.CellPadding.All = 5;

            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable1Light);

            layoutResult = grid.Draw(page, new PointF(0, layoutResult.Bounds.Bottom + 40));

            textElement.Text = "Grand Total: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom +
                lineSpace));

            float totalAmount = float.Parse(products.total_price);
            textElement.Text = "$" + totalAmount.ToString();
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            //graphics.DrawString("$" + totalAmount.ToString(), arialBoldFont, whiteBrush, 
            //    new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
            //    PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));
            textElement.Text = "Total Discount: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom +
                lineSpace));

            float totalDisc = float.Parse(products.discount);
            textElement.Text = "$" + totalDisc.ToString();
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            //graphics.DrawString("$" + totalDisc.ToString(), arialBoldFont, whiteBrush,
            //    new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
            //    PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));


            #endregion

            #region Seller information
            borderPen.DashStyle = PdfDashStyle.Custom;
            borderPen.DashPattern = new float[] { 3, 3 };

            PdfLine line = new PdfLine(borderPen, new PointF(0, 0), new PointF(pageWidth, 0));
            layoutResult = line.Draw(page, new PointF(0, pageHeight - 100));

            textElement.Text = "IttezanPos";
            textElement.Font = arialRegularFont;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + (lineSpace * 3)));
            textElement.Text = "Buradah,  AlQassim, Saudi Arabia";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = "Any Questions? ittezan.com";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            
            #endregion

            //#region Create ZUGFeRD XML

            ////Create //ZUGFeRD Invoice
            //ZugferdInvoice invoice = new ZugferdInvoice("2058557939", DateTime.Now, CurrencyCodes.USD);

            ////Set ZUGFeRD profile to basic
            //invoice.Profile = ZugferdProfile.Basic;

            ////Add buyer details
            //invoice.Buyer = new UserDetails
            //{
            //    ID = "Abraham_12",
            //    Name = "Abraham Swearegin",
            //    ContactName = "Swearegin",
            //    City = "United States, California",
            //    Postcode = "9920",
            //    Country = CountryCodes.US,
            //    Street = "9920 BridgePointe Parkway"
            //};

            ////Add seller details
            //invoice.Seller = new UserDetails
            //{
            //    ID = "Adventure_123",
            //    Name = "AdventureWorks",
            //    ContactName = "Adventure support",
            //    City = "Austin,TX",
            //    Postcode = "78721",
            //    Country = CountryCodes.US,
            //    Street = "800 Interchange Blvd"
            //};


            //IEnumerable<Product> products = saleproducts;

            //foreach (Product product in products)
            //    invoice.AddProduct(product);


            //invoice.TotalAmount = totalAmount;

            //MemoryStream zugferdXML = new MemoryStream();
            //invoice.Save(zugferdXML);
            //#endregion

            #region Embed ZUGFeRD XML to PDF

            //Attach ZUGFeRD XML to PDF
            //PdfAttachment attachment = new PdfAttachment("ZUGFeRD-invoice.xml", zugferdXML);
            //attachment.Relationship = PdfAttachmentRelationship.Alternative;
            //attachment.ModificationDate = DateTime.Now;
            //attachment.Description = "ZUGFeRD-invoice";
            //attachment.MimeType = "application/xml";
            //document.Attachments.Add(attachment);
            #endregion
            //Creates an attachment 
            MemoryStream stream = new MemoryStream();
        //    Stream invoiceStream = typeof(MainPage).GetTypeInfo().Assembly.GetManifestResourceStream("Sample.Assets.Data.ZUGFeRD-invoice.xml");

            PdfAttachment attachment = new PdfAttachment("ZUGFeRD-invoice.xml", stream);

            attachment.Relationship = PdfAttachmentRelationship.Alternative;

            attachment.ModificationDate = DateTime.Now;

            attachment.Description = "ZUGFeRD-invoice";

            attachment.MimeType = "application/xml";

            document.Attachments.Add(attachment);

            //Save the document into memory stream

           

            document.Save(stream);

            //Close the document

            document.Close(true);
          
            await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("تقرير العملاء.pdf", "application/pdf", stream);
        }

        private async void Print_Clicked(object sender, EventArgs e)
        {
            #region Fields
            //Create border color
            PdfColor borderColor = new PdfColor(Color.FromArgb(255, 51, 181, 75));
            PdfBrush lightGreenBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 218, 218, 221)));

            PdfBrush darkGreenBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 51, 181, 75)));

            PdfBrush whiteBrush = new PdfSolidBrush(new PdfColor(Color.FromArgb(255, 255, 255, 255)));
            PdfPen borderPen = new PdfPen(borderColor, 1f);
            Stream fontStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.arial.ttf");
            //Create TrueType font
            PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Bold);
            PdfTrueTypeFont arialRegularFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Regular);
            PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 11, PdfFontStyle.Bold);


            const float margin = 30;
            const float lineSpace = 7;
            const float headerHeight = 90;
            #endregion

            #region header and buyer infomation
            //Create PDF with PDF/A-3b conformance
            PdfDocument document = new PdfDocument(PdfConformanceLevel.Pdf_A3B);
            //Set ZUGFeRD profile
            document.ZugferdConformanceLevel = ZugferdConformanceLevel.Basic;

            //Add page to the PDF
            PdfPage page = document.Pages.Add();

            PdfGraphics graphics = page.Graphics;

            //Get the page width and height
            float pageWidth = page.GetClientSize().Width;
            float pageHeight = page.GetClientSize().Height;
            //Draw page border
            graphics.DrawRectangle(borderPen, new RectangleF(0, 0, pageWidth, pageHeight));

            //Fill the header with light Brush 
            graphics.DrawRectangle(lightGreenBrush, new RectangleF(0, 0, pageWidth, headerHeight));

            RectangleF headerAmountBounds = new RectangleF(400, 0, pageWidth - 400, headerHeight);

            graphics.DrawString("INVOICE", headerFont, whiteBrush, new PointF(margin, headerAmountBounds.Height / 3));
            Stream imageStream = typeof(App).GetTypeInfo().Assembly.GetManifestResourceStream("IttezanPos.Assets.ic_launcher.png");

            //Create a new PdfBitmap instance
            PdfBitmap image = new PdfBitmap(imageStream);

            //Draw the image    
            graphics.DrawImage(image, new PointF(margin + 90, headerAmountBounds.Height / 2));
            graphics.DrawRectangle(darkGreenBrush, headerAmountBounds);

            graphics.DrawString("Amount", arialRegularFont, whiteBrush, headerAmountBounds,
                new PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));
            graphics.DrawString("$" + products.amount_paid.ToString(), arialBoldFont, whiteBrush,
                new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
                PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));



            PdfTextElement textElement = new PdfTextElement("Invoice Number: " + products.id.ToString(), arialRegularFont);

            PdfLayoutResult layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - margin, 120));

            textElement.Text = "Date : " + DateTime.Now.ToString("dddd dd, MMMM yyyy");
            textElement.Draw(page, new PointF(layoutResult.Bounds.X, layoutResult.Bounds.Bottom + lineSpace));
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            if (products.client_id != null)
            {
                var client = (db.Table<Client>().ToList().Where(clien => clien.id == int.Parse(products.client_id)).FirstOrDefault());
                textElement.Text = "Bill To:";
                layoutResult = textElement.Draw(page, new PointF(margin, 120));

                textElement.Text = client.enname;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.address;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.email;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
                textElement.Text = client.phone;
                layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            }

            #endregion

            #region Invoice data
            PdfGrid grid = new PdfGrid();
            DataTable dataTable = new DataTable("EmpDetails");
            List<Product> customerDetails = new List<Product>();
            //Add columns to the DataTable           
            dataTable.Columns.Add("ID");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Price");
            dataTable.Columns.Add("Qty");
            dataTable.Columns.Add("Disc");
            dataTable.Columns.Add("Total");

            //Add rows to the DataTable.
            foreach (var item in products.products)
            {
                Product customer = new Product();
                customer.id = products.products.IndexOf(item) + 1;
                customer.Enname = item.Enname;
                customer.sale_price = item.sale_price;
                customer.quantity = item.quantity;
                customer.discount = item.discount;
                customer.total_price = item.total_price;
                customerDetails.Add(customer);
                dataTable.Rows.Add(new string[] { customer.id.ToString(), customer.Enname, customer.sale_price.ToString(),
                customer.quantity.ToString(), customer.discount.ToString(), customer.total_price.ToString()});
            }

            //Assign data source.
            grid.DataSource = dataTable;


            grid.Columns[1].Width = 150;
            grid.Style.Font = arialRegularFont;
            grid.Style.CellPadding.All = 5;

            grid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable1Light);

            layoutResult = grid.Draw(page, new PointF(0, layoutResult.Bounds.Bottom + 40));

            textElement.Text = "Grand Total: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom +
                lineSpace));

            float totalAmount = float.Parse(products.total_price);
            textElement.Text = "$" + totalAmount.ToString();
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            //graphics.DrawString("$" + totalAmount.ToString(), arialBoldFont, whiteBrush, 
            //    new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
            //    PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));
            textElement.Text = "Total Discount: ";
            textElement.Font = arialBoldFont;
            layoutResult = textElement.Draw(page, new PointF(headerAmountBounds.X - 40, layoutResult.Bounds.Bottom +
                lineSpace));

            float totalDisc = float.Parse(products.discount);
            textElement.Text = "$" + totalDisc.ToString();
            layoutResult = textElement.Draw(page, new PointF(layoutResult.Bounds.Right + 4, layoutResult.Bounds.Y));

            //graphics.DrawString("$" + totalDisc.ToString(), arialBoldFont, whiteBrush,
            //    new RectangleF(400, lineSpace, pageWidth - 400, headerHeight + 15), new
            //    PdfStringFormat(PdfTextAlignment.Center, PdfVerticalAlignment.Middle));


            #endregion

            #region Seller information
            borderPen.DashStyle = PdfDashStyle.Custom;
            borderPen.DashPattern = new float[] { 3, 3 };

            PdfLine line = new PdfLine(borderPen, new PointF(0, 0), new PointF(pageWidth, 0));
            layoutResult = line.Draw(page, new PointF(0, pageHeight - 100));

            textElement.Text = "IttezanPos";
            textElement.Font = arialRegularFont;
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + (lineSpace * 3)));
            textElement.Text = "Buradah,  AlQassim, Saudi Arabia";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));
            textElement.Text = "Any Questions? ittezan.com";
            layoutResult = textElement.Draw(page, new PointF(margin, layoutResult.Bounds.Bottom + lineSpace));

            #endregion

            //#region Create ZUGFeRD XML

            ////Create //ZUGFeRD Invoice
            //ZugferdInvoice invoice = new ZugferdInvoice("2058557939", DateTime.Now, CurrencyCodes.USD);

            ////Set ZUGFeRD profile to basic
            //invoice.Profile = ZugferdProfile.Basic;

            ////Add buyer details
            //invoice.Buyer = new UserDetails
            //{
            //    ID = "Abraham_12",
            //    Name = "Abraham Swearegin",
            //    ContactName = "Swearegin",
            //    City = "United States, California",
            //    Postcode = "9920",
            //    Country = CountryCodes.US,
            //    Street = "9920 BridgePointe Parkway"
            //};

            ////Add seller details
            //invoice.Seller = new UserDetails
            //{
            //    ID = "Adventure_123",
            //    Name = "AdventureWorks",
            //    ContactName = "Adventure support",
            //    City = "Austin,TX",
            //    Postcode = "78721",
            //    Country = CountryCodes.US,
            //    Street = "800 Interchange Blvd"
            //};


            //IEnumerable<Product> products = saleproducts;

            //foreach (Product product in products)
            //    invoice.AddProduct(product);


            //invoice.TotalAmount = totalAmount;

            //MemoryStream zugferdXML = new MemoryStream();
            //invoice.Save(zugferdXML);
            //#endregion

            #region Embed ZUGFeRD XML to PDF

            //Attach ZUGFeRD XML to PDF
            //PdfAttachment attachment = new PdfAttachment("ZUGFeRD-invoice.xml", zugferdXML);
            //attachment.Relationship = PdfAttachmentRelationship.Alternative;
            //attachment.ModificationDate = DateTime.Now;
            //attachment.Description = "ZUGFeRD-invoice";
            //attachment.MimeType = "application/xml";
            //document.Attachments.Add(attachment);
            #endregion
            //Creates an attachment 
            MemoryStream stream = new MemoryStream();
            //    Stream invoiceStream = typeof(MainPage).GetTypeInfo().Assembly.GetManifestResourceStream("Sample.Assets.Data.ZUGFeRD-invoice.xml");

            PdfAttachment attachment = new PdfAttachment("ZUGFeRD-invoice.xml", stream);

            attachment.Relationship = PdfAttachmentRelationship.Alternative;

            attachment.ModificationDate = DateTime.Now;

            attachment.Description = "ZUGFeRD-invoice";

            attachment.MimeType = "application/xml";

            document.Attachments.Add(attachment);

            //Save the document into memory stream



            document.Save(stream);

            //Close the document

            document.Close(true);
             DependencyService.Get<IPrintService>().Print(stream, "تقرير العملاء.pdf");
        //    await Xamarin.Forms.DependencyService.Get<ISave>().SaveAndView("تقرير العملاء.pdf", "application/pdf", stream);
        }
    }
}