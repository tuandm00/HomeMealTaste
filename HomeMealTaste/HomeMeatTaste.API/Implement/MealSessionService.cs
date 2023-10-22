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
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Implement
{
    public class MealSessionService : IMealSessionService
    {
        private readonly IMealSessionRepository _mealSessionRepository;
        private readonly IMapper _mapper;

        public MealSessionService(IMealSessionRepository mealSessionRepository, IMapper mapper)
        {
            _mealSessionRepository = mealSessionRepository;
            _mapper = mapper;
        }

        public async Task<MealSessionResponseModel> CreateMealSession(MealSessionRequestModel mealSessionRequest)
        {
            var entity = _mapper.Map<MealSession>(mealSessionRequest);
            var result = await _mealSessionRepository.Create(entity,true);

            return _mapper.Map<MealSessionResponseModel>(result);
        }

        public async Task<PagedList<MealSession>> GetAllMealSession(PagingParams pagingParams)
        {
            var result = await _mealSessionRepository.GetWithPaging(pagingParams);

            return result;
        }
    }
}
