using AutoMapper;
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
            var result = _context.Orders.ToList();
            var mappedResult = result.Select(x => _mapper.Map<OrderResponseModel>(x)).ToList();
            return mappedResult;
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
                Meal = new Meal
                {
                    MealId = x.Meal.MealId,
                    Name = x.Meal.Name,
                    Image = x.Meal.Image,
                    DefaultPrice = x.Meal.DefaultPrice,
                    Kitchen = new Kitchen
                    {
                        KitchenId = x.Meal.Kitchen.KitchenId,
                        Name = x.Meal.Kitchen.Name,
                        Phone = x.Meal.Kitchen.Phone,
                        Address = x.Meal.Kitchen.Address,
                        District = x.Meal.Kitchen.District
                    }
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
    }
}
