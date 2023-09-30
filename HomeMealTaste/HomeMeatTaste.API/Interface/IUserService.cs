using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Dto;

namespace HomeMealTaste.Services.Interface
{
    public interface IUserService
    {
        Task<LoginDto> LoginAsync(User user);
        Task<User> RegisterForCustomer(User user);
        Task<User> RegisterForChef(User user);
        Task<User> DeleteUserById(int id);
        List<User> GetAllUser();

    }
}
