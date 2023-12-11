using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllTransactionByTransactionTypeRECHARGED
    {
        public int TransactionId { get; set; }
        public int? OrderId { get; set; }
        public WalletDtoGetAllTransactionRECHARGED? WalletDtoGetAllTransactionRECHARGED { get; set; }
        public string? Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
    }
    public class WalletDtoGetAllTransactionRECHARGED
    {
        public int? WalletId { get; set; }
        public UserDtoGetAllTransactionRECHARGED? UserDtoGetAllTransactionRECHARGED { get; set; }
        public int? Balance { get; set; }
    }
    public class UserDtoGetAllTransactionRECHARGED
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public bool? Status { get; set; }
        public int? AreaId { get; set; }
    }
}
