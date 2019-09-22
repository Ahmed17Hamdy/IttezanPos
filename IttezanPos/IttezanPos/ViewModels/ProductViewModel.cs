using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
  public  class ProductViewModel
    {
        [Headers("Content-Type: application/json")]
        public interface IProductService
        {
            [Multipart]
            [Post("/api/addproduct")]
            Task<AddProduct> AddProduct(int id, string name, string user_id, int category_id,
                string locale, int purchase_price , int sale_price, int stock, string description,
                [AliasAs("image")] StreamPart stream);
            [Post("/api/addproduct")]
            Task<AddProduct> AddProductError(Product product);



            [Post("/api/updateproduct")]
            Task<AddProduct> UpdateProduct(Product product);

            [Post("/api/delproduct")]
            Task<DelResponse> DeleteClient(int product_id);
        }
    }
}
