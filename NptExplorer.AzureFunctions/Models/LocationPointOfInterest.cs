using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationPointOfInterest
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int PointOfInterestId { get; set; }

        public virtual Location Location { get; set; }
        public virtual PointOfInterest PointOfInterest { get; set; }
    }
}
