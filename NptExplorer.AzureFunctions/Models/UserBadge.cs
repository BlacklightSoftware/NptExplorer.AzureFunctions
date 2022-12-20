#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class UserBadge
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BadgeId { get; set; }
        public bool CheckedIn { get; set; }

        public virtual Badge Badge { get; set; }
        public virtual User User { get; set; }
    }
}
