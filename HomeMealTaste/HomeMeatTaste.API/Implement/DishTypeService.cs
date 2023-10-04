using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Implement
{
    public class DishTypeService : IDishTypeService
    {
        private readonly IDishTypeRepository _dishTypeRepository;
        private readonly HomeMealTasteContext _context;
        public DishTypeService(IDishTypeRepository dishTypeRepository, HomeMealTasteContext context)
        {
            _dishTypeRepository = dishTypeRepository;
            _context = context;
        }
        public async Task<DishTypeResponseModel> CreateDishType(DishTypeRequestModel requestModel)
        {

            var request = new DishType()
            {
                DishTypeId = requestModel.DishTypeId,
                Name = requestModel.Name,
                Description = requestModel.Description,
            };

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

            var response = new DishTypeResponseModel()
            {
                DishTypeId = request.DishTypeId,
                Name = request.Name,
                Description = request.Description,
            };

            return response;
        }

        public List<DishTypeRequestModel> GetAllDishType()
        {
            var result = _dishTypeRepository.GetAllDishType();
            return result;
        }
    }
}
