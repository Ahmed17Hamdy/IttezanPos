using IttezanPos.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
                NumbertoTextlbl.Text = AppResources.ZeroText + " " + AppResources.SR;
            }
        }
    }
}