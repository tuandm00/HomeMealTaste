using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.ResponseModel;


namespace HomeMealTaste.Services.Interface
{
    public interface IDishTypeService
    {
        Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel requestModel);
        Task<PagedList<DishType>> GetAllDishType(PagingParams pagingParams);
        Task DeleteDishTypeById(int id);
        
    }
}
