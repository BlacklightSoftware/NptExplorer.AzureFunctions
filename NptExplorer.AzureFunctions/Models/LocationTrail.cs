using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationTrail
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int TrailId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Trail Trail { get; set; }
    }
}
