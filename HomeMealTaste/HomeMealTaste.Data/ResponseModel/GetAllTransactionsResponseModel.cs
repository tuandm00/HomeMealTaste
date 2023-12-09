using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllTransactionsResponseModel
    {
        public int? TransactionId { get; set; }
        public OrderDtoGetAllTransactions? OrderDtoGetAllTransactions { get; set; }
        public WalletDtoGetAllTransactions? WalletDtoGetAllTransactions { get; set; }
        public string? Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }
        public UserDtoGetAllTransactions? UserDtoGetAllTransactions { get; set; }
    }
    public class WalletDtoGetAllTransactions
    {
        public int? WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
    }
    public class UserDtoGetAllTransactions
    {
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int? DistrictId { get; set; }
        public int? RoleId { get; set; }
        public int? AreaId { get; set; }
    }

    public class OrderDtoGetAllTransactions
    {
        public int? OrderId { get; set; }
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }
    }
}
