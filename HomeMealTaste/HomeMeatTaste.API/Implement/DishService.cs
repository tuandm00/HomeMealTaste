using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly HomeMealTasteContext _context;

        public DishService(IDishRepository dishRepository, HomeMealTasteContext context)
        {
            _dishRepository = dishRepository;
            _context = context;
        }

        public async Task<Dish> CreateDish(Dish dish)
        {
            var result = new Dish()
            {
                DishId = dish.DishId,
                Name = dish.Name,
                Image = dish.Image,
                DishTypeId = dish.DishTypeId,
                ChefId = dish.ChefId,
                FoodPackageId = dish.FoodPackageId,
            };

            await _context.AddAsync(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}
