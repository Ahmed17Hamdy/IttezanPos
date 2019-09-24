﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SupplierAddedPopup : PopupPage
    {
        private string edite;
        private int id;

        public SupplierAddedPopup(Models.AddSupplierError data)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            if (data.data.email.First()== "قيمة البريد الالكتروني مُستخدمة من قبل")
            {
                Supplierlbl.Text = AppResources.UsedEmail;
            }
        }
        public SupplierAddedPopup()
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
        }

        public SupplierAddedPopup(string edite)
        {
            InitializeComponent();
            Frame.BorderColor = Color.FromHex("#33b54b");
            Supplierlbl.Text = AppResources.UpdatedSupplier;
            this.edite = edite;
        }

        public SupplierAddedPopup(int id)
        {
            InitializeComponent();
            Frame.BorderColor = Color.Red;
            Supplierlbl.Text = AppResources.DeletedSupplier;
            this.id = id;
        }
    }
}