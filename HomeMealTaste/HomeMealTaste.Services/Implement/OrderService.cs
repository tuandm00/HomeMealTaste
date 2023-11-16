using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Globalization;


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
        public static DateTime TranferDateTimeByTimeZone(DateTime dateTime, string timezoneArea)
        {

            ReadOnlyCollection<TimeZoneInfo> collection = TimeZoneInfo.GetSystemTimeZones();
            var timeZone = collection.ToList().Where(x => x.DisplayName.ToLower().Contains(timezoneArea)).First();

            var timeZoneLocal = TimeZoneInfo.Local;

            var utcDateTime = TimeZoneInfo.ConvertTime(dateTime, timeZoneLocal, timeZone);

            return utcDateTime;
        }

        public static DateTime GetDateTimeTimeZoneVietNam()
        {

            return TranferDateTimeByTimeZone(DateTime.Now, "hanoi");
        }
        public static DateTime? StringToDateTimeVN(string dateStr)
        {

            var isValid = System.DateTime.TryParseExact(
                                dateStr,
                                "d'/'M'/'yyyy",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var date
                            );
            return isValid ? date : null;
        }
        
        public async Task<List<OrderResponseModel>> GetAllOrder()
        {
            var result = _context.Orders
                .Include(x => x.MealSession.Meal.Kitchen)
                .Include(x => x.MealSession)
                .Select(x => new OrderResponseModel
                {
                OrderId = x.OrderId,
                Time = x.Time.ToString(),
                CustomerDto1 = new CustomerDto1
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    DistrictId = x.Customer.DistrictId,
                    AreaId = x.Customer.AreaId,
                },
                MealSessionDto1 = new MealSessionDto1
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealDto1 = new MealDto1
                    {
                        MealId = x.MealSession.Meal.MealId,
                        Name = x.MealSession.Meal.Name,
                        Image = x.MealSession.Meal.Image,
                        KitchenDto1 = new KitchenDto1
                        {
                            KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                            UserId = x.MealSession.Meal.Kitchen.UserId,
                            Name = x.MealSession.Meal.Kitchen.Name,
                            Address = x.MealSession.Meal.Kitchen.Address,
                            AreaId = x.MealSession.Meal.Kitchen.AreaId,
                        },
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        Description = x.MealSession.Meal.Description,
                    },
                    SessionDto1 = new SessionDto1
                    {
                        SessionId = x.MealSession.Session.SessionId,
                        CreateDate = x.MealSession.Session.CreateDate.ToString(),
                        StartTime = x.MealSession.Session.StartTime.ToString(),
                        EndTime = x.MealSession.Session.EndTime.ToString(),
                        EndDate = x.MealSession.Session.EndDate.ToString(),
                        UserId = x.MealSession.Session.UserId,
                        Status = x.MealSession.Session.Status,
                        SessionType = x.MealSession.Session.SessionType,
                        AreaId = x.MealSession.Session.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                Price = x.Price,
            });
            var mappedResult = result.Select(x => _mapper.Map<OrderResponseModel>(x)).ToList();
            
            return mappedResult;
        }

        public Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id)
        {
            var results = _context.Orders
                .Include(x => x.MealSession.Meal.Kitchen)
                .Include(x => x.MealSession)
                .Where(x => x.OrderId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                    Time = x.Time.ToString(),
                    CustomerDto2 = new CustomerDto2
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    DistrictId = x.Customer.DistrictId,
                    AreaId = x.Customer.AreaId,
                },
                MealSessionDto2 = new MealSessionDto2
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealDto2 = new MealDto2
                    {
                        MealId = x.MealSession.Meal.MealId,
                        Name = x.MealSession.Meal.Name,
                        Image = x.MealSession.Meal.Image,
                        KitchenDto2 = new KitchenDto2
                        {
                            KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                            UserId = x.MealSession.Meal.Kitchen.UserId,
                            Name = x.MealSession.Meal.Kitchen.Name,
                            Address = x.MealSession.Meal.Kitchen.Address,
                            AreaId = x.MealSession.Meal.Kitchen.AreaId,
                        },
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        Description = x.MealSession.Meal.Description,

                    },
                    SessionDto2 = new SessionDto2
                    {
                        SessionId = x.MealSession.Session.SessionId,
                        CreateDate = x.MealSession.Session.CreateDate.ToString(),
                        StartTime = x.MealSession.Session.StartTime.ToString(),
                        EndTime = x.MealSession.Session.EndTime.ToString(),
                        EndDate = x.MealSession.Session.EndDate.ToString(),
                        UserId = x.MealSession.Session.UserId,
                        Status = x.MealSession.Session.Status,
                        SessionType = x.MealSession.Session.SessionType,
                        AreaId = x.MealSession.Session.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                Price = x.Price,
            }).FirstOrDefault();

            return Task.FromResult(results);
        }

        public async Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id)
        {
            var results = _context.Orders.Where(x => x.CustomerId == id).Select(x => new GetAllOrderByUserIdResponseModel
            {
                OrderId = x.OrderId,
                Time = x.Time.ToString(),

                CustomerDto2 = new CustomerDto2
                {
                    CustomerId = x.Customer.CustomerId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    DistrictId = x.Customer.DistrictId,
                    AreaId = x.Customer.AreaId,
                },
                MealSessionDto2 = new MealSessionDto2
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealDto2 = new MealDto2
                    {
                        MealId = x.MealSession.Meal.MealId,
                        Name = x.MealSession.Meal.Name,
                        Image = x.MealSession.Meal.Image,
                        KitchenDto2 = new KitchenDto2
                        {
                            KitchenId = x.MealSession.Meal.Kitchen.KitchenId,
                            UserId = x.MealSession.Meal.Kitchen.UserId,
                            Name = x.MealSession.Meal.Kitchen.Name,
                            Address = x.MealSession.Meal.Kitchen.Address,
                            AreaId = x.MealSession.Meal.Kitchen.AreaId,
                        },
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        Description = x.MealSession.Meal.Description,

                    },
                    SessionDto2 = new SessionDto2
                    {
                        SessionId = x.MealSession.Session.SessionId,
                        CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        StartTime = GetDateTimeTimeZoneVietNam().ToString("HH:mm"),
                        EndTime = GetDateTimeTimeZoneVietNam().ToString("HH:mm"),
                        EndDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                        UserId = x.MealSession.Session.UserId,
                        Status = x.MealSession.Session.Status,
                        SessionType = x.MealSession.Session.SessionType,
                        AreaId = x.MealSession.Session.AreaId,
                    },
                    Price = x.MealSession.Price,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = GetDateTimeTimeZoneVietNam().ToString("dd-MM-yyyy"),
                },
                Status = x.Status,
                Price = x.Price,

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
                    Time = x.Time.ToString(),

                    Customer = new CustomerDto
                {
                    CustomerId = x.Customer.CustomerId,
                    UserId = x.Customer.UserId,
                    Name = x.Customer.Name,
                    Phone = x.Customer.Phone,
                    DistrictId = x.Customer.DistrictId,
                },
                MealSession = new MealSessionDto
                {
                    MealSessionId = x.MealSession.MealSessionId,
                    MealId = x.MealSession.MealId,
                    Quantity = x.MealSession.Quantity,
                    RemainQuantity = x.MealSession.RemainQuantity,
                    Status = x.MealSession.Status,
                    CreateDate = x.MealSession.CreateDate
                },
                    Status = x.Status,
                    Price = x.Price,
                });

            var mapped = result.Select(x => _mapper.Map<GetOrderByKitchenIdResponseModel>(x)).ToList();
            return Task.FromResult(mapped);


        }

        public async Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest)
        {
            using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? transaction = _context.Database.BeginTransaction();
            //var entity = _mapper.Map<Order>(createOrderRequest);
            //var customerid = _context.Customers.Where(customer => customer.CustomerId == entity.CustomerId).AsNoTracking().FirstOrDefault();
            //var mealsessionid = _context.MealSessions
            //    .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId)
            //    .Include(mealsession => mealsession.Meal)
            //        .ThenInclude(meal => meal.MealDishes)
            //        .ThenInclude(mealDish => mealDish.Dish)
            //             .AsNoTracking().FirstOrDefault();

            //var mealdish = _context.MealDishes.Where(x => x.MealId == mealsessionid.MealId).AsNoTracking().FirstOrDefault();
            //var kitchenid = mealsessionid.KitchenId;
            //var price = mealsessionid.Price;
            //var createOrder = new CreateOrderRequestModel
            //{
            //    CustomerId = entity.CustomerId,
            //    CustomerDtoRequest = new CustomerDtoRequest
            //    {
            //        CustomerId = customerid.CustomerId,
            //        UserId = customerid.UserId,
            //        Name = customerid.Name,
            //        Phone = customerid.Phone,
            //        District = customerid.District,
            //        AreaId = customerid.AreaId,
            //    },
            //    MealSessionId = mealsessionid.MealSessionId,
            //    MealSessionDtoRequest = new MealSessionDtoRequest
            //    {
            //        MealSessionId = mealsessionid.MealSessionId,
            //        MealDtoRequest = new MealDtoRequest
            //        {
            //            MealId = mealsessionid.Meal.MealId,
            //            Name = mealsessionid.Meal.Name,
            //            Image = mealsessionid.Meal.Image,
            //            KitchenDtoRequest = new KitchenDtoRequest
            //            {
            //                KitchenId = mealsessionid.Meal.Kitchen.KitchenId,
            //                UserId = mealsessionid.Meal.Kitchen.KitchenId,
            //                Name = mealsessionid.Meal.Kitchen.Name,
            //                Address = mealsessionid.Meal.Kitchen.Address,
            //                District = mealsessionid.Meal.Kitchen.District,
            //                AreaId = mealsessionid.Meal.Kitchen.AreaId,
            //            },
            //            MealDishDtoRequest = new MealDishDtoRequest
            //            {
            //                MealDishId = mealdish.MealDishId,
            //                MealId = mealdish.MealId,
            //                DishId = mealdish.DishId,
            //                DishDtoRequest = new List<DishDtoRequest?>
            //                {
            //                    new DishDtoRequest
            //                    {
            //                        DishId = mealdish.Dish.DishId,
            //                        Name = mealdish.Dish.Name,
            //                        Image = mealdish.Dish.Image,
            //                        DishTypeId = mealdish.Dish.DishTypeId,
            //                        KitchenDtoRequest = new KitchenDtoRequest
            //                        {
            //                            KitchenId = kitchenid.KitchenId,
            //                            UserId = kitchenid.UserId,
            //                            Name = kitchenid.Name,
            //                            Address = kitchenid.Address,
            //                            District = kitchenid.District,
            //                            AreaId = kitchenid.AreaId
            //                        },
            //                    }
            //                }
            //            },
            //            Description = mealsessionid.Meal.Description,
            //            CreateDate = mealsessionid.Meal.CreateDate,
            //        },
            //        Price = mealsessionid.Price,
            //        Quantity = mealsessionid.Quantity,
            //        RemainQuantity = mealsessionid.RemainQuantity,
            //        Status = mealsessionid.Status,
            //        CreateDate = mealsessionid.CreateDate,
            //        KitchenDtoRequest = new KitchenDtoRequest
            //        {
            //            KitchenId = kitchenid.KitchenId,
            //            UserId = kitchenid.UserId,
            //            Name = kitchenid.Name,
            //            Address = kitchenid.Address,
            //            District = kitchenid.District,
            //            AreaId = kitchenid.AreaId
            //        },
            //    },
            //    Price = (int?)price,
            //    Time = GetDateTimeTimeZoneVietNam(),
            //    Status = "PAID",
            //};
            var entity = _mapper.Map<Order>(createOrderRequest);
            var customerid = _context.Customers.Where(customer => customer.CustomerId == entity.CustomerId).AsNoTracking().FirstOrDefault();
            var mealsessionid = _context.MealSessions
                .Where(mealsession => mealsession.MealSessionId == entity.MealSessionId)
                .Include(mealsession => mealsession.Meal)
                    .ThenInclude(meal => meal.MealDishes)
                    .ThenInclude(mealDish => mealDish.Dish)
                .AsNoTracking().FirstOrDefault();

            var mealdish = _context.MealDishes.Where(x => x.MealId == mealsessionid.MealId).AsNoTracking().FirstOrDefault();
            var kitchenid = mealsessionid.KitchenId;
            var price = mealsessionid.Price;
            var createOrder = new CreateOrderRequestModel
            {
                CustomerId = entity.CustomerId,
                Price = (int?)price,
                Time = GetDateTimeTimeZoneVietNam(),
                Status = "PAID",
                MealSessionId = mealsessionid.MealSessionId,
            };

            var orderEntity = _mapper.Map<Order>(createOrder);
            await _context.AddAsync(orderEntity);
            await _context.SaveChangesAsync();
            transaction.Commit();

            var mapped = _mapper.Map<CreateOrderResponseModel>(orderEntity);
            return mapped;
        }

        
    }
}
