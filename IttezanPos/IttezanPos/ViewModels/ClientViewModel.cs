using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
  public  class ClientViewModel
    {
        [Headers("Content-Type: application/json")]
        public interface IClientService
        {
            [Post("/api/addclient")]
            Task<AddClient> AddClient(Client client);
            [Post("/api/addclient")]
            Task<AddClientError> AddClientError(Client client);



            [Post("/api/updateclient")]
            Task<AddClient> UpdateClient(Client client);

            [Post("/api/delclient")]
            Task<DelResponse> DeleteClient(int client_id);
        }
    }
}
