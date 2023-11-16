//using System.Linq.Expressions;
//using HomeMealTaste.Data.Models;

//namespace HomeMealTaste.Data.ResponseModel;

//public class GetAllMealResponseModel
//{
//    public int Id { get; set; }
//    public string? Name { get; set; }
//    public string? Image { get; set; }
//    public decimal? DefaultPrice { get; set; }
//    public List<MealSession> MealSessions { get; set; }
//    public List<MealDish> MealDishes { get; set; }
    
//    public static Expression<Func<Meal, GetAllMealResponseModel>> FromEntity()
//    {
//        return (Meal entity) => new GetAllMealResponseModel()
//        {
//            Id = entity.MealId,
//            Image = entity.Image,
//            Name = entity.Name,
//            MealDishes = entity.MealDishes.ToList(),
//        };
//    }
//}