using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUsernamePassword(UserRequestModel userRequest);

        List<User> GetAllUser();

    }
}
