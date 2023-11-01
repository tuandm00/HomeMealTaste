using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;

namespace HomeMealTaste.Data.Implement;

public class MealDishRepository : BaseRepository<MealDish>, IMealDishRepository
{
    public MealDishRepository(HomeMealTasteContext context) : base(context)
    {
    }
}