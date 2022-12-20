#nullable disable

using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Time
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Trail> Trails { get; set; }

        public Time()
        {
            Trails = new HashSet<Trail>();
        }
    }
}
