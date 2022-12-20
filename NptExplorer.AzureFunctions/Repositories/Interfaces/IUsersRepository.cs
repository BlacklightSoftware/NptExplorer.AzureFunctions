using NptExplorer.AzureFunctions.Models;
using NptExplorer.Dto.Requests;
using System.Collections.Generic;

namespace NptExplorer.AzureFunctions.Repositories.Interfaces
{
    public interface IUsersRepository: IRepository
    {
        bool CheckUserExists(string user);
        bool AddUser(User request);
        List<User> ExplorerLevel();
        User GetUser(string userId);
        List<User> GetAllUsers();
        void AmendFollower(UserRequest userData);
        User GetByUserId(string userId);
        void DeleteUser(string userId);
        void UpdateExplorerBoard(int userId, bool include);
        List<BadgeType> GetBadgeTypes();
        void UpdateBadgeType(BadgeType badgeType);
        List<BadgePoint> GetBadgeLevels();
        void UpdateBadgeLevel(BadgePoint badgeLevel);
        List<CategoryPoint> GetCategoryPoints();
        void UpdateCategoryPoints(CategoryPoint categoryPoint);
        List<User> GetAllUsersPortal();
    }
}
