using System;
using System.Collections.Generic;

#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Habitat
    {
        public Habitat()
        {
            LocationHabitats = new HashSet<LocationHabitat>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<LocationHabitat> LocationHabitats { get; set; }
    }
}
