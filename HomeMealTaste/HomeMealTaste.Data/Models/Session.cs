using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Session
    {
        public Session()
        {
            MealSessions = new HashSet<MealSession>();
        }

        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
        public int? AreaId { get; set; }

        public virtual Area? Area { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<MealSession> MealSessions { get; set; }
    }
}
