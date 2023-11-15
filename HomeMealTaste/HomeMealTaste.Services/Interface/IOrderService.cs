using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;


namespace HomeMealTaste.Services.Interface
{
    public interface IOrderService
    {
        Task<List<GetAllOrderByUserIdResponseModel>> GetAllOrderByCustomerId(int id);
        Task<GetAllOrderByUserIdResponseModel> GetSingleOrderById(int id);
        Task<List<OrderResponseModel>> GetAllOrder();
        Task<List<GetOrderByKitchenIdResponseModel>> GetOrderByKitchenId(int kitchenid);
        Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel createOrderRequest);
    }
}
