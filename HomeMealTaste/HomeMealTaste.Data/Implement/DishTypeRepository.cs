
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Implement
{
    public class DishTypeRepository : BaseRepository<DishType>, IDishTypeRepository
    {
        private readonly HomeMealTasteContext _context;
        public DishTypeRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }

        public List<DishTypeRequestModel> GetAllDishType()
        {
           var result = _context.DishTypes.Select(d => new DishTypeRequestModel
           {
               DishTypeId = d.DishTypeId,
               Name = d.Name,
               Description = d.Description,
           }).ToList();

           return result;
        }

    }
}
