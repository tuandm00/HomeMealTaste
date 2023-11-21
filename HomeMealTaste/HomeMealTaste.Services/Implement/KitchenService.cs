using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
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
            var result = _context.Kitchens
                .Include(x => x.User)
                .ThenInclude(x => x.Wallets)
                .ToList();
            var mapped = result.Select(kitchen =>
            {
                var response = _mapper.Map<KitchenResponseModel>(kitchen);
                response.KitchenId = kitchen.KitchenId;
                response.UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
                {
                    UserId = kitchen.User.UserId,
                    Username = kitchen.User.Username,
                    Email = kitchen.User.Email,
                    Phone = kitchen.User.Phone,
                    WalletDtoKitchenResponseModel = kitchen.User.Wallets.Select(wallet => new WalletDtoKitchenResponseModel
                    {
                        WalletId = wallet.WalletId,
                        UserId = wallet.UserId,
                        Balance = wallet.Balance,

                    }).ToList()
                };

                response.Name = kitchen.Name;
                response.Address = kitchen.Address;

                return response;
            }).ToList();

            return mapped;
        }

        public async Task<KitchenResponseModel> GetAllKitchenByKitchenId(int id)
        {
            var result = _context.Kitchens.Where(x => x.KitchenId == id).Select(x => new KitchenResponseModel
            {
                KitchenId = x.KitchenId,
                UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
                {
                    UserId = x.User.UserId,
                    Username = x.User.Username,
                    Email = x.User.Email,
                    Phone = x.User.Phone,
                    WalletDtoKitchenResponseModel = x.User.Wallets.Select(wallet => new WalletDtoKitchenResponseModel
                    {
                        WalletId = wallet.WalletId,
                        UserId = wallet.UserId,
                        Balance = wallet.Balance,

                    }).ToList()
                },
                Name = x.Name,
                Address = x.Address,
            }).FirstOrDefault();

            return _mapper.Map<KitchenResponseModel>(result);
        }
    }
}
