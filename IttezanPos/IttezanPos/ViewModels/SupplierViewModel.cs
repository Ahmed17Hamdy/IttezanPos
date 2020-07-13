using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
 public   class SupplierViewModel
    {
        [Headers("Content-Type: application/json")]
        public interface ISupplierService
        {
            [Post("/api/addsupplier")]
            Task<RootObject> AddSupplier(Supplier supplier);
            [Post("/api/addsupplier")]
            Task<AddSupplierError> AddSupplierError(Supplier supplier);


            [Post("/api/updatesupplier")]
            Task<RootObject> UpdateSupplier(Supplier supplier);

            [Post("/api/delsupplier")]
            Task<DelResponse> DeleteSupplier(int supplier_id);
            [Post("/api/offlineOrder")]
            Task<Orderoffline> AddOrder([Body] string orders);
            
        }
      
    }
}
public class Orderoffline
{
    public bool success { get; set; }
    public string data { get; set; }
    public List<int> message { get; set; }
}
