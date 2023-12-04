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
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;

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
        //public FormFile ConvertStringToFormFile(string imageString)
        //{
        //    // Assuming 'imageString' is a Base64 encoded image string
        //    byte[] imageData = Convert.FromBase64String(imageString);

        //    // Create a MemoryStream from the byte array
        //    using MemoryStream imageStream = new MemoryStream(imageData);

        //    // Create a FormFile instance
        //    var imageFile = new FormFile(imageStream, 0, imageStream.Length, "image", "image.jpg")
        //    {
        //        Headers = new HeaderDictionary(),
        //        ContentType = "image/jpeg" // Adjust content type based on your image format
        //    };

        //    return imageFile;
        //}
        public async Task<DishResponseModel> CreateDishAsync(DishRequestModel dishRequest)
        {

            var entity = _mapper.Map<Dish>(dishRequest);
            var imagePath = await _blobService.UploadQuestImgAndReturnImgPathAsync(dishRequest.Image,"dish-image");


            entity.Image = imagePath.ToString();

            var result = await _dishRepository.Create(entity, true);

            return _mapper.Map<DishResponseModel>(result);
        }

        public async Task<DishResponseModel> GetDetailAsync(int id)
        {
            var dish = await _dishRepository.GetFirstOrDefault(x => x.DishId == id) ?? throw new Exception($"No dish found with id {id}");

            return _mapper.Map<DishResponseModel>(dish);
        }

        public async Task DeleteDishNotExistInSessionByDishId(int dishid)
        {
            var getDishById = _context.Dishes.Where(x => x.DishId == dishid).FirstOrDefault();
            if(getDishById == null)
            {
                throw new Exception("No dish found");
            }
            else
            {
                var checkDishExist = _context.MealDishes.Where(x => x.DishId == getDishById.DishId).FirstOrDefault();
                if (checkDishExist != null)
                {
                    throw new Exception("Cannot Delete");
                }
                else _context.Dishes.Remove(getDishById);
                _context.SaveChanges();
            }
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

        public async Task<UpdateDishResponseModel> UpdateDishNotExistInMealSession(UpdateDishRequestModel request)
        {
            var entity = _mapper.Map<Dish>(request);
            var getDishById = _context.Dishes.Where(x => x.DishId == request.DishId).FirstOrDefault();
            if (getDishById == null)
            {
                throw new Exception("No dish found");
            }
            else
            {
                var listmealid = _context.MealDishes.Where(x => x.DishId == getDishById.DishId).Select(x => x.MealId).ToList();
                UpdateDishResponseModel mapped = null;
                foreach (var mealid in listmealid)
                {
                    var mealExist = _context.MealSessions.Where(x => x.MealId == mealid).FirstOrDefault();
                    if (mealExist != null)
                    {
                        throw new Exception("Cannot Update");
                    }
                    else
                    {
                        var imagePath = await _blobService.UploadQuestImgAndReturnImgPathAsync(request.Image, "dish-image");

                        getDishById.DishId = entity.DishId;
                        getDishById.Name = entity.Name;
                        getDishById.DishTypeId = entity.DishTypeId;
                        getDishById.KitchenId = entity.KitchenId;
                        getDishById.Image = imagePath;

                        await _dishRepository.Update(getDishById);
                        _context.SaveChanges();
                        mapped = _mapper.Map<UpdateDishResponseModel>(getDishById);
                    }
                }
                return mapped;

            }
        }

        public async Task DeleteDishInMealDish(int dishid, int mealid)
        {
            var mealdishId = _context.MealDishes.Where(x=> x.DishId == dishid && x.MealId == mealid).FirstOrDefault();
            if(mealdishId != null)
            {
                _context.MealDishes.Remove(mealdishId);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Cannot Delete");
            }

        }
    }
}
