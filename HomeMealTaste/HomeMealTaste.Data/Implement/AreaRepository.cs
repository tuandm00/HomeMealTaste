using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.Implement
{
    public class AreaRepository : BaseRepository<Area>, IAreaRepository
    {
        public AreaRepository(HomeMealTasteContext context) : base(context)
        {
        }
    }
}
