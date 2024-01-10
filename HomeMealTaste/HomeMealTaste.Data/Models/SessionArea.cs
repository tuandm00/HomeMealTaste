using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class SessionArea
    {
        public int SessionAreaId { get; set; }
        public int? SessionId { get; set; }
        public int? AreaId { get; set; }
        public bool? Status { get; set; }

        public virtual Area? Area { get; set; }
        public virtual Session? Session { get; set; }
    }
}
