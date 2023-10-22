using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Repositories
{
    public interface IDishRepository : IBaseRepository<Dish>
    {
        List<DishRequestModel> GetAllDish();
        
    }
}
