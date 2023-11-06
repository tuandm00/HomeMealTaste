using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Meal
    {
        public int MealId { get; set; }
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? DefaultPrice { get; set; }
        public int? KitchenId { get; set; }
    }
}
