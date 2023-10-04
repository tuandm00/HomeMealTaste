using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.ResponseModel;


namespace HomeMealTaste.Services.Interface
{
    public interface IDishTypeService
    {
        Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel requestModel);
        List<DishTypeRequestModel> GetAllDishType();

        Task DeleteDishTypeById(int id);
    }
}
