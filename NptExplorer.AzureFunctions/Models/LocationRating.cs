using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationRating
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public bool Rating { get; set; }
        public DateTime RatedDate { get; set; }

        public virtual Location Location { get; set; }
    }
}
