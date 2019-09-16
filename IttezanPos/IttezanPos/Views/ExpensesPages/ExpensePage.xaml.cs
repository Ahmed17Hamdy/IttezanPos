using IttezanPos.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.Helpers.CurrencyInfo;

namespace IttezanPos.Views.ExpensesPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpensePage : ContentPage
    {
        public ExpensePage()
        {
            InitializeComponent();
        }
        private void CustomEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue != "")
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
                NumbertoTextlbl.Text = AppResources.ZeroText;
            }
        }
    }
}