using System;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class DefaultLocation
    {
        public int Id { get; set; }
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}
