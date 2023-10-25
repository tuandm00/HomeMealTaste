using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Implement
{
    public class DishTypeService : IDishTypeService
    {
        private readonly IDishTypeRepository _dishTypeRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;
        public DishTypeService(IDishTypeRepository dishTypeRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _dishTypeRepository = dishTypeRepository;
            _context = context;
            _mapper = mapper;
        }
        public async Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel dishTypeRequest)
        {
            var entity = _mapper.Map<DishType>(dishTypeRequest);
            var result = await _dishTypeRepository.Create(entity, true);
            return _mapper.Map<DishTypeResponseModel>(result);

        }

        public async Task<PagedList<DishType>> GetAllDishType(PagingParams pagingParams)
        {
            return await _dishTypeRepository.GetWithPaging(pagingParams);
        }

        public Task DeleteDishTypeById(int id)
        {
            if(id >= 0)
            {
                var result = _dishTypeRepository.Delete(id);
                return result;
            }
            return null;
        }
    }
}
