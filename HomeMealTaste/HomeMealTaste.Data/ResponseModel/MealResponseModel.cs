﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.ResponseModel
{
    public class MealResponseModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
        public decimal? DefaultPrice { get; set; }
    }
}