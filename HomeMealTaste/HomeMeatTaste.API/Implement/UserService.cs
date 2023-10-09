using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Services.ResponseModel;
using HomeMealTaste.Services.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;

using System.Text;
using HomeMealTaste.Data.RequestModel;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using System.Net.Mail;
using System.Net;

namespace HomeMealTaste.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly HomeMealTasteContext _context;


        public UserService(IUserRepository userRepository, IConfiguration config, HomeMealTasteContext context)
        {
            _userRepository = userRepository;
            _config = config;
            _context = context;
        }

        private string GenerateToken(User users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponseModel> LoginAsync(UserRequestModel userRequest)
        {
            var existedUser = await _userRepository.GetFirstOrDefault(x => x.Username == userRequest.Username);
            var chekhash = BCrypt.Net.BCrypt.Verify(userRequest.Password, existedUser?.Password);
            if (!chekhash) throw new Exception("Username or Password not match!");
            var result = await _userRepository.GetUsernamePassword(userRequest);
            if(result != null)
            {
                return new UserResponseModel
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

        private async Task SendResetPasswordToEmail(string userEmail, string resetPassword)
        {
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com"))
            {
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("dominhtuan23102000@gmail.com", "djmr tgxz wfao upwq"); // password use app pasword in gmeow
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("dominhtuan23102000@gmail.com"),
                    Subject = resetPassword,
                    Body = "Your new password is: " +resetPassword,
                    IsBodyHtml = false
                };
                mailMessage.To.Add(userEmail);
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        private string GenerateRandomResetPassword()
        {
            return Guid.NewGuid().ToString();
        }
        public async  Task<User> ForgetPassword(string user)
        {
            var users = await _context.Users.Where(u => u.Username == user).FirstOrDefaultAsync();
            if (user != null)
            {
                var resetPassword = GenerateRandomResetPassword();

                var response = new User
                {
                    Password = resetPassword
                };
                var hash = BCrypt.Net.BCrypt.HashPassword(response.Password);
                users.Password = hash;

                await _context.SaveChangesAsync();
                await SendResetPasswordToEmail(users.Email, resetPassword);
                return response;
            }
            else
            {
                throw new Exception("Username not exist");
            }

        }
    }
}
