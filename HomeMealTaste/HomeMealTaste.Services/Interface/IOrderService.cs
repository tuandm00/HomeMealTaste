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
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByUserId(int id);
        Task<List<OrderResponseModel>> GetAllOrder();
    }
}
