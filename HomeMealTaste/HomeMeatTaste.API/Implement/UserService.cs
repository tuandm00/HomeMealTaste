using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.Dto;
using HomeMealTaste.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Text;


namespace HomeMealTaste.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;


        public UserService(IUserRepository userRepository, IConfiguration config)
        {
            _userRepository = userRepository;
            _config = config;
        }

        private string GenerateToken(User users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<LoginDto> LoginAsync(User user)
        {
            var existedUser = await _userRepository.GetFirstOrDefault(x => x.Username == user.Username);
            var chekhash = BCrypt.Net.BCrypt.Verify(user.Password, existedUser?.Password);
            if (!chekhash) throw new Exception("Username or Password not match!");
            var result = await _userRepository.GetUsernamePassword(user);
            if(result != null)
            {
                return new LoginDto
                {
                    Name = result.Name,
                    UserId = result.UserId,
                    Role = result.Role,
                    Token = GenerateToken(result),
                };
            }

            return null;
        }

        public async Task<User> RegisterForCustomer(User user)
        {
            var existedUser = await _userRepository.GetByCondition(x => x.Username == user.Username);
            if (existedUser.Count() != 0) throw new Exception("existed Username");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Role = 2;
            var result = await _userRepository.Create(user, true);
            return result;
        }

        public async Task<User> RegisterForChef(User user)
        {
            var existedUser = await _userRepository.GetByCondition(x => x.Username == user.Username);
            if (existedUser.Count() != 0) throw new Exception("existed Username");
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Role = 3;
            var result = await _userRepository.Create(user, true);
            return result;
        }

        public async Task<User> DeleteUserById(int id)
        {
            if(id >= 0)
            {
                await _userRepository.Delete(id, false);
            }

            return null;
            
        }

        public List<User> GetAllUser()
        {
            var getall = _userRepository.GetAllUser();
            return getall;
        }
    }
}
