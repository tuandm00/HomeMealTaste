﻿using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using Microsoft.AspNetCore.Http;

namespace HomeMealTaste.Services.Interface
{
    public interface IOrderService
    {
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id);
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByUserId(int userId);
        Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id);
        Task<List<OrderResponseModel>> GetAllOrder();
        Task<List<GetAllOrderBySessionIdResponseModel>> GetAllOrderBySessionId(int sessionId);
        Task<List<OrderResponseModel>> GetAllOrderWithStatusCompleted();
        Task<List<GetOrderByKitchenIdResponseModel>> GetAllOrderByKitchenId(int kitchenid);
        Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest);
        Task RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotEnough(int orderId);
        Task RefundMoneyToSingleCustomerByOrderIdWhenChefCancelledOrderWithBookingSlotNotEnough(int orderId);
        Task RefundMoneyToSingleCustomerByOrderIdWhenCustomerCancelledOrderWithStatusPaid(int orderId);
        Task ChefCancelledOrderRefundMoneyToCustomer(RefundMoneyToWalletByOrderIdRequestModel refundRequest);
        Task ChefCancelledOrderRefundMoneyToCustomerV2(int mealsessionId);
        Task ChefCancelledNotEnoughOrderRefundMoneyToCustomerV2(int mealsessionId);
        Task ChangeStatusOrderToCancelledWhenOrderIsPaidByCustomer(int orderId);
        Task<List<ChangeStatusOrderToCompletedResponseModel>> ChangeStatusOrder(int mealsessionid, string status);
        Task<ChangeStatusOrderResponseModel> ChangeSingleStatusOrder(int orderId , string status);
        Task<List<ChangeStatusOrderResponseModel>> ChangeListStatusOrderToCancelledForAdmin(ChangeListStatusOrderToCancelledForAdminRequestModel request);
        Task<int> TotalOrderInSystem();
        Task<int> TotalPriceOfOrderInSystemInEveryMonth(int month);
        Task<List<GetTop5CustomerOrderTimesResponseModel>> GetTop5CustomerOrderTimes();
        Task<List<GetTop5ChefOrderTimesResponseModel>> GetTop5ChefOrderTimes();
        Task<List<TotalPriceOfOrderInSystemWithEveryMonthResponseModel>> TotalPriceOfOrderInSystemWithEveryMonth();
        Task<int> TotalCustomerOrderInSystem();
        Task<List<GetAllOrderByMealSessionIdResponseModel>> GetAllOrderByMealSessionId(int mealsessionid);
        Task<int> GetTotalPriceWithMealSessionByMealSessionId(int mealsessionid);
        Task<int> GetTotalPriceWithMealSessionBySessionIdAndKitchenId(int sessionId, int kitchenId);
        Task<List<GetAllOrderWithStatusPaidByMealSessionIdResponseModel>> GetAllOrderWithStatusPaidByMealSessionId(int mealsessionId);

    }
}
