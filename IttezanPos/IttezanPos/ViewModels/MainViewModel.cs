using IttezanPos.Models;
using IttezanPos.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace IttezanPos.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Client> clients;

        public ObservableCollection<Client> Clients
        {
            get { return clients; }
            set
            {
                clients = value;
            }
        }

        public ICommand getDataCommand {get;set;}
       public MainViewModel()
        {
            //   getDataCommand = new Command(async () => await GetData());
         
        }
        public event PropertyChangedEventHandler PropertyChanged;
       
      
    }
}
