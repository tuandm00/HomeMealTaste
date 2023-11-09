using HomeMealTaste.Data.Models;
using HomeMealTaste.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeMealTaste.Data.Implement
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(HomeMealTasteContext context) : base(context)
        {
        }
    }
}
