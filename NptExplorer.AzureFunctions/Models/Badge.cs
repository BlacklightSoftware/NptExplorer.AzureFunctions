using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Badge
    {
        public Badge()
        {
            UserBadges = new HashSet<UserBadge>();
        }

        public int Id { get; set; }
        public int LocationId { get; set; }
        public int BadgeTypeId { get; set; }
        public int? PointOfInterestId { get; set; }
        public int? TrailId { get; set; }

        public virtual BadgeType BadgeType { get; set; }
        public virtual Location Location { get; set; }
        public virtual PointOfInterest PointOfInterest { get; set; }
        public virtual Trail Trail { get; set; }
        public virtual ICollection<UserBadge> UserBadges { get; set; }
    }
}
