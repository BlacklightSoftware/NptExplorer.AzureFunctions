using System;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class BadgePoint
    {
        public int Id { get; set; }
        public string BadgeName { get; set; }
        public int Points { get; set; }
    }
}
