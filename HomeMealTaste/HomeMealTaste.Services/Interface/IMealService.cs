using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Interface
{
    public interface IMealService
    {
        Task<MealResponseModel> CreateMeal(MealRequestModel mealRequest);
        Task<PagedList<GetAllMealResponseModel>> GetAllMeal(PagingParams pagingParams);
    }
}
