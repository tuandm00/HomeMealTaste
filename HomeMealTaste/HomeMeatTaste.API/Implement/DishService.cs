using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Implement
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public DishService(IDishRepository dishRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _context = context;
            _mapper = mapper;
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

        public async Task<PagedList<Dish>> GetAllDish(PagingParams pagingParams)
        {
            var result = await _dishRepository.GetWithPaging(pagingParams);
            
            return result;
        }
    }
}
