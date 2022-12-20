using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class BadgeType
    {
        public BadgeType()
        {
            Badges = new HashSet<Badge>();
            CategoryPointBadgeTypes = new HashSet<CategoryPointBadgeType>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Points { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }
        public virtual ICollection<CategoryPointBadgeType> CategoryPointBadgeTypes { get; set; }
    }
}
