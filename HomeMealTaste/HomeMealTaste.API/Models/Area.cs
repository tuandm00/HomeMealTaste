﻿using System;
using System.Collections.Generic;

namespace HomeMealTaste.API.Models
{
    public partial class Area
    {
        public int AreaId { get; set; }
        public int? SessionId { get; set; }
        public int? GroupId { get; set; }
        public string? Address { get; set; }
        public string? Street { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }

        public virtual Session? Session { get; set; }
    }
}