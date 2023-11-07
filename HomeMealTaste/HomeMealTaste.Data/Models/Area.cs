using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Area
    {
        public Area()
        {
            Groups = new HashSet<Group>();
        }

        public int AreaId { get; set; }
        public int? SessionId { get; set; }
        public string? Address { get; set; }
        public string? District { get; set; }

        public virtual Session? Session { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
