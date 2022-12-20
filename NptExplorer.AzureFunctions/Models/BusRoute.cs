using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class BusRoute
    {
        public BusRoute()
        {
            LocationBusRoutes = new HashSet<LocationBusRoute>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationBusRoute> LocationBusRoutes { get; set; }
    }
}
