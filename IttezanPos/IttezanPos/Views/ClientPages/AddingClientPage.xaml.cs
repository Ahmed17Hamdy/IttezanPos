using IttezanPos.Models;
using IttezanPos.Resources;
using IttezanPos.Services;
using IttezanPos.Views.InventoryPages.InventoryPopups;
using Plugin.Connectivity;
using Refit;
using Rg.Plugins.Popup.Extensions;
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
using static IttezanPos.ViewModels.ClientViewModel;

namespace IttezanPos.Views.ClientPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddingClientPage : ContentPage
    {
        private Client content;
        private string edite;
        private int limit;

        public ObservableCollection<Client> Clients { get; private set; }

        public AddingClientPage()
        {
            InitializeComponent();
        }

        public AddingClientPage(Client content)
        {
            InitializeComponent();
            this.content = content;
            ClientNameArentry.Text = content.name;
            ClientNameArentry.Text = content.enname;
            ClientAddressentry.Text = content.address;
            Emailentry.Text = content.email;
            phoneentry.Text = content.phone;
            Notesentry.Text = content.note;
            Limitentry.Text = content.limitt.ToString();
            Detailsstk.IsVisible = true;
            Savebtn.IsVisible = false;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (Limitentry.Text == "")
            {
                limit = 0;
            }
            else
            {
                limit = int.Parse(Limitentry.Text);
            }
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Client client = new Client
                    {                        
                       name = ClientNameArentry.Text,
                        enname = ClientEnnamenentry.Text,
                        address =ClientAddressentry.Text,
                       email=Emailentry.Text,
                       phone=phoneentry.Text,
                       note=Notesentry.Text,
                       limitt= limit
                    };
                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");
                    try
                    {
                        var data = await nsAPI.AddClient(client);
                        if (data.success == true)
                        {
                            ActiveIn.IsVisible = false;                         
                            await Navigation.PushPopupAsync(new ClientAdded());
                            await Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        ActiveIn.IsVisible = false;
                        var data = await nsAPI.AddClientError(client);
                        await Navigation.PushPopupAsync(new ClientAdded(data.data.email.First()));
                        Emailentry.Focus();
                    }
                }
                else
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit,
                            created_at= DateTime.Now.ToString()
                            
                        };

                        var udatedclient = db.Table<Client>().Where(i => i.email == client.email).First();


                        if (udatedclient == null)
                        {
                            ActiveIn.IsVisible = false;
                            await Navigation.PushPopupAsync(new ClientAdded());
                            await Navigation.PopAsync();
                            db.Insert(client);
                        }
                        else
                        {
                            ActiveIn.IsVisible = false;

                            await Navigation.PushPopupAsync(new ClientAdded("قيمة البريد الالكتروني مُستخدمة من قبل"));
                            Emailentry.Focus();
                        }
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit,
                            created_at = DateTime.Now.ToString()
                        };
                        var udatedclient = db.Table<Client>().Where(i => i.email == client.email).First();
                       
                       
                            if (udatedclient == null)
                            {
                                ActiveIn.IsVisible = false;
                                await Navigation.PushPopupAsync(new ClientAdded());
                                await Navigation.PopAsync();
                                db.Insert(client);
                            }
                            else
                            {
                                ActiveIn.IsVisible = false;

                                await Navigation.PushPopupAsync(new ClientAdded("قيمة البريد الالكتروني مُستخدمة من قبل"));
                                Emailentry.Focus();
                            }
                       
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
        }
        private async void Update_Clicked(object sender, EventArgs e)
        {
            if (Limitentry.Text == "")
            {
                limit = 0;
            }
            else
            {
                limit = int.Parse(Limitentry.Text);
            }
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {
                    Client client = new Client
                    {
                        client_id = content.id,
                        name = ClientNameArentry.Text,
                        enname = ClientEnnamenentry.Text,
                        address = ClientAddressentry.Text,
                        email = Emailentry.Text,
                        phone = phoneentry.Text,
                        note = Notesentry.Text,
                        id = content.id,
                        limitt= limit,

                    };
                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.UpdateClient(client);
                    if (data.success == true)
                    {
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(edite));
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            id = content.id,
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit,
                            updated_at = DateTime.Now.ToString()
                        };
                        var udatedclient = db.Table<Client>().Where(i => i.id == client.id).First();
                        db.Update(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(edite));
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            id=content.id,
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit,
                            updated_at = DateTime.Now.ToString()
                        };
                       var udatedclient= db.Table<Client>().Where(i => i.id == client.id).First();
                        db.Update(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(edite));
                        await Navigation.PopAsync();
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
        private async void Deletebtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                ActiveIn.IsVisible = true;
                if (CrossConnectivity.Current.IsConnected)
                {

                    var nsAPI = RestService.For<IClientService>("https://ittezanmobilepos.com");

                    var data = await nsAPI.DeleteClient(content.id);
                    if (data.success == true)
                    {

                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(content.id));
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    if (Device.RuntimePlatform == Device.iOS)
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            id = content.id,
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit
                        };
                        var udatedclient = db.Table<Client>().Where(i => i.id == client.id).First();
                        db.Update(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(content.id));
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        var dbpath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "MyDb.db");
                        var db = new SQLiteConnection(dbpath);
                        var info = db.GetTableInfo("Client");
                        if (!info.Any())
                            db.CreateTable<Client>();
                        Client client = new Client
                        {
                            id = content.id,
                            name = ClientNameArentry.Text,
                            enname = ClientEnnamenentry.Text,
                            address = ClientAddressentry.Text,
                            email = Emailentry.Text,
                            phone = phoneentry.Text,
                            note = Notesentry.Text,
                            limitt = limit
                        };
                        var udatedclient = db.Table<Client>().Where(i => i.id == client.id).First();
                        db.Delete(client);
                        ActiveIn.IsVisible = false;
                        await Navigation.PushPopupAsync(new ClientAdded(content.id));
                        await Navigation.PopAsync();
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
    }
}