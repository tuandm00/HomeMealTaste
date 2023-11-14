using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.Implement
{
    public class KitchenRepository : BaseRepository<Kitchen>, IKitchenRepository
    {
        public KitchenRepository(HomeMealTasteContext context) : base(context)
        {
        }
    }
}
