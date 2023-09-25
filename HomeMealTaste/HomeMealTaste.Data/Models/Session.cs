using System;
using System.Collections.Generic;

namespace HomeMealTaste.Models
{
    public partial class Session
    {
        public Session()
        {
            FoodPackageSessions = new HashSet<FoodPackageSession>();
        }

        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual ICollection<FoodPackageSession> FoodPackageSessions { get; set; }
    }
}
