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
    public partial class CalculatorPage : PopupPage
    {
        private string percent;
        private string value;
        private bool check;
       

        public CalculatorPage()
        {
            InitializeComponent();
            check = true;
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
            Resultlbl.Text = Resultlbl.Text.Remove(Resultlbl.Text.Length - 1, 1);
        }

        private async void Next_Tapped(object sender, EventArgs e)
        {
            if (check == true)
            {
                value = Resultlbl.Text;
                percent = "";
            }
            else
            {
                value = "";
                percent = (double.Parse(Resultlbl.Text) / 100).ToString();
            }
            MessagingCenter.Send(new ValuePercent() { Value = value ,Percentage=percent}, "PopUpData");
            await Navigation.PopPopupAsync();
        }

        private void Valuebtn_Clicked(object sender, EventArgs e)
        {
            check = true;
         
        }

        private void Percentagebtn_Clicked(object sender, EventArgs e)
        {
            check = false;
          
        }

       
    }
}
