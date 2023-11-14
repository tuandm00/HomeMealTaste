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
    public interface IOrderService
    {
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id);
        Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id);
        Task<List<OrderResponseModel>> GetAllOrder();
        Task<List<GetOrderByKitchenIdResponseModel>> GetOrderByKitchenId(int kitchenid);

    }
}
