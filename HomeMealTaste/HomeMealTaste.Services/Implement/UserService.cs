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
using System.Net.Mail;
using System.Net;
using AutoMapper;
using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using System.Security.Cryptography;

namespace HomeMealTaste.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly HomeMealTasteContext _context;
        private readonly IMapper _mapper;


        public UserService(IUserRepository userRepository, IConfiguration config, HomeMealTasteContext context, IMapper mapper)
        {
            _userRepository = userRepository;
            _config = config;
            _context = context;
            _mapper = mapper;
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
                    Address = result.Address,
                    District = result.District,
                    Email = result.Email,
                    Phone = result.Phone,
                    Status = result.Status,
                    RoleId = result.RoleId,
                    Token = GenerateToken(result),
                };
            }

            return null;
        }

        public async Task<UserRegisterCustomerResponseModel> RegisterForCustomer(UserRegisterCustomerRequestModel userRegisterCustomerRequest)
        {
            var entity = _mapper.Map<User>(userRegisterCustomerRequest);
            var existedUser = await _userRepository.GetByCondition(x => x.Username == entity.Username);
            if (existedUser.Count() != 0) throw new Exception("existed Username");
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            entity.RoleId = 2;
            var result = await _userRepository.Create(entity, true);
            if(result != null)
            {
                var customer = new Customer
                {
                    UserId = result.UserId,
                    Name = result.Name,
                    Phone = result.Phone,
                    Address = result.Address,
                    //Street = result.Street,
                    //Ward = result.Ward,
                    //District = result.District,
                    AccountStatus = true
                };
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserRegisterCustomerResponseModel>(result);
        }

        public async Task<UserRegisterChefResponseModel> RegisterForChef(UserRegisterChefRequestModel userRegisterChefRequest)
        {
            var entity = _mapper.Map<User>(userRegisterChefRequest);
            var existedUser = await _userRepository.GetByCondition(x => x.Username == entity.Username);
            if (existedUser.Count() != 0) throw new Exception("existed Username");
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            entity.RoleId = 3;
            var result = await _userRepository.Create(entity, true);
            if (result != null)
            {
                var chef = new Kitchen
                {
                    UserId = result.UserId,
                    Name = result.Name,
                    Phone = result.Phone,
                    Address = result.Address,
                    District = result.District,
                };
                await _context.AddAsync(chef);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserRegisterChefResponseModel>(result);
        }

        public async Task<User> DeleteUserById(int id)
        {
            if(id >= 0)
            {
                await _userRepository.Delete(id, false);
            }
            return null;
            
        }

        public async Task<PagedList<User>> GetAllUser(PagingParams pagingParams)
        {
            var result = await _userRepository.GetWithPaging(pagingParams);
            
            return result;
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
            Guid newGuid = Guid.NewGuid();
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(newGuid.ToByteArray());
                string shortGuid = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 8);
                return shortGuid;
            }
        }
        public async  Task<UserResponseForgetPasswordModel> ForgetPassword(string username)
        {
            var users = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            if (users != null)
            {
                var resetPassword = GenerateRandomResetPassword();

                var response = new UserResponseForgetPasswordModel
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

        public async Task UpdatePasswordAccount(string username, string newPassword)
        {
            var result = await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
            if (result != null)
                {
                    result.Password = BCrypt.Net.BCrypt.HashPassword(newPassword); ;
                    await _context.SaveChangesAsync();
                }
        }

        public async Task UpdateStatusUser(int userid)
        {
            var result = await _context.Users.FindAsync(userid);
            if (result != null && result.Status == true)
            {
                result.Status = false;
            }
            else result.Status = true;

            await _context.SaveChangesAsync();
        }
    }
}
