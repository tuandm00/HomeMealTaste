using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.RequestModel
{
    public class CreateOrderRequestModel
    {
        public int? CustomerId { get; set; }
        public string? Status { get; set; }
        public int? MealSessionId { get; set; }
        public int? SessionId { get; set; }
        public int? Price { get; set; }
        public DateTime? Time { get; set; }


    }
}

public class CustomerDtoRequest
{
    public int CustomerId { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public string? District { get; set; }
    public int? AreaId { get; set; }
}

public class MealSessionDtoRequest
{
    public int MealSessionId { get; set; }
    public MealDtoRequest? MealDtoRequest { get; set; }
    public SessionDtoRequest? SessionDtoRequest { get; set; }
    public double? Price { get; set; }
    public int? Quantity { get; set; }
    public int? RemainQuantity { get; set; }
    public bool? Status { get; set; }
    public DateTime? CreateDate { get; set; }
    public KitchenDtoRequest? KitchenDtoRequest { get; set; }
}

public class MealDtoRequest
{
    public int MealId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public KitchenDtoRequest? KitchenDtoRequest { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? Description { get; set; }
    public MealDishDtoRequest? MealDishDtoRequest { get; set; }
}

public class SessionDtoRequest
{
    public int SessionId { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public DateTime? EndDate { get; set; }
    public int? UserId { get; set; }
    public bool? Status { get; set; }
    public string? SessionType { get; set; }
    public int? AreaId { get; set; }
}

public class KitchenDtoRequest
{
    public int KitchenId { get; set; }
    public int? UserId { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? District { get; set; }
    public int? AreaId { get; set; }
}

public class DishDtoRequest
{
    public int DishId { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public int? DishTypeId { get; set; }
    public KitchenDtoRequest? KitchenDtoRequest { get; set; }
}

public class MealDishDtoRequest
{
    public int MealDishId { get; set; }
    public int? MealId { get; set; }
    public int? DishId { get; set; }
    public List<DishDtoRequest?> DishDtoRequest { get; set; }
}


