using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.ResponseModel;


namespace HomeMealTaste.Services.Interface
{
    public interface IDishTypeService
    {
        Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel requestModel);
        Task<List<DishTypeResponseModel>> GetAllDishType();
        Task DeleteDishTypeById(int id);
        Task<UpdateDishTypeResponseModel> UpdateDishType(UpdateDishTypeRequestModel request);
        Task<DishTypeResponseModel> GetSingleDishTypeById(int dishtypeId);
        
    }
}
