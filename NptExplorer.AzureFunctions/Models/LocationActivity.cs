

#nullable disable

using System;
using System.Collections.Generic;
using NptExplorer.AzureFunctions.Models;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationActivity
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int ActivityId { get; set; }

        public virtual Activity Activity { get; set; }
        public virtual Location Location { get; set; }
    }
}
