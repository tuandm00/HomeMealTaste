using HomeMealTaste.Models;


namespace HomeMealTaste.Data.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUsernamePassword(User user);

        List<User> GetAllUser();

    }
}
