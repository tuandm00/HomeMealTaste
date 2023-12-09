using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using Microsoft.AspNetCore.Http;

namespace HomeMealTaste.Services.Interface
{
    public interface IOrderService
    {
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id);
        Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id);
        Task<List<OrderResponseModel>> GetAllOrder();
        Task<List<GetOrderByKitchenIdResponseModel>> GetOrderByKitchenId(int kitchenid);
        Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest);
        Task ChefCancelledOrderRefundMoneyToCustomer(RefundMoneyToWalletByOrderIdRequestModel refundRequest);
        Task ChefCancelledOrderRefundMoneyToCustomerV2(int mealsessionId);
        Task<List<ChangeStatusOrderToCompletedResponseModel>> ChangeStatusOrder(int mealsessionid, string status);
        Task<int> TotalOrderInSystem();
        Task<List<GetAllOrderByMealSessionIdResponseModel>> GetAllOrderByMealSessionId(int mealsessionid);
        Task<int> GetTotalPriceWithMealSessionByMealSessionId(int mealsessionid);
        Task<int> GetTotalPriceWithMealSessionBySessionIdAndKitchenId(int sessionId, int kitchenId);
    }
}
