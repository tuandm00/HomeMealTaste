using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class KitchenResponseModel
    {
        public int KitchenId { get; set; }
        public UserDtoKitchenResponseModel? UserDtoKitchenResponseModel { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
    }

    public class UserDtoKitchenResponseModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public WalletDtoKitchenResponseModel? WalletDtoKitchenResponseModel { get; set; }

    }
    public class WalletDtoKitchenResponseModel
    {
        public int WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
    }
}
