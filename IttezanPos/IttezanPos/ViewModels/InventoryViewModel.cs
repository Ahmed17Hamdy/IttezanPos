using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
    public class InventoryViewModel
    {
        [Headers("Content-Type: application/json")]
        public interface IUpdateService
        {
            [Post("/api/updateprice")]
            Task<RootObjectUpdate> UpdatePrice(Updateprice client);
            [Post("/api/addexpense")]
            Task<ExpenseRootObject> addexpense(addexpense client);
        }
    }
}
