using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationHabitat
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int HabitatId { get; set; }

        public virtual Habitat Habitat { get; set; }
        public virtual Location Location { get; set; }
    }
}
