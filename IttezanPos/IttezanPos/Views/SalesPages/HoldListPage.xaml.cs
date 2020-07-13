using IttezanPos.Helpers;
using IttezanPos.Models;
using IttezanPos.Resources;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HoldListPage : ContentPage
    {
      
        private ObservableCollection<HoldProduct> holdList = new ObservableCollection<HoldProduct>();
        public ObservableCollection<HoldProduct> HoldList
        {
            get { return holdList; }
            set
            {
                holdList = value;
                OnPropertyChanged(nameof(holdList));
            }
        }


        public HoldListPage()
        {
            InitializeComponent();
            FlowDirectionPage();
            GetData();
            
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
        private void GetData()
        {
            var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
            var db = new SQLiteConnection(dbpath);
            var info = db.GetTableInfo("HoldProduct");
            if (!info.Any())
            {
                db.CreateTable<HoldProduct>();
            }
            HoldList = new ObservableCollection<HoldProduct>(db.GetAllWithChildren<HoldProduct>().ToList());
      
            listhold.ItemsSource = HoldList;
        }

        private async void Edite_Tapped(object sender, EventArgs e)
        {
            if (((sender as Frame).BindingContext is HoldProduct saleproduct))
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                var db = new SQLiteConnection(dbpath);
                var info = db.GetTableInfo("HoldProduct");


             var sale =  db.GetAllWithChildren<HoldProduct>().ToList().Where(clien => 
             clien.id == saleproduct.id).FirstOrDefault();
                db.Delete(sale);
                MessagingCenter.Send(new SaleHold() { product = saleproduct }, "SaleHold");
                await Navigation.PopAsync();
            }
        }

        private void Delete_Tapped(object sender, EventArgs e)
        {
            if (((sender as Frame).BindingContext is HoldProduct saleproduct))
            {
                var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                var db = new SQLiteConnection(dbpath);
                var info = db.GetTableInfo("HoldProduct");


                var sale = db.GetAllWithChildren<HoldProduct>().ToList().Where(clien =>
               clien.id == saleproduct.id).FirstOrDefault();
                db.Delete(sale);
                HoldList = new ObservableCollection<HoldProduct>(db.GetAllWithChildren<HoldProduct>().ToList());
                listhold.ItemsSource = HoldList;
            }
        }
    }
}