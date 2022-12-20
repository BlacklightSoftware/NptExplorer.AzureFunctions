using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class PointOfInterest
    {
        public PointOfInterest()
        {
            Badges = new HashSet<Badge>();
            LocationPointOfInterests = new HashSet<LocationPointOfInterest>();
        }

        public int Id { get; set; }
        public string NameEnglish { get; set; }
        public string NameWelsh { get; set; }
        public string Image { get; set; }
        public string DescriptionEnglish { get; set; }
        public string DescriptionWelsh { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }
        public virtual ICollection<LocationPointOfInterest> LocationPointOfInterests { get; set; }
    }
}
