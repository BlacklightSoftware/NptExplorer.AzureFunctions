#nullable disable

namespace NptExplorer.AzureFunctions.Models
{
    public partial class UserFriend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FriendId { get; set; }

        public virtual User Friend { get; set; }
        public virtual User User { get; set; }
    }
}
