﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class GetAllTransactionByUserIdResponseModel
    {
        public int TransactionId { get; set; }
        public int? UserId { get; set; }
        public OrderDtoTransactionResponse? OrderDtoTransactionResponse { get; set; }
        public WalletDtoTransactionResponse? WalletDtoTransactionResponse { get; set; }
        public string? Date { get; set; }
        public decimal? Amount { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? TransactionType { get; set; }

    }

    public class OrderDtoTransactionResponse
    {
        public int? OrderId { get; set; }
        public CustomerDtoTransaction? CustomerDtoTransaction { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? TotalPrice { get; set; }
        public string? Time { get; set; }
        public int? Quantity { get; set; }
    }

    public class WalletDtoTransactionResponse
    {
        public int? WalletId { get; set; }
        public int? UserId { get; set; }
        public int? Balance { get; set; }
    }

    public class CustomerDtoTransaction
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public int? DistrictId { get; set; }
        public int? AreaId { get; set; }
    }
}
