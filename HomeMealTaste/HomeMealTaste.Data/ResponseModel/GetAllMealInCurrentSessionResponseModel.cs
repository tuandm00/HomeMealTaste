using System.Linq.Expressions;
using HomeMealTaste.Data.Models;

namespace HomeMealTaste.Data.ResponseModel;

public class GetAllMealInCurrentSessionResponseModel
{
    public SessionModel? Session { get; set; }
    public ChefInfo? Chef { get; set; }
    public MealModel? Meal { get; set; }
    public List<DishModel?> Dish { get; set; }
    
    public static Expression<Func<Session, GetAllMealInCurrentSessionResponseModel>> FromSession()
    {
        return entity => new GetAllMealInCurrentSessionResponseModel()
        {
            Session = new SessionModel
            {
                Id = entity.SessionId,
                Type = entity.SessionType,
                CreatedDate = entity.CreateDate,
                EndDate = entity.EndDate,
                EndTime = entity.EndTime,
                SessionName = entity.SessionName,
                StartTime = entity.StartTime,
                Status = entity.Status,
            },
            Chef = entity.User!.Kitchens.Select(x => new ChefInfo()
            {
                Address = x.Address,
                Name = x.Name,
            }).FirstOrDefault(),
        };
    }
    
    public static Expression<Func<MealSession, GetAllMealInCurrentSessionResponseModel>> FromEntity()
    {
        Expression<Func<MealSession, GetAllMealInCurrentSessionResponseModel>> selectExpression = entity => new GetAllMealInCurrentSessionResponseModel()
        {
            Session = new SessionModel
            {
                Id = entity.Session.SessionId,
                Type = entity.Session.SessionType,
                CreatedDate = entity.Session.CreateDate,
                EndDate = entity.Session.EndDate,
                EndTime = entity.Session.EndTime,
                SessionName = entity.Session.SessionName,
                StartTime = entity.Session.StartTime,
                Status= entity.Session.Status,
            },
            Meal = new MealModel
            {
                Id = entity.Meal.MealId,
                Image = entity.Meal.Image,
                Price = entity.Meal.DefaultPrice,
                MealName = entity.Meal.Name,
            },
        };

        return selectExpression;
    }

    public class MealModel
    {
        public int Id { get; set; }
        public string? MealName { get; set; }
        public string? Image { get; set; }
        public decimal? Price { get; set; }
    }

    public class SessionModel
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? SessionName { get; set; }
        public bool? Status { get; set; }
        public string? Type { get; set; }
    }
    
    public class DishModel
    {
        public int DishId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public int? DishTypeId { get; set; }
        public int? KitchenId { get; set; }
    }

    public class ChefInfo
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
}

