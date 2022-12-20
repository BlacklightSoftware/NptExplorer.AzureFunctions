using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Difficulty
    {
        public Difficulty()
        {
            Trails = new HashSet<Trail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Trail> Trails { get; set; }
    }
}
