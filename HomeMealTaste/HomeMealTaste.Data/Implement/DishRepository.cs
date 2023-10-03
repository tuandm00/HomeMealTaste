using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Implement
{
    public class DishRepository : BaseRepository<Dish>,IDishRepository
    {
        private readonly HomeMealTasteContext _context;

        public DishRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }

        public List<DishRequestModel> GetAllDish()
        {
            var result = _context.Dishes.Select(d => new DishRequestModel
            { 
                DishId = d.DishId,
                Name = d.Name,  
                Image = d.Image,
                DishTypeId = d.DishTypeId,
                KitchenId = d.KitchenId,
                MealId = d.MealId,
            }).ToList();

            return result;
        }

    }
}
