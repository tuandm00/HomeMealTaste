﻿using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.EntityFrameworkCore;


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
                    WalletDtoKitchenResponseModel = kitchen.User.Wallets
                .OrderBy(wallet => wallet.WalletId)
                .Select(wallet => new WalletDtoKitchenResponseModel
                {
                    WalletId = wallet.WalletId,
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                })
                .FirstOrDefault(),
                };

                response.Name = kitchen.Name;
                response.Address = kitchen.Address;

                return response;
            }).ToList();

            return mapped;
        }

        public async Task<KitchenResponseModel> GetAllKitchenByKitchenId(int id)
        {
            var result = _context.Kitchens
                .Where(x => x.KitchenId == id)
                .Select(x => new KitchenResponseModel
                {
                    KitchenId = x.KitchenId,
                    UserDtoKitchenResponseModel = new UserDtoKitchenResponseModel
                    {
                        UserId = x.User.UserId,
                        Username = x.User.Username,
                        Email = x.User.Email,
                        Phone = x.User.Phone,
                        WalletDtoKitchenResponseModel = x.User.Wallets
                .OrderBy(wallet => wallet.WalletId)
                .Select(wallet => new WalletDtoKitchenResponseModel
                {
                    WalletId = wallet.WalletId,
                    UserId = wallet.UserId,
                    Balance = wallet.Balance,
                }).FirstOrDefault(),
                    },
                    Name = x.Name,
                    Address = x.Address,
                }).FirstOrDefault();

            return _mapper.Map<KitchenResponseModel>(result);
        }

        public async Task<List<GetAllKitchenBySessionIdResponseModel>> GetAllKitchenBySessionId(int sessionid)
        {
            var kitchenid = _context.MealSessions
                .Where(x => x.SessionId == sessionid)
                .Select(x => x.KitchenId);

            var result = _context.Kitchens.Where(x => kitchenid.Contains(x.KitchenId)).Select(x => new GetAllKitchenBySessionIdResponseModel
            {
                KitchenId = x.KitchenId,
                Address = x.Address,
                Name = x.Name,
                UserId = (int)x.UserId,
                UserDtoGetAllKitchenBySessionId = new UserDtoGetAllKitchenBySessionId
                {
                    Name = x.User.Name,
                    UserId = x.User.UserId,
                    Username = x.User.Username,
                    Address = x.User.Address,
                    DistrictId = x.User.DistrictId,
                    Email = x.User.Email,
                    Phone = x.User.Phone,
                },
                AreaDtoGetAllKitchenBySessionId = new AreaDtoGetAllKitchenBySessionId
                {
                    Address = x.Area.Address,
                    AreaId = x.Area.AreaId,
                    AreaName = x.Area.AreaName,
                },
            });

            var mapped = result.Select(result => _mapper.Map<GetAllKitchenBySessionIdResponseModel>(result)).ToList();
            return mapped;
        }


    }
}
