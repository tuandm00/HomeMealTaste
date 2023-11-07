using HomeMealTaste.Data.Helper;
using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.RequestModel;
using HomeMealTaste.Data.ResponseModel;
using HomeMealTaste.Services.Helper;
using HomeMealTaste.Services.ResponseModel;

namespace HomeMealTaste.Services.Interface
{
    public interface IUserService
    {
        Task<UserResponseModel> LoginAsync(UserRequestModel user);
        Task<UserRegisterCustomerResponseModel> RegisterForCustomer(UserRegisterCustomerRequestModel user);
        Task<UserRegisterChefResponseModel> RegisterForChef(UserRegisterChefRequestModel userRegisterChefRequest);
        Task<User> DeleteUserById(int id);
        Task<PagedList<User>> GetAllUser(PagingParams pagingParams);
        Task<UserResponseForgetPasswordModel> ForgetPassword(string username);
        Task UpdatePasswordAccount(string username, string newPassword);
        Task UpdateStatusUser(int userid);
        Task<User> GetUserById(int id);
    }
}
