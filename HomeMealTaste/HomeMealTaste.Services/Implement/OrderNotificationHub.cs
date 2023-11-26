using Microsoft.AspNetCore.SignalR;
namespace HomeMealTaste.Services.Implement

{
    public class OrderNotificationHub : Hub
    {
        public async Task SendOrderNotification(string customerId, string message)
        {
            await Clients.User(customerId).SendAsync("ReceiveOrderNotification", message);
        }
    }
}
