using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
  public class SalesViewModel
    {
        [Headers("Content-Type: application/json")]
        public interface ISalesService
        {
            [Post("/api/makeOrder")]
            Task<SaleObject> AddSale(OrderItem item);
          //  [Post("/api/addclient")]
         //   Task<AddClientError> AddClientError(Client client);



            [Post("/api/updateclient")]
            Task<RootObject> UpdateClient(Client client);

            [Post("/api/delclient")]
            Task<DelResponse> DeleteClient(int client_id);
        }
    }
}
