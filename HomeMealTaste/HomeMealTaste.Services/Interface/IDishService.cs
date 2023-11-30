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
        Task DeleteSingleDishById(int dishid);
        Task DeleteDishInMealDish(int dishid, int mealid);
        Task<UpdateDishResponseModel> UpdateDishNotExistInMealSession(UpdateDishRequestModel request);
        Task<List<GetDishIdByMealIdResponseModel>> GetDishIdByMealId(int mealid);
        Task<List<GetDishByKitchenIdResponseModel>> GetDishByKitchenId(int kitchenid);
    }
}
