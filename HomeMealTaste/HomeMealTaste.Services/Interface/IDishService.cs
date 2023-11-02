using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Interface
{
    public interface IDishService
    {
        Task<DishResponseModel> CreateDishAsync(DishRequestModel dish);
        Task<DishResponseModel> GetDetailAsync(int id);
        Task<PagedList<Dish>> GetAllDishAsync(PagingParams pagingParams);
        Task DeleteAsync(int id);
    }
}
