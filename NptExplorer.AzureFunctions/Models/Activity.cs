#nullable disable

using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Activity
    {
        public Activity()
        {
            LocationActivities = new HashSet<LocationActivity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationActivity> LocationActivities { get; set; }
    }
}
