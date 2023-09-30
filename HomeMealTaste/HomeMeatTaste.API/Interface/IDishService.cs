using HomeMealTaste.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Services.Interface
{
    public interface IDishService
    {
        Task<Dish> CreateDish(Dish dish);
    }
}
