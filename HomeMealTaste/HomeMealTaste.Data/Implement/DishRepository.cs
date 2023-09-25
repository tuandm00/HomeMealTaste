using HomeMealTaste.Data.Repositories;
using HomeMealTaste.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.Implement
{
    public class DishRepository : BaseRepository<Dish>,IDishRepository
    {
        private readonly HomeMealTasteContext _context;

        public DishRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }
    }
}
