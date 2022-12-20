using System;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class CategoryPointBadgeType
    {
        public int Id { get; set; }
        public int BadgeTypeId { get; set; }
        public int CategoryPointId { get; set; }

        public virtual BadgeType BadgeType { get; set; }
        public virtual CategoryPoint CategoryPoint { get; set; }
    }
}
