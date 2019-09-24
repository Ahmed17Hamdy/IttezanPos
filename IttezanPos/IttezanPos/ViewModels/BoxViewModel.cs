using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.ViewModels
{
 public   class BoxViewModel
    {

        [Headers("Content-Type: application/json")]
        public interface IBoxService
        {
            
            [Post("/api/intrprocessTreasury")]
            Task<RootObject> Addinterproccess(Box box);
        }
    }
}
