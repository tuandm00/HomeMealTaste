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

namespace HomeMealTaste.Services.Implement
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;
        public MealService(IMealRepository mealRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _mealRepository = mealRepository;   
            _mapper = mapper;
            _context = context;
        }
        public async Task<MealResponseModel> CreateMeal(MealRequestModel mealRequest)
        {
            var entity = _mapper.Map<Meal>(mealRequest);
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
            return _mapper.Map<MealResponseModel>(result);
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

        public Task<List<GetAllMealByKitchenIdResponseModel>> GetAllMealByKitchenId(int id)
        {
            var result = _context.Meals.Where(x => x.KitchenId == id).Select(x => new GetAllMealByKitchenIdResponseModel
            {
                MealId = x.MealId,
                Name = x.Name,
                Image = x.Image,
                DefaultPrice = x.DefaultPrice,
                Kitchen = new Kitchen
                {
                    KitchenId = x.Kitchen.KitchenId,
                    Name = x.Kitchen.Name,
                    Address = x.Kitchen.Address,
                    District = x.Kitchen.District,
                },
            });

            var mapped = result.Select(meal => _mapper.Map<GetAllMealByKitchenIdResponseModel>(meal)).ToList();
            return Task.FromResult(mapped);
        }
    }
}
