using System;
using System.Collections.Generic;

namespace HomeMealTaste.Data.Models
{
    public partial class Sysdiagram
    {
        public string Name { get; set; } = null!;
        public int PrincipalId { get; set; }
        public int DiagramId { get; set; }
        public int? Version { get; set; }
        public byte[]? Definition { get; set; }
    }
}
