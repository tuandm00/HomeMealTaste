using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Session
    {
        public int SessionId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SessionName { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public string? SessionType { get; set; }
    }
}
