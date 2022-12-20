using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationFacility
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int FacilityId { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual Location Location { get; set; }
    }
}
