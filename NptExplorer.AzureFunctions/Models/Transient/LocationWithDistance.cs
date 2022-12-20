using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Models.Transient
{
    public class LocationWithDistance
    {
        public Location Location { get; set; }
        public double Distance { get; set; }
    }
}
