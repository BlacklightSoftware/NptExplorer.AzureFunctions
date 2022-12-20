#nullable disable

using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class Distance
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Trail> Trails { get; set; }

        public Distance()
        {
            Trails = new HashSet<Trail>();
        }
    }
}
