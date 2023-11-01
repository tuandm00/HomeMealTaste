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

namespace HomeMealTaste.Services.Implement
{
    public class MealSessionService : IMealSessionService
    {
        private readonly IMealSessionRepository _mealSessionRepository;
        private readonly IMapper _mapper;
        private readonly IDishRepository _dishRepository;
        private readonly IMealRepository _mealRepository;
        private readonly IMealDishRepository _mealDishRepository;

        public MealSessionService(IMealSessionRepository mealSessionRepository, IMapper mapper,
            IDishRepository dishRepository, IMealRepository mealRepository, IMealDishRepository mealDishRepository)
        {
            _mealSessionRepository = mealSessionRepository;
            _mapper = mapper;
            _dishRepository = dishRepository;
            _mealRepository = mealRepository;
            _mealDishRepository = mealDishRepository;
        }

        public async Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest)
        {
            var entity = _mapper.Map<MealSession>(mealSessionRequest);
            var result = await _mealSessionRepository.Create(entity, true);

            return _mapper.Map<MealSessionResponseModel>(result);
        }

        public async Task<PagedList<GetAllMealInCurrentSessionResponseModel>> GetAllMealSession(
            GetAllMealRequest pagingParams)
        {
            var selectExpression = GetAllMealInCurrentSessionResponseModel.FromEntity();
            var includes = new Expression<Func<MealSession, object>>[]
            {
                x => x.Meal!,
                x => x.Session!,
                x => x.Meal.MealDishes,
            };
            Expression<Func<MealSession, bool>> conditionAddition = e => e.Session.StartTime < (pagingParams.SessionStartTime ?? DateTime.Now);

            var result =
                await _mealSessionRepository.GetWithPaging(pagingParams, conditionAddition, selectExpression, includes);
            foreach (var response in result.Data)
            {
                var mealId = response.Meal?.Id;
                if (mealId == null) continue;
                var meal = await _mealRepository.GetFirstOrDefault(x => x.MealId == mealId);

                var mealDishes = await _mealDishRepository.GetByCondition(x => x.MealId == meal.MealId);
                    
                var dishes = new List<Dish>();
                foreach (var dishId in mealDishes)
                {
                    var dish = await _dishRepository.GetFirstOrDefault(x => x.DishId == dishId.DishId, x => x.Kitchen);
                    var kitchen =_mapper.Map<GetAllMealInCurrentSessionResponseModel.ChefInfo>(dish.Kitchen);
                    response.Chef = kitchen;

                    dishes.Add(dish);
                }
                response.Dish = _mapper.Map<List<GetAllMealInCurrentSessionResponseModel.DishModel?>>(dishes);
            }

            return result;
        }
    }
}

