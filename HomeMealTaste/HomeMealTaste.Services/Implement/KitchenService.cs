using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class KitchenService : IKitchenService
    {
        private readonly IKitchenRepository _kitchenRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;


        public KitchenService(IKitchenRepository kitchenRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _kitchenRepository = kitchenRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<KitchenResponseModel>> GetAllKitchen()
        {
            var result = _context.Kitchens.Select(x => new KitchenResponseModel
            {
                KitchenId = x.KitchenId,
                User = new User
                {
                    Username = x.User.Username,
                    Email = x.User.Email,
                    RoleId = x.User.RoleId,
                    Phone = x.User.Phone,
                    Address = x.User.Address,
                    District = x.User.District,
                    Status = x.User.Status,

                },
                Name = x.Name,
                Address = x.Address,
                District = x.District,
            }).ToList();
            return result;
        }

        public Task<List<KitchenResponseModel>> GetAllKitchenByKitchenId(int id)
        {
            var result = _context.Kitchens.Where(x => x.KitchenId == id).Select(x => new KitchenResponseModel
            {
                KitchenId = x.KitchenId,
                User = new User
                {
                    Username = x.User.Username,
                    Email = x.User.Email,
                    RoleId = x.User.RoleId,
                    Phone = x.User.Phone,
                    Address = x.User.Address,
                    District = x.User.District,
                    Status = x.User.Status,
                },
                Name = x.Name,
                Address = x.Address,
                District = x.District,
            }).ToList();

            var mappedResults = result.Select(kitchen => _mapper.Map<KitchenResponseModel>(kitchen)).ToList();
            return Task.FromResult(mappedResults);
        }
    }
}
