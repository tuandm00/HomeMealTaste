using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IKitchenService
    {
        Task<List<KitchenResponseModel>> GetAllKitchen();
        Task<KitchenResponseModel> GetAllKitchenByKitchenId(int id);
    }
}
