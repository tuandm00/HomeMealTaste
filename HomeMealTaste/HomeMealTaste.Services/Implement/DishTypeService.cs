using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
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

        //public async Task<PagedList<DishType>> GetAllDishType(PagingParams pagingParams)
        //{
        //    return await _dishTypeRepository.GetWithPaging(pagingParams);
        //}


        public Task DeleteDishTypeById(int id)
        {
            if(id >= 0)
            {
                var result = _dishTypeRepository.Delete(id);
                return result;
            }
            return null;
        }

        public async Task<UpdateDishTypeResponseModel> UpdateDishType(UpdateDishTypeRequestModel request)
        {
            var entity = _mapper.Map<DishType>(request);
            var result = _context.DishTypes.Where(x => x.DishTypeId == request.DishTypeId).FirstOrDefault();
            if(result != null)
            {
                result.Name = request.Name;
                result.Description = request.Description;

                _context.DishTypes.Update(result);
            }
            await _context.SaveChangesAsync();

            var mapped = _mapper.Map<UpdateDishTypeResponseModel>(result);
            return mapped;
        }

        public async Task<DishTypeResponseModel> GetSingleDishTypeById(int dishtypeId)
        {
            var result = _context.DishTypes.Where(x => x.DishTypeId == dishtypeId).Select(x => new DishTypeResponseModel
            {
                Description = x.Description,
                Name = x.Name
                
            }).FirstOrDefault();
            var mapped = _mapper.Map<DishTypeResponseModel>(result);
            return mapped;
        }

        public async Task<List<DishTypeResponseModel>> GetAllDishType()
        {
            var result = _context.DishTypes.Select(x => new DishTypeResponseModel
            {
                DishTypeId= x.DishTypeId,
                Description= x.Description,
                Name= x.Name
            }).ToList();
            var mapped = result.Select(r => _mapper.Map<DishTypeResponseModel>(r)).ToList();
            return mapped;
        }
    }
}
