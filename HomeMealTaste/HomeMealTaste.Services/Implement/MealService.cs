using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;

namespace HomeMealTaste.Services.Implement
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;
        private readonly IBlobService _blobService;

        public MealService(IMealRepository mealRepository, IMapper mapper, HomeMealTasteContext context, IBlobService blobService)
        {
            _mealRepository = mealRepository;
            _mapper = mapper;
            _context = context;
            _blobService = blobService;
        }
        public static DateTime TranferDateTimeByTimeZone(DateTime dateTime, string timezoneArea)
        {

            ReadOnlyCollection<TimeZoneInfo> collection = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = collection.ToList().Where(x => x.DisplayName.ToLower().Contains(timezoneArea)).First();

            var timeZoneLocal = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneLocal, timeZone);

            return utcDateTime;
        }

        public static DateTime GetDateTimeTimeZoneVietNam()
        {

            return TranferDateTimeByTimeZone(DateTime.Now, "hanoi");
        }
        public static DateTime? StringToDateTimeVN(string dateStr)
        {

            var isValid = System.DateTime.TryParseExact(
                                dateStr,
                                "d'/'M'/'yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date
                            );
            return isValid ? date : null;
        }
        public async Task<MealResponseModel> CreateMeal(MealRequestModel mealRequest)
        {
            var entity = _mapper.Map<Meal>(mealRequest);
            
            var imagePath = await _blobService.UploadQuestImgAndReturnImgPathAsync(mealRequest.Image, "meal-image");
            entity.Image = imagePath;
            entity.CreateDate = GetDateTimeTimeZoneVietNam();
            
            var result = await _mealRepository.Create(entity, true);

            if(result != null)
            {
                var mealdish = new MealDish
                {
                    MealId = result.MealId,
                    DishId = mealRequest.DishId
                };
                await _context.AddAsync(mealdish);
                await _context.SaveChangesAsync();
            }
             var response = _mapper.Map<MealResponseModel>(result);
            //response.CreateDate = result.CreateDate?.ToString("yyyy-MM-dd HH:mm:ss");
            response.CreateDate = result.CreateDate?.ToString("dd-MM-yyyy");
            return response;
        }

        public async Task<PagedList<GetAllMealResponseModel>> GetAllMeal(PagingParams pagingParams)
        {
            var selectExpression = GetAllMealResponseModel.FromEntity();
            var includes = new Expression<Func<Meal, object>>[]
            {
                x => x.MealDishes,
                x => x.MealSessions,
            };
            Expression<Func<Meal, bool>> conditionAddition = e => true;

            var result = await _mealRepository.GetWithPaging(pagingParams, conditionAddition, selectExpression, includes);

            return result;
        }

        public async Task<List<GetAllMealByKitchenIdResponseModel>> GetAllMealByKitchenId(int id)
        {
            var result = await _context.MealDishes
        .Include(x => x.Dish)
        .ThenInclude(x => x.Kitchen)
        .Where(x => x.Dish.Kitchen.KitchenId == id)
        .GroupBy(x => x.Meal.MealId) 
        .Select(group => new GetAllMealByKitchenIdResponseModel
        {
            MealId = group.Key,
            Name = group.First().Meal.Name, 
            Image = group.First().Meal.Image,
            KitchenDtoReponseMeal = new KitchenDtoReponseMeal
            {
                KitchenId = group.First().Meal.Kitchen.KitchenId,
                Name = group.First().Meal.Kitchen.Name,
                Address = group.First().Meal.Kitchen.Address,
                AreaId = group.First().Meal.Kitchen.AreaId,
                UserId = group.First().Meal.Kitchen.UserId,
            },
            DishModel = group.Select(md => new DishModel
            {
                DishId = md.Dish.DishId,
                Name = md.Dish.Name,
                Image = md.Dish.Image,
                DishTypeId = md.Dish.DishTypeId,
                KitchenId = md.Dish.KitchenId
            }).ToList()

        }).ToListAsync();

            return result;
        }

        public  Task<GetMealByMealIdResponseModel> GetMealByMealId(int mealid)
        {
            var result = _context.Meals
                .Include(x => x.MealDishes)
                .ThenInclude(x => x.Dish)
                .Where(x => x.MealId == mealid)
                .Select(group => new GetMealByMealIdResponseModel
                {
                    MealId = group.MealId,
                    Name = group.Name,
                    Image = group.Image,
                    KitchenDto = new KitchenDto
                    {
                        KitchenId = group.Kitchen.KitchenId,
                        UserId = group.Kitchen.UserId,
                        Name = group.Kitchen.Name,
                        Address = group.Kitchen.Address,
                    },
                    Description = group.Description,
                    DishDto = group.MealDishes.Select(md => new DishDto
                    {
                        DishId = md.Dish.DishId,
                        Name = md.Dish.Name,
                        Image = md.Dish.Image,
                        DishType = md.Dish.DishType,
                    }).ToList()
                }).FirstOrDefault();

            var mapped = _mapper.Map<GetMealByMealIdResponseModel>(result);
            return Task.FromResult(mapped);
        }

        public async Task<List<GetAllMealResponseModelNew>> GetAllMeal()
        {
            var result = _context.Meals
                .Include(x => x.Kitchen)
                .ToList();

            var mapped = result.Select(meal =>
            {
                var response = _mapper.Map<GetAllMealResponseModelNew>(meal);
                response.MealId = meal.MealId;
                response.Name = meal.Name;
                response.Image = meal.Image;
                response.KitchenDtoGetAllMealResponseModelNew = new KitchenDtoGetAllMealResponseModelNew
                {
                    KitchenId = meal.Kitchen.KitchenId,
                    UserId = meal.Kitchen.UserId,
                    Name = meal.Kitchen.Name,
                    Address = meal.Kitchen.Address,
                    AreaId = meal.Kitchen.AreaId,
                };
                response.CreateDate = meal.CreateDate.ToString();
                response.Description = meal.Description;

                return response;
            }).ToList();

             return mapped;
        }
    }
}
