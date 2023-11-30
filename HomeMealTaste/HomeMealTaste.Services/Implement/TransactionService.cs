﻿using AutoMapper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, HomeMealTasteContext context, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<GetAllTransactionByTransactionTypeORDERED>> GetAllTransactionByTransactionTypeORDERED()
        {
            var result = _context.Transactions
                .Where(x => x.TransactionType == "ORDERED")
                .Select(t => new GetAllTransactionByTransactionTypeORDERED
                {
                    TransactionId = t.TransactionId,
                    OrderId = t.OrderId,
                    WalletDtoGetAllTransaction = new WalletDtoGetAllTransaction
                    {
                        WalletId = t.Wallet.WalletId,
                        UserDtoGetAllTransaction = new UserDtoGetAllTransaction
                        {
                            UserId = t.Wallet.User.UserId,
                            Name = t.Wallet.User.Name,
                            Username = t.Wallet.User.Username,
                            Email = t.Wallet.User.Email,
                            Phone = t.Wallet.User.Phone,
                            Address = t.Wallet.User.Address,
                            DistrictId = t.Wallet.User.DistrictId,
                            Status = t.Wallet.User.Status,
                            AreaId = t.Wallet.User.AreaId,
                        },
                        Balance = t.Wallet.Balance,
                    },
                    Date = t.Date.ToString(),
                    Amount = t.Amount,
                    Description = t.Description,
                    Status = t.Status,
                    TransactionType = t.TransactionType,
                });
            var mapped = result.Select(result => _mapper.Map<GetAllTransactionByTransactionTypeORDERED>(result)).ToList();
            return mapped;
        }

        public async Task<List<GetAllTransactionByTransactionTypeRECHARGED>> GetAllTransactionByTransactionTypeRECHARGED()
        {
            var result = _context.Transactions
                .Where(x => x.TransactionType == "RECHARGED")
                .Select(t => new GetAllTransactionByTransactionTypeRECHARGED
                {
                    TransactionId = t.TransactionId,
                    OrderId = t.OrderId,
                    WalletDtoGetAllTransactionRECHARGED = new WalletDtoGetAllTransactionRECHARGED
                    {
                        WalletId = t.Wallet.WalletId,
                        UserDtoGetAllTransactionRECHARGED = new UserDtoGetAllTransactionRECHARGED
                        {
                            UserId = t.Wallet.User.UserId,
                            Name = t.Wallet.User.Name,
                            Username = t.Wallet.User.Username,
                            Email = t.Wallet.User.Email,
                            Phone = t.Wallet.User.Phone,
                            Address = t.Wallet.User.Address,
                            DistrictId = t.Wallet.User.DistrictId,
                            Status = t.Wallet.User.Status,
                            AreaId = t.Wallet.User.AreaId,
                        },
                        Balance = t.Wallet.Balance,
                    },
                    Date = t.Date.ToString(),
                    Amount = t.Amount,
                    Description = t.Description,
                    Status = t.Status,
                    TransactionType = t.TransactionType,
                });
            var mapped = result.Select(result => _mapper.Map<GetAllTransactionByTransactionTypeRECHARGED>(result)).ToList();
            return mapped;
        }

        public async Task<List<GetAllTransactionByUserIdResponseModel>> GetAllTransactionByUserId(int userid)
        {
            var result = _context.Transactions
        .Include(x => x.Wallet)
        .ThenInclude(x => x.User)
        .Include(x => x.Order)
        .Where(x => x.Wallet.UserId == userid)
        .ToList();

            var mapped = result.Select(transaction =>
            {
                var response = _mapper.Map<GetAllTransactionByUserIdResponseModel>(transaction);
                response.TransactionId = transaction.TransactionId;
                response.Date = transaction.Date.ToString();
                response.Amount = transaction.Amount;
                response.Description = transaction.Description;
                response.Status = transaction.Status;
                response.OrderDtoTransactionResponse = new OrderDtoTransactionResponse
                {
                    OrderId = transaction.Order.OrderId,
                    CustomerId = transaction.Order.CustomerId,
                    Status = transaction.Order.Status,
                    MealSessionId = transaction.Order.MealSessionId,
                    TotalPrice = transaction.Order.TotalPrice,
                    Time = transaction.Order.Time.ToString(),
                    Quantity = transaction.Order.Quantity,
                };
                response.WalletDtoTransactionResponse = new WalletDtoTransactionResponse
                {
                    WalletId = transaction.Wallet.WalletId,
                    UserId = transaction.Wallet.UserId,
                    Balance = transaction.Wallet.Balance,
                };
                return response;
            }).ToList();

            return mapped;

        }
    }
}
