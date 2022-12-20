using System;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class CategoryPoint
    {
        public CategoryPoint()
        {
            CategoryPointBadgeTypes = new HashSet<CategoryPointBadgeType>();
        }

        public int Id { get; set; }
        public int Adventurer { get; set; }
        public int Champion { get; set; }
        public int Hero { get; set; }
        public int Rockstar { get; set; }

        public virtual ICollection<CategoryPointBadgeType> CategoryPointBadgeTypes { get; set; }
    }
}
