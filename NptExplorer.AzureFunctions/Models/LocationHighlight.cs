#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class LocationHighlight
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public int Sequence { get; set; }
        public string HighlightEnglish { get; set; }
        public string HighlightWelsh { get; set; }

        public virtual Location Location { get; set; }
    }
}
