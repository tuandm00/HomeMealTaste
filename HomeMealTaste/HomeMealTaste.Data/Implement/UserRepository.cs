using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;

namespace HomeMealTaste.Data.Implement
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly HomeMealTasteContext _context;

        public UserRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }

        public List<User> GetAllUser()
        {
            var result = _context.Users.Select(x => new User
            {
                UserId = x.UserId,
                Name = x.Name,
                Username = x.Username,
                Password = x.Password,
                Address = x.Address,
                Phone = x.Phone,
                Role = x.Role,
            });
            return result.ToList();
        }

        public async Task<User> GetUsernamePassword(UserRequestModel userRequest)
        {

            var result = _context.Users.FirstOrDefault(x => x.Phone == userRequest.Phone);
            if (result == null) return null;

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userRequest.Password, result.Password);
            if (isPasswordValid) 
            {
                return new User
                {
                    Name = result.Name,
                    Username = result.Username,
                    Email = result.Email,
                    Phone = result.Phone,
                    Address = result.Address,
                    District = result.District,
                    Status = result.Status,
                    RoleId = result.RoleId
                };
            }
            return null;
        }

    }
}
