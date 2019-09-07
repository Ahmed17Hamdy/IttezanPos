using Fusillade;
using IttezanPos.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IttezanPos.Services
{
    [Headers("Content-Type: application/json")]
    public interface IApiService
    {
      [Get("/api/settinginfo")]
        Task<RootObject> GetSettings();
    }
}
