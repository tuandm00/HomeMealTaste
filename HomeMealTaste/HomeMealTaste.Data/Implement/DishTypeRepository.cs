using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.Implement
{
    public class DishTypeRepository : BaseRepository<DishType>, IDishTypeRepository
    {
        private readonly HomeMealTasteContext _context;
        public DishTypeRepository(HomeMealTasteContext context) : base(context)
        {
            _context = context;
        }


    }
}
