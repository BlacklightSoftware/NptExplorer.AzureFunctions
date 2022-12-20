using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Facility
    {
        public Facility()
        {
            LocationFacilities = new HashSet<LocationFacility>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationFacility> LocationFacilities { get; set; }
    }
}
