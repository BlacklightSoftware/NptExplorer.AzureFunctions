using System;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class TrophyPoint
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
    }
}
