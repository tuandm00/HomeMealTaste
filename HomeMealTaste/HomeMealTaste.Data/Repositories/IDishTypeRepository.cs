using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Repositories
{
    public interface IDishTypeRepository : IBaseRepository<DishType>
    {
        List<DishTypeRequestModel> GetAllDishType();
    }
}
