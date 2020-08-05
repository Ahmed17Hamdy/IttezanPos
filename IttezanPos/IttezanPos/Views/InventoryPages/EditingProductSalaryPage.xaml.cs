using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.ViewModels.InventoryViewModel;

namespace IttezanPos.Views.InventoryPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditingProductSalaryPage : ContentPage
    {
        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        private int up_down;
        private int purchase_sale;
        private int value_ratio;
        private int category_Id;

        public ObservableCollection<Category> Categories
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(categories));
            }
        }

        public EditingProductSalaryPage()
        {
            InitializeComponent();
            if (IttezanPos.Helpers.Settings.LastUserGravity == "Arabic")
            {
                CategoryListar.IsVisible = true;
                CategoryListen.IsVisible = false;
            }
            else
            {
                CategoryListar.IsVisible = false;
                CategoryListen.IsVisible = true;
            }
            up_down = 1;
            purchase_sale = 1;
            value_ratio = 1;
            category_Id = 0;
        }
        protected override void OnAppearing()
        {
            _ = GetData();
            base.OnAppearing();
        }
        async Task GetData()
        {
            try
            {

             
                    ActiveIn.IsRunning = true;
                   
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                Categories = new ObservableCollection<Category>(db.GetAllWithChildren<Category>().ToList());

                CategoryListar.ItemsSource = Categories;
                CategoryListar.ItemsSource = Categories;
                ActiveIn.IsRunning = false;
            }
            catch (ValidationApiException validationException)
            {
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
            catch (ApiException exception)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
            catch (Exception ex)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }

        private void increasebtn_Clicked(object sender, EventArgs e)
        {
            up_down = 1;
        }

        private void decreaebtn_Clicked(object sender, EventArgs e)
        {
            up_down = 0;
        }

        private void sellingbtn_Clicked(object sender, EventArgs e)
        {
            purchase_sale = 1;
        }

        private void Purchasingbtn_Clicked(object sender, EventArgs e)
        {
            purchase_sale = 0;
        }

        private void amountbtn_Clicked(object sender, EventArgs e)
        {
            value_ratio = 1;
        }

        private void percentbtn_Clicked(object sender, EventArgs e)
        {
            value_ratio = 0;
        }
        private void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryListar.SelectedItem as Category;
            category_Id = category.category.id;
        }
        private void CategoryListen_SelectedIndexChanged(object sender, EventArgs e)
        {
            var category = CategoryListen.SelectedItem as Category;
            category_Id = category.category.id;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
           
            try
            {
                ActiveIn.IsRunning = true;
                Updateprice client = new Updateprice
                {
                    category_id = category_Id,
                    purchase_sale = purchase_sale,
                    up_down = up_down,
                    value_ratio = value_ratio,
                    amount = double.Parse(amountentry.Text)

                };
                if (CrossConnectivity.Current.IsConnected)
                {
                   
                    var nsAPI = RestService.For<IUpdateService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.UpdatePrice(client);
                        if (data.success == true)
                        {
                            ActiveIn.IsRunning = false;
                            await DisplayAlert(AppResources.Alert, AppResources.Productsmodified, AppResources.Ok);
                            await Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveIn.IsRunning = false;
                   //     var data = await nsAPI.AddClientError(client);
                       // await Navigation.PushPopupAsync(new ClientAdded(data));
                      //  Emailentry.Focus();
                    }
                }
                else
                {
                    var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                    var db = new SQLiteConnection(dbpath);
                    var info = db.GetTableInfo("Updateprice");
                    if (!info.Any())
                      
                    db.CreateTable<Updateprice>();
                    db.Insert(client);
                    ActiveIn.IsRunning = false;
                    await DisplayAlert(AppResources.Alert, AppResources.Productsmodified, AppResources.Ok);
                    await Navigation.PopAsync();
                }
            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsRunning = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling
            }
        }
    }
}