using IttezanPos.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.ReservoirPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservoirPage : ContentPage
    {
        public ReservoirPage()
        {
            InitializeComponent();
        }

        private  void Addrdbtn_Checked(object sender, EventArgs e)
        {
            Deductionbtn.IsVisible = false;
            Addingbtn.IsVisible = true;          
        }

        private void Deductrdbtn_Checked(object sender, EventArgs e)
        {
            Addingbtn.IsVisible = false;
            Deductionbtn.IsVisible = true;
        }

        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
           
            if (e.NewTextValue != "" && e.NewTextValue.ToCharArray().All(x => char.IsDigit(x)))
            {
                switch (Settings.LastUserGravity)
                {
                    case "Arabic":
                        NumbertoTextlbl.Text = ConvertNumberToText.ConvertToArabic(Convert.ToDouble(e.NewTextValue)) + " " + AppResources.SR;
                        break;
                    case "English":
                        NumbertoTextlbl.Text = NumbersToEnglishText.ConvertNumberAsText(Convert.ToInt32(e.NewTextValue)) + " " + AppResources.SR;
                        break;
                }

            }
            else
            {
                NumbertoTextlbl.Text = AppResources.ZeroText + " "+ AppResources.SR;
            }
        }
    }
}