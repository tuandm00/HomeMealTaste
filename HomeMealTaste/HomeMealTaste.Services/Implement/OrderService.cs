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
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly HomeMealTasteContext _context;
        public OrderService(IOrderRepository orderRepository, IMapper mapper, HomeMealTasteContext context)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        public async Task<List<OrderResponseModel>> GetAllOrder()
        {
            var result = _context.Orders.Select(x => new OrderResponseModel
            {
                OrderId = x.OrderId,
                Date = x.Date,
                Customer = new Customer
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    Address = x.Customer.Address,
                    District = x.Customer.District,
                },
                MealSession = new MealSession
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealId = x.MealSession.MealId,
                    Points = x.MealSession.Points,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = x.MealSession.CreateDate,
                },
                Session = new Session
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = x.Session.CreateDate,
                    StartTime = x.Session.StartTime,
                    EndTime = x.Session.EndTime,
                    EndDate = x.Session.EndDate,
                    SessionName = x.Session.SessionName,
                    SessionType = x.Session.SessionType,
                },
            });
            var mappedResult = result.Select(x => _mapper.Map<OrderResponseModel>(x)).ToList();
            return mappedResult;
        }

        public Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderById(int id)
        {
            var results = _context.Orders.Where(x => x.OrderId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = id,
                Date = DateTime.Now,
                Customer = new Customer
                {
                    CustomerId = id,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    Address = x.Customer.Address,
                    District = x.Customer.District,
                },
                MealSession = new MealSession
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealId = x.MealSession.MealId,
                    Points = x.MealSession.Points,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = x.MealSession.CreateDate,
                },
                Session = new Session
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = x.Session.CreateDate,
                    StartTime = x.Session.StartTime,
                    EndTime = x.Session.EndTime,
                    EndDate = x.Session.EndDate,
                    SessionName = x.Session.SessionName,
                    SessionType = x.Session.SessionType,
                },
            }).ToList();

            var mappedResults = results.Select(order => _mapper.Map<GetAllOrderByUserIdResponseModel>(order)).ToList();
            return Task.FromResult(mappedResults);
        }

        public async Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByUserId(int id)
        {
            var results = _context.Orders.Where(x => x.CustomerId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                Date = DateTime.Now,
                Customer = new Customer
                {
                    CustomerId = id,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    Address = x.Customer.Address,
                    District = x.Customer.District,
                },
                MealSession = new MealSession
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealId = x.MealSession.MealId,
                    Points = x.MealSession.Points,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = x.MealSession.CreateDate,
                },
                Session = new Session
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = x.Session.CreateDate,
                    StartTime = x.Session.StartTime,
                    EndTime = x.Session.EndTime,
                    EndDate = x.Session.EndDate,
                    SessionName = x.Session.SessionName,
                    SessionType = x.Session.SessionType,
                },
            }).ToList();

            var mappedResults = results.Select(order => _mapper.Map<GetAllOrderByUserIdResponseModel>(order)).ToList();
            return mappedResults;
        }

        public Task<List<GetOrderByKitchenIdResponseModel>> GetOrderByKitchenId(int kitchenid)
        {
            var result = _context.Orders
                .Include(x => x.MealSession)
                .ThenInclude(x => x.Meal)
                .ThenInclude(x => x.Kitchen)
                .Where(x => x.MealSession.Meal.Kitchen.KitchenId == kitchenid)
                .Select(x => new GetOrderByKitchenIdResponseModel
                {
                OrderId = x.OrderId,
                Date = x.Date,
                Customer = new CustomerDto
                {
                    CustomerId = x.Customer.CustomerId,
                    UserId = x.Customer.UserId,
                    Name = x.Customer.Name,
                    Address = x.Customer.Address,
                    Phone = x.Customer.Phone,
                    District = x.Customer.District,
                    AccountStatus = x.Customer.AccountStatus,
                },
                Status = x.Status,
                MealSession = new MealSessionDto
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealId = x.MealSession.MealId,
                    Points = x.MealSession.Points,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = x.MealSession.CreateDate
                },
                Session = new SessionDto
                {
                    SessionId = x.Session.SessionId,
                    CreateDate = x.Session.CreateDate,
                    StartTime = x.Session.StartTime,
                    EndTime = x.Session.EndTime,
                    EndDate = x.Session.EndDate,
                    SessionName = x.Session.SessionName,
                    UserId = x.Session.UserId,
                    SessionType = x.Session.SessionType,
                    Status = x.Session.Status,
                },
            });

            var mapped = result.Select(x => _mapper.Map<GetOrderByKitchenIdResponseModel>(x)).ToList();
            return Task.FromResult(mapped);


        }
    }
}
