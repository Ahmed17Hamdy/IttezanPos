using IttezanPos.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;

namespace IttezanPos.Views.ReservoirPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReservoirPage : ContentPage
    {
        List<CurrencyInfo> currencies = new List<CurrencyInfo>();
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
                //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Syria));
                //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.UAE));
                //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.SaudiArabia));
                //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Tunisia));
                //currencies.Add(new CurrencyInfo(CurrencyInfo.Currencies.Gold));
              //  ToWord toWord = new ToWord(Convert.ToDecimal(e.NewTextValue), currencies[Convert.ToInt32(2)]);
              if(e.NewTextValue!= "" )
            {
                ToWord toWord2 = new ToWord(Convert.ToDecimal(e.NewTextValue), new CurrencyInfo(Currencies.SaudiArabia));

                switch (Settings.LastUserGravity)
                {
                    case "Arabic":
                        NumbertoTextlbl.Text = toWord2.ConvertToArabic();
                        break;
                    case "English":
                        NumbertoTextlbl.Text = toWord2.ConvertToEnglish();
                        break;
                }
            }
            else
            {                
                    NumbertoTextlbl.Text = AppResources.ZeroText ;
            } 
        }
    }
}