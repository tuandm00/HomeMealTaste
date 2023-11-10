using AutoMapper;
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

        public async Task<List<OrderResponseModel>> GetAllOrderByUserId(int id)
        {
            var results = _context.Orders.Where(x => x.CustomerId == id).ToList();

            var mappedResults = results.Select(order => _mapper.Map<OrderResponseModel>(order)).ToList();

            return mappedResults;
        }
    }
}
