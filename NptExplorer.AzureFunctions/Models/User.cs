#nullable disable
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Models
{
    public partial class User
    {
        public User()
        {
            UserBadges = new HashSet<UserBadge>();
            UserFriendFriends = new HashSet<UserFriend>();
            UserFriendUsers = new HashSet<UserFriend>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public bool ExplorerBoard { get; set; }

        public virtual ICollection<UserBadge> UserBadges { get; set; }
        public virtual ICollection<UserFriend> UserFriendFriends { get; set; }
        public virtual ICollection<UserFriend> UserFriendUsers { get; set; }
    }
}
