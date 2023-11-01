using HomeMealTaste.Data.ResponseModel;

namespace HomeMealTaste.API.Dto;

public class GetAllMealInCurrentSession
{
    public string? Name { get; set; }
    public string? Image { get; set; }
    public decimal? DefaultPrice { get; set; }
    public ICollection<GetAllMealInCurrentSessionResponseModel.MealModel> MealSessions { get; set; }
}