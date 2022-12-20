using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationBusRoute
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int BusRouteId { get; set; }

        public virtual BusRoute BusRoute { get; set; }
        public virtual Location Location { get; set; }
    }
}
