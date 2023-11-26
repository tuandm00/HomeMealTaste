using System.Linq.Expressions;
using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Implement
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        private readonly IBlobService _blobService;


        public DishService(IDishRepository dishRepository, HomeMealTasteContext context, IMapper mapper, IBlobService blobService)
        {
            _dishRepository = dishRepository;
            _context = context;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task<DishResponseModel> CreateDishAsync(DishRequestModel dishRequest)
        {
            var entity = _mapper.Map<Dish>(dishRequest);

            var imagePath = await _blobService.UploadQuestImgAndReturnImgPathAsync(dishRequest.Image, "dish-image");
            entity.Image = imagePath;

            var result = await _dishRepository.Create(entity, true);

            return _mapper.Map<DishResponseModel>(result);
        }

        public async Task<DishResponseModel> GetDetailAsync(int id)
        {
            var dish = await _dishRepository.GetFirstOrDefault(x => x.DishId == id) ?? throw new Exception($"No dish found with id {id}");

            return _mapper.Map<DishResponseModel>(dish);
        }

        public async Task DeleteAsync(int id)
        {
            _ = await _dishRepository.GetFirstOrDefault(x => x.DishId == id) ??
                throw new Exception($"No dish found with id {id}");

            await _dishRepository.Delete(id);
        }

        public async Task<PagedList<Dish>> GetAllDishAsync(PagingParams pagingParams)
        {
            var result = await _dishRepository.GetWithPaging(pagingParams);

            return result;
        }

        public Task<List<GetDishIdByMealIdResponseModel>> GetDishIdByMealId(int mealid)
        {
            var result = _context.MealDishes.Where(x => x.MealId == mealid).Select(x => new GetDishIdByMealIdResponseModel
            {
                MealId = x.MealId,
                Dish = new Dish
                {
                    DishId = x.Dish.DishId,
                    Name = x.Dish.Name,
                    Image = x.Dish.Image,
                    DishTypeId = x.Dish.DishTypeId,
                    KitchenId = x.Dish.KitchenId
                }
            });

            var mapped = result.Select(r => _mapper.Map<GetDishIdByMealIdResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }

        public Task<List<GetDishByKitchenIdResponseModel>> GetDishByKitchenId(int kitchenid)
        {
            var result = _context.Dishes.Where(x => x.KitchenId == kitchenid).Select(x => new GetDishByKitchenIdResponseModel
            {
                DishId = x.DishId,
                Name = x.Name,
                Image = x.Image,
                DishTypeResponse = new DishTypeResponse
                {
                    DishTypeId = x.DishType.DishTypeId,
                    Name = x.DishType.Name,
                    Description = x.DishType.Description
                },
                KitchenId = kitchenid,
            });

            var mapped = result.Select(r => _mapper.Map<GetDishByKitchenIdResponseModel>(r)).ToList();
            return Task.FromResult(mapped);
        }
    }
}
