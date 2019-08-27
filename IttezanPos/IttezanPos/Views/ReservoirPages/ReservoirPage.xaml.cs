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
    }
}