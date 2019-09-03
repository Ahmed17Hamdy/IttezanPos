using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPopUpPage : PopupPage
    {
        public AddCategoryPopUpPage()
        {
            InitializeComponent();
        }

        private void RedColor_Tapped(object sender, EventArgs e)
        {
            RedColorlbl.Text = "✔";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void BlueColor_Tapped(object sender, EventArgs e)
        {
            Bluelbl.Text = "✔";
            RedColorlbl.Text = "";
          
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void VoiletColor_Tapped(object sender, EventArgs e)
        {
            Voiletlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
           
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void IndigoColor_Tapped(object sender, EventArgs e)
        {
            IndigoColor.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void GreenColor_Tapped(object sender, EventArgs e)
        {
            GreenColor.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
          
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void YellowColor_Tapped(object sender, EventArgs e)
        {
            Yellowlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";          
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void OrangeColor_Tapped(object sender, EventArgs e)
        {
            Orangelbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
           
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void CyanColor_Tapped(object sender, EventArgs e)
        {
            Cyanlbl.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
         
            Brownlbl.Text = "";
            DarkBlue.Text = "";
        }

        private void BrownColor_Tapped(object sender, EventArgs e)
        {
            Brownlbl.Text= "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
           
            DarkBlue.Text = "";
        }

        private void DarkBlueColor_Tapped(object sender, EventArgs e)
        {
            DarkBlue.Text = "✔";
            RedColorlbl.Text = "";
            Bluelbl.Text = "";
            Voiletlbl.Text = "";
            IndigoColor.Text = "";
            GreenColor.Text = "";
            Yellowlbl.Text = "";
            Orangelbl.Text = "";
            Cyanlbl.Text = "";
            Brownlbl.Text = "";
           
        }

        private async void ClosePage_Tapped(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
        }
     
     

       
    }
}