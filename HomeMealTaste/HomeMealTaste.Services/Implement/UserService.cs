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
using System.Security.Claims;

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

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, users.UserId.ToString()),
                //new Claim(ClaimTypes.Name, users.Email),
                new Claim("RoleId", users.RoleId.ToString()),
                new Claim("Phone", users.Phone.ToString()),
                new Claim("Username", users.Username.ToString()),
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponseModel?> LoginAsync(UserRequestModel userRequest)
        {
            var existedUser = await _userRepository.GetFirstOrDefault(x => x.Phone == userRequest.Phone);
            var chekhash = BCrypt.Net.BCrypt.Verify(userRequest.Password, existedUser?.Password);
            if (!chekhash) throw new Exception("Username or Password not match!");
            var result = await _userRepository.GetUsernamePassword(userRequest);
            var customerIds = _context.Customers.Where(x => x.Phone == existedUser.Phone).Select(x => x.CustomerId).FirstOrDefault();
            var userIdOfCustomer = _context.Customers.Where(x => x.CustomerId == customerIds).Select(x => x.UserId).FirstOrDefault();
            var kitchenIds = _context.Kitchens.Where(x => x.UserId == existedUser.UserId).Select(x => x.KitchenId).FirstOrDefault();
            var userIdOfKitchen = _context.Kitchens.Where(x => x.KitchenId == kitchenIds).Select(x => x.UserId).FirstOrDefault();
            switch (result.RoleId)
            {
                case 1:
                    var adminWalletId = _context.Wallets
                        .Where(x => x.UserId == result.UserId)
                        .Select(x => x.WalletId)
                        .FirstOrDefault();
                    var adminBalance = _context.Wallets
                        .Where(x => x.UserId == result.UserId)
                        .Select(x => x.Balance)
                        .FirstOrDefault();
                    return new UserResponseModel
                    {
                        Name = result.Name,
                        UserId = result.UserId,
                        Address = result.Address,
                        DistrictId = result.DistrictId,
                        Email = result.Email,
                        Phone = result.Phone,
                        Status = result.Status,
                        RoleId = result.RoleId,
                        AreaId = result.AreaId,
                        Token = GenerateToken(result),
                        WalletDtoResponse = new WalletDtoResponse
                        {
                            WalletId = adminWalletId,
                            UserId = result.UserId,
                            Balance = adminBalance,
                        }
                    };
                case 2:
                    var CustomerWalletId = _context.Wallets
                        .Where(x => x.UserId == userIdOfCustomer)
                        .Select(x => x.WalletId)
                        .FirstOrDefault();
                    var CustomerBalance = _context.Wallets
                        .Where(x => x.UserId == userIdOfCustomer)
                        .Select(x => x.Balance)
                        .FirstOrDefault();
                    return new UserResponseModel
                    {
                        Name = result.Name,
                        UserId = result.UserId,
                        Address = result.Address,
                        DistrictId = result.DistrictId,
                        Email = result.Email,
                        Phone = result.Phone,
                        Status = result.Status,
                        RoleId = result.RoleId,
                        AreaId = result.AreaId,
                        Token = GenerateToken(result),
                        CustomerId = customerIds,
                        WalletDtoResponse = new WalletDtoResponse
                        {
                            WalletId = CustomerWalletId,
                            UserId = userIdOfCustomer,
                            Balance = CustomerBalance,
                        }
                    };
                case 3:
                    var kitchenWalletId = _context.Wallets
                        .Where(x => x.UserId == userIdOfKitchen)
                        .Select(x => x.WalletId)
                        .FirstOrDefault();
                    var chefBalance = _context.Wallets
                        .Where(x => x.UserId == userIdOfKitchen)
                        .Select(x => x.Balance)
                        .FirstOrDefault();
                    return new UserResponseModel
                    {
                        Name = result.Name,
                        UserId = result.UserId,
                        Address = result.Address,
                        DistrictId = result.DistrictId,
                        AreaId = result.AreaId,
                        Phone = result.Phone,
                        Email = result.Email,
                        Status = result.Status,
                        RoleId = result.RoleId,
                        Token = GenerateToken(result),
                        KitchenId = kitchenIds,
                        WalletDtoResponse = new WalletDtoResponse
                        {
                            WalletId = kitchenWalletId,
                            UserId = userIdOfKitchen,
                            Balance = chefBalance,
                        }
                    };
            }
            return null;
        }

        public async Task<UserRegisterCustomerResponseModel> RegisterForCustomer(UserRegisterCustomerRequestModel userRegisterCustomerRequest)
        {
            var entity = _mapper.Map<User>(userRegisterCustomerRequest);
            var existedUser = await _userRepository.GetByCondition(x => x.Phone == entity.Phone);
            if (existedUser.Count() != 0) throw new Exception("existed Phonenumber");
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            entity.RoleId = 2;
            entity.AreaId = userRegisterCustomerRequest.AreaId;
            var result = await _userRepository.Create(entity, true);
            if (result != null)
            {
                var customer = new Customer
                {
                    UserId = result.UserId,
                    Name = result.Name,
                    Phone = result.Phone,
                    DistrictId = result.DistrictId,
                };

                var wallet = new Wallet
                {
                    UserId = entity.UserId,
                    Balance = 0,
                };
                await _context.AddAsync(wallet);
                await _context.AddAsync(customer);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserRegisterCustomerResponseModel>(result);
        }

        public async Task<UserRegisterChefResponseModel> RegisterForChef(UserRegisterChefRequestModel userRegisterChefRequest)
        {
            var entity = _mapper.Map<User>(userRegisterChefRequest);
            var existedUser = await _userRepository.GetByCondition(x => x.Phone == entity.Phone);
            if (existedUser.Count() != 0) throw new Exception("existed Phone");
            entity.Password = BCrypt.Net.BCrypt.HashPassword(entity.Password);
            entity.RoleId = 3;
            entity.AreaId = userRegisterChefRequest.AreaId;
            entity.DistrictId = userRegisterChefRequest.DistrictId;
            entity.Status = true;
            var result = await _userRepository.Create(entity, true);
            if (result != null)
            {
                var chef = new Kitchen
                {
                    UserId = result.UserId,
                    Name = result.Name,
                    Address = result.Address,
                    AreaId = result.AreaId,
                    DistrictId = result.DistrictId,
                };
                var wallet = new Wallet
                {
                    UserId = entity.UserId,
                    Balance = 0,
                };
                await _context.AddAsync(wallet);
                await _context.AddAsync(chef);
                await _context.SaveChangesAsync();
            }


            var subject = "Welcome to Our HomeMealTaste Platform";
            var body = $"Thank you for registering as a chef! We are excited to have you on board.<br>" +
                       $"This is your Phone and Password to login:<br>" +
                       $"Phone: {userRegisterChefRequest.Phone}<br>" +
                       $"Password: {userRegisterChefRequest.Password}";

            await SendEmail(userRegisterChefRequest.Email, subject, body);
            return _mapper.Map<UserRegisterChefResponseModel>(result);
        }
        public async Task SendEmail(string? to, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("dominhtuan23102000@gmail.com", "djmr tgxz wfao upwq"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("dominhtuan23102000@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task<User> DeleteUserById(int id)
        {
            if (id >= 0)
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
                    Body = "Your new password is: " + resetPassword,
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
        public async Task<UserResponseForgetPasswordModel> ForgetPassword(string username)
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

        public async Task<GetUserByIdResponseModel> GetUserById(int id)
        {
            var result = await _context.Users
        .Include(x => x.Wallets)
        .Where(x => x.UserId == id).Select(x => new GetUserByIdResponseModel
        {
            UserId = x.UserId,
            Name = x.Name,
            Username = x.Username,
            Email = x.Email,
            Phone = x.Phone,
            Address = x.Address,
            DistrictDto = new DistrictDto
            {
                DistrictId = x.District.DistrictId,
                DistrictName = x.District.DistrictName,
            },
            RoleId = x.RoleId,
            Status = x.Status,
            AreaId = x.AreaId,
            WalletDto = x.Wallets.Select(w => new WalletDto
            {
                WalletId = w.WalletId,
                UserId = w.UserId,
                Balance = w.Balance,
            }).FirstOrDefault()
        }).FirstOrDefaultAsync();

            var mapped = _mapper.Map<GetUserByIdResponseModel>(result);
            return mapped;
        }

        public Task<List<GetAllUserWithRoleCustomerAndChefResponseModel>> GetAllUserWithRoleCustomerAndChef()
        {
            var result = _context.Users.Where(x => x.RoleId == 2 || x.RoleId == 3).Select(x => new GetAllUserWithRoleCustomerAndChefResponseModel
            {
                UserId = x.UserId,
                Name = x.Name,
                Username = x.Username,
                Email = x.Email,
                Phone = x.Phone,
                Address = x.Address,
                DistrictId = x.DistrictId,
                RoleId = x.RoleId,
                Status = x.Status,
                AreaId = x.AreaId,
            });

            var mapped = result.Select(x => _mapper.Map<GetAllUserWithRoleCustomerAndChefResponseModel>(x)).ToList();
            return Task.FromResult(mapped);
        }

        public async Task<int> TotalAccountInSystem()
        {
            int userCount = await _context.Users.CountAsync();
            return userCount;

        }

        public async Task<int> CountAllUserWithRoleId2()
        {
            int userCountWithRole2 = await _context.Users.Where(x => x.RoleId == 2).CountAsync();
            return userCountWithRole2;
        }

        public async Task<UpdateUserResponseModel> UpdateProfileChef(UpdateUserRequestModel request)
        {
            var entity = _mapper.Map<User>(request);
            var result = _context.Users.Where(x => x.UserId == request.UserId).FirstOrDefault();
            var kitchenId = _context.Kitchens.Where(x => x.UserId == result.UserId).Select(x => x.KitchenId).FirstOrDefault();
            if (result != null)
            {
                result.UserId = entity.UserId;
                result.Name = entity.Name;
                result.Username = entity.Username;
                result.Email = entity.Email;
                result.Address = entity.Address;
                result.DistrictId = entity.DistrictId;

                _context.Users.Update(result);

                var kitchenTable = new Kitchen
                {
                    KitchenId = kitchenId,
                    UserId = result.UserId,
                    Name = result.Name,
                    Address = result.Address,
                    AreaId = result.AreaId,
                    DistrictId = result.DistrictId,
                };
                _context.Kitchens.Update(kitchenTable);
            }
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<UpdateUserResponseModel>(result);
            mapped.KitchenId = kitchenId;
            return mapped;
        }
        public async Task<UpdateUserResponseModel> UpdateProfileCustomer(UpdateUserRequestModel request)
        {
            var entity = _mapper.Map<User>(request);
            var result = _context.Users.Where(x => x.UserId == request.UserId).FirstOrDefault();
            var customerId = _context.Customers.Where(x => x.UserId == result.UserId).Select(x => x.CustomerId).FirstOrDefault();
            if (result != null)
            {
                result.UserId = entity.UserId;
                result.Name = entity.Name;
                result.Username = entity.Username;
                result.Email = entity.Email;
                result.DistrictId = entity.DistrictId;

                _context.Users.Update(result);

                var customerTable = new Customer
                {
                    CustomerId = customerId,
                    UserId = result.UserId,
                    Name = result.Name,
                    AreaId = result.AreaId,
                    DistrictId = result.DistrictId,
                };
                _context.Customers.Update(customerTable);
            }
            await _context.SaveChangesAsync();
            var mapped = _mapper.Map<UpdateUserResponseModel>(result);
            mapped.KitchenId = customerId;
            return mapped;
        }
    }
}
