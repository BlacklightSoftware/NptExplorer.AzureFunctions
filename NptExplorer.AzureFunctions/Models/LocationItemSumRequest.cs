using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NptExplorer.AzureFunctions.Models
{
    public class LocationItemSumRequest
    {
        public int Id { get; set; }
        public string? SelectedValue { get; set; }
        public string? Area { get; set; }
    }
}
