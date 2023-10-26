﻿using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Interface;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.Helper;

namespace HomeMealTaste.Services.Implement
{
    public class DishService : IDishService
    {
        private readonly IDishRepository _dishRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public DishService(IDishRepository dishRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _dishRepository = dishRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<DishResponseModel> CreateDishAsync(DishRequestModel dishRequest)
        {
            var request = new Dish()
            {
                DishId = dishRequest.DishId,
                Name = dishRequest.Name,
                Image = dishRequest.Image,
                DishTypeId = dishRequest.DishTypeId,
                KitchenId = dishRequest.KitchenId,
            };

            await _context.AddAsync(request);
            await _context.SaveChangesAsync();

            var response = new DishResponseModel()
            {
                DishId = request.DishId,
                Name = request.Name,
                Image = request.Image,
                DishTypeId = request.DishTypeId,
                KitchenId = request.KitchenId,
            };

            return response;
            
        }

        public async Task<DishResponseModel> GetDetailAsync(int id)
        {
            var dish = await _dishRepository.GetFirstOrDefault(x => x.DishId == id) ?? throw new Exception($"No dish found with id {id}");

            return _mapper.Map<DishResponseModel>(dish);
        }

        public async Task DeleteAsync(int id)
        {
            _ = await _dishRepository.GetFirstOrDefault(x => x.DishId == id) ??
                throw new Exception($"No dish found with id {id}");
            
            await _dishRepository.Delete(id);
        }

        public async Task<PagedList<Dish>> GetAllDishAsync(PagingParams pagingParams)
        {
            var result = await _dishRepository.GetWithPaging(pagingParams);
            
            return result;
        }
    }
}
