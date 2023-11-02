namespace HomeMealTaste.Services.Helper;

public class GetAllMealRequest : PagingParams
{
    public DateTime? SessionStartTime { get; set; }
}