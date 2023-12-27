using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Session
    {
        public Session()
        {
            MealSessions = new HashSet<MealSession>();
            SessionAreas = new HashSet<SessionArea>();
        }

        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndDate { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public string? SessionType { get; set; }
        public string? SessionName { get; set; }
        public bool? RegisterForMealStatus { get; set; }
        public bool? BookingSlotStatus { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<MealSession> MealSessions { get; set; }
        public virtual ICollection<SessionArea> SessionAreas { get; set; }
    }
}
