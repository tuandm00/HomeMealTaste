using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Interface
{
    public interface IUserService
    {
        Task<UserResponseModel> LoginAsync(UserRequestModel user);
        Task<User> RegisterForCustomer(User user);
        Task<User> RegisterForChef(User user);
        Task<User> DeleteUserById(int id);
        List<User> GetAllUser();

    }
}
