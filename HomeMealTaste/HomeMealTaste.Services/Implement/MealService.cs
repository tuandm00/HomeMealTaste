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
    }
}
