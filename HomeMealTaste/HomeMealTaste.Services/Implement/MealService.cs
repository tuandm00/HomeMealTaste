using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        private readonly IMapper _mapper;
        public MealService(IMealRepository mealRepository, IMapper mapper)
        {
            _mealRepository = mealRepository;   
            _mapper = mapper;
        }
        public async Task<MealResponseModel> CreateMeal(MealRequestModel mealRequest)
        {
            var entity = _mapper.Map<Meal>(mealRequest);
            var result = await _mealRepository.Create(entity, true);

            return _mapper.Map<MealResponseModel>(result);
        }
    }
}
