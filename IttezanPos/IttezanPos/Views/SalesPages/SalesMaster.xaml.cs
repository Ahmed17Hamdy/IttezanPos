using IttezanPos.Helpers;
using IttezanPos.Resources;
using IttezanPos.Views.Master;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SalesMaster : MasterDetailPage
    {
        public SalesMaster()
        {
            InitializeComponent();
            
            if (Device.Idiom == TargetIdiom.Tablet)
                MasterBehavior = MasterBehavior.Popover;

            FlowDirectionPage();
            SaleNewMenu.listView.ItemSelected += OnItemSelected;
            SaleNewMenu.newlist.ItemSelected += OnItemSelected;
            SaleNewMenu.listhelp.ItemSelected += OnItemSelected;
        }
        private void FlowDirectionPage()
        {
            FlowDirection = (Settings.LastUserGravity == "Arabic") ? FlowDirection.RightToLeft
            : FlowDirection.LeftToRight;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultures(CultureTypes.NeutralCultures).ToList().
        First(element => element.EnglishName.Contains(Settings.LastUserGravity));
            AppResources.Culture = Thread.CurrentThread.CurrentUICulture;
            GravityClass.Grav();
        }
        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                SaleNewMenu.listView.SelectedItem = null;
                SaleNewMenu.newlist.SelectedItem = null;
                SaleNewMenu.listhelp.SelectedItem = null;
                SaleNewMenu.listhelp.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}