using IttezanPos.Models;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SalesPages.SalesPopups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Quantitypop : PopupPage
    {
        private Product selectedprp;
        private Double Quantity;

        public Quantitypop(Product selectedprp)
        {
            InitializeComponent();
            this.selectedprp = selectedprp;
        }

       

        private async void Next_Tapped(object sender, EventArgs e)
        {
            if (Resultlbl.Text != "")
            {
                Quantity = double.Parse(Resultlbl.Text);
           
                MessagingCenter.Send(new ValueQuantity() { Quantity = Quantity, product = selectedprp }, "PopUpData1");
                await Navigation.PopPopupAsync();
            }
        }
        private async void Closelbl_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }

        private void One_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "1";
        }
        private void two_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "2";
        }
        private void three_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "3";
        }
        private void four_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "4";
        }
        private void five_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "5";
        }
        private void six_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "6";
        }
        private void seven_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "7";
        }
        private void eight_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "8";
        }
        private void nine_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "9";
        }
        private void Zero_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + "0";
        }
        private void dot_Tapped(object sender, EventArgs e)
        {
            Resultlbl.Text = Resultlbl.Text + ".";
        }

        private void Clear_Tapped(object sender, EventArgs e)
        {
            if (Resultlbl.Text.Length != 0)
            {
                Resultlbl.Text = Resultlbl.Text.Remove(Resultlbl.Text.Length - 1, 1);
            }
        }
    }
}