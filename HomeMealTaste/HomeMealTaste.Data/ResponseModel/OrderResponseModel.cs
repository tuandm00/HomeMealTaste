﻿using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class OrderResponseModel
    {
        public int OrderId { get; set; }
        public string? Date { get; set; }
        public CustomerDto1? CustomerDto1 { get; set; }
        public string? Status { get; set; }
        public MealSessionDto1? MealSessionDto1 { get; set; }
        public int? Price { get; set; }
    }

    public class CustomerDto1
    {
        public int CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? District { get; set; }
        public int? AreaId { get; set; }
    }

    public class MealSessionDto1
    {
        public int MealSessionId { get; set; }
        public MealDto1? MealDto1 { get; set; }
        public SessionDto1? SessionDto1 { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public int? RemainQuantity { get; set; }
        public bool? Status { get; set; }
        public string? CreateDate { get; set; }
        public int? KitchenId { get; set; }
    }

    public class MealDto1
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? KitchenId { get; set; }
        public string? CreateDate { get; set; }
    }
     public class SessionDto1
    {
        public int SessionId { get; set; }
        public string? CreateDate { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public int? AreaId { get; set; }
    }
}
