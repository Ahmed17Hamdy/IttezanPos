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
            Task<AddSupplier> AddSupplier(Supplier supplier);
            [Post("/api/addsupplier")]
            Task<AddSupplierError> AddSupplierError(Supplier supplier);


            [Post("/api/updatesupplier")]
            Task<AddSupplier> UpdateSupplier(Supplier supplier);

            [Post("/api/delsupplier")]
            Task<DelResponse> DeleteSupplier(int supplier_id);

        }
      
    }
}
