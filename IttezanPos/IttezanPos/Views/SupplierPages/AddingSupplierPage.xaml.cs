using IttezanPos.Models;
using IttezanPos.Models.OfflineModel;
using IttezanPos.Resources;
using IttezanPos.Services;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static IttezanPos.ViewModels.SupplierViewModel;

namespace IttezanPos.Views.SupplierPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingSupplierPage : ContentPage
    {
        private Supplier content;
        private string edite;

        public ObservableCollection<Supplier> Suppliers { get; private set; }

        public AddingSupplierPage()
        {
            InitializeComponent();
        }

        public AddingSupplierPage(Supplier content)
        {
            InitializeComponent();
            this.content = content;
            SupplierNameArentry.Text = content.name;
            SupplierEnnamenentry.Text = content.enname;
            SupplierAddressentry.Text = content.address;
            SupplierEmailentry.Text = content.email;
            SupplierPhoneentry.Text = content.phone;
            SupplierNotesentry.Text = content.note;          
            Detailsstk.IsVisible = true;
            savrbtn.IsVisible = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Supplier supplier = new Supplier
                    {
                        name = SupplierNameArentry.Text,
                        enname = SupplierEnnamenentry.Text,
                        address = SupplierAddressentry.Text,
                        email = SupplierEmailentry.Text,
                        phone = SupplierPhoneentry.Text,
                        note = SupplierNotesentry.Text,
                    };
                    var nsAPI = RestService.For<ISupplierService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.AddSupplier(supplier);
                        if (data.success == true)
                        {
                            ActiveIn.IsVisible = false;
                            await Navigation.PushPopupAsync(new SupplierAddedPopup());
                        }
                    }
                    catch
                    {
                        ActiveIn.IsVisible = false;
                        var data = await nsAPI.AddSupplierError(supplier);
                        await Navigation.PushPopupAsync(new SupplierAddedPopup(data.data.email.First()));
                        SupplierEmailentry.Focus();
                    }
                    
                }
                else
                {
                    
                   
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                            db.CreateTable<Supplier>();
                    db.CreateTable<AddSupplierOffline>();
                    Supplier client = new Supplier
                        {
                            name = SupplierNameArentry.Text,
                            enname = SupplierEnnamenentry.Text,
                            address = SupplierAddressentry.Text,
                            email = SupplierEmailentry.Text,
                            phone = SupplierPhoneentry.Text,
                            note = SupplierNotesentry.Text,
                            created_at = DateTime.Now.ToString()

                        };
                    Suppliers = new ObservableCollection<Supplier>(db.Table<Supplier>().ToList());
                 var   udatedclient = Suppliers. Where(i => i.email == client.email).ToList();
                    //           var udatedclient = db.Table<Supplier>().Where(i => i.email == client.email).First();


                    if (udatedclient .Count== 0)
                        {
                            ActiveIn.IsVisible = false;
                        AddSupplierOffline supplierOffline = new AddSupplierOffline
                        {
                            name = SupplierNameArentry.Text,
                            enname = SupplierEnnamenentry.Text,
                            address = SupplierAddressentry.Text,
                            email = SupplierEmailentry.Text,
                            phone = SupplierPhoneentry.Text,
                            note = SupplierNotesentry.Text,
                            created_at = DateTime.Now.ToString()

                        };
                        db.Insert(supplierOffline);
                        db.Insert(client);
                        await Navigation.PushPopupAsync(new SupplierAddedPopup());
                            await Navigation.PopAsync();
                           
                       
                    }
                        else
                        {
                            ActiveIn.IsVisible = false;

                            await Navigation.PushPopupAsync(new SupplierAddedPopup("قيمة البريد الالكتروني مُستخدمة من قبل"));
                            SupplierEmailentry.Focus();
                        }

                    
                }
            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling 
            }
            catch (Exception ex)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }

        private async void Update_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Supplier supplier = new Supplier
                    {
                        id = content.id,
                        name = SupplierNameArentry.Text,
                        enname = SupplierEnnamenentry.Text,
                        address = SupplierAddressentry.Text,
                        email = SupplierEmailentry.Text,
                        phone = SupplierPhoneentry.Text,
                        note = SupplierNotesentry.Text,
                    };
                    var nsAPI = RestService.For<ISupplierService>("https://ittezanmobilepos.com");
                   
                        var data = await nsAPI.UpdateSupplier(supplier);
                        if (data.success == true)
                        {
                            ActiveIn.IsVisible = false;
                            await Navigation.PushPopupAsync(new SupplierAddedPopup(edite));
                            await Navigation.PopAsync();
                    }                
                }
                else
                {
                   
                    
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                            db.CreateTable<Supplier>();
                    db.CreateTable<UpdateSupplierOffline>();
                    Supplier client = new Supplier
                        {
                            id = content.id,
                            name = SupplierNameArentry.Text,
                            enname = SupplierEnnamenentry.Text,
                            address = SupplierAddressentry.Text,
                            email = SupplierEmailentry.Text,
                            phone = SupplierPhoneentry.Text,
                            note = SupplierNotesentry.Text,
                            updated_at = DateTime.Now.ToString()

                        };
                        var udatedclient = db.Table<Supplier>().Where(i => i.email == client.email).First();
                    UpdateSupplierOffline supplierOffline = new UpdateSupplierOffline
                    {
                        supplier_id= content.supplier_id,
                        name = SupplierNameArentry.Text,
                        enname = SupplierEnnamenentry.Text,
                        address = SupplierAddressentry.Text,
                        email = SupplierEmailentry.Text,
                        phone = SupplierPhoneentry.Text,
                        note = SupplierNotesentry.Text,
                       
                    };
                    db.Insert(supplierOffline);
                    db.Update(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new SupplierAddedPopup(edite));
                        await Navigation.PopAsync();
                   

                }
            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling 
            }
            catch (Exception ex)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }

        private async void Deletebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                   
                    var nsAPI = RestService.For<ISupplierService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.DeleteSupplier(content.id);
                    if (data.success == true)
                     {

                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new SupplierAddedPopup(content.id));
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    
                    
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Supplier");
                        if (!info.Any())
                            db.CreateTable<Supplier>();
                    db.CreateTable<DeleteSupplierOffline>();
                    Supplier client = new Supplier
                        {
                            id = content.id,
                            name = SupplierNameArentry.Text,
                            enname = SupplierEnnamenentry.Text,
                            address = SupplierAddressentry.Text,
                            email = SupplierEmailentry.Text,
                            phone = SupplierPhoneentry.Text,
                            note = SupplierNotesentry.Text,

                        };
                    //       var udatedclient = db.Table<Client>().Where(i => i.email == client.email).First();
                    DeleteSupplierOffline supplierOffline = new DeleteSupplierOffline
                    {
                        name = SupplierNameArentry.Text,
                        enname = SupplierEnnamenentry.Text,
                        address = SupplierAddressentry.Text,
                        email = SupplierEmailentry.Text,
                        phone = SupplierPhoneentry.Text,
                        note = SupplierNotesentry.Text,
                        created_at = DateTime.Now.ToString(),
                        updated_at = DateTime.Now.ToString(),
                    };
                    db.Insert(supplierOffline);
                    db.Delete(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new SupplierAddedPopup(content.id));
                        await Navigation.PopAsync();
                    
                }
            }
            catch (ValidationApiException validationException)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // handle validation here by using validationException.Content, 
                // which is type of ProblemDetails according to RFC 7807
            }
            catch (ApiException exception)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
                // other exception handling 
            }
            catch (Exception ex)
            {
                ActiveIn.IsVisible = false;
                await DisplayAlert(IttezanPos.Resources.AppResources.Alert, AppResources.ConnectionNotAvailable, AppResources.Ok);
            }
        }
    }
}