using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Data.RequestModel;

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

        public async Task<DishResponseModel> CreateDish(DishRequestModel dishRequest)
        {
            var request = new Dish()
            {
                DishId = dishRequest.DishId,
                Name = dishRequest.Name,
                Image = dishRequest.Image,
                DishTypeId = dishRequest.DishTypeId,
                KitchenId = dishRequest.KitchenId,
                MealId = dishRequest.MealId,
            };

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

            var response = new DishResponseModel()
            {
                DishId = request.DishId,
                Name = request.Name,
                Image = request.Image,
                DishTypeId = request.DishTypeId,
                KitchenId = request.KitchenId,
                MealId = request.MealId,
            };

            return response;
            
        }

        public async Task<Dish> DeleteDishId(int id)
        {
            if(id >= 0)
            {
                await _dishRepository.Delete(id, false);
            }
            return null;
        }

        public  List<DishRequestModel> GetAllDish()
        {
            var result = _dishRepository.GetAllDish();
            return result;
        }
    }
}
