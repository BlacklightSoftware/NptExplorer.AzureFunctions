using System.Linq;
using Microsoft.EntityFrameworkCore;
using NptExplorer.AzureFunctions.Context;
using NptExplorer.AzureFunctions.Models;
using NptExplorer.AzureFunctions.Repositories.Interfaces;
using NptExplorer.Dto.Requests;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Logging;

namespace NptExplorer.AzureFunctions.Repositories
{
    public class UsersRepository: RepositoryBase<NptExplorerContext, User>, IUsersRepository
    {
        private readonly NptExplorerContext _context;
        private readonly ILogger _logger;

        public UsersRepository(NptExplorerContext context) : base(context)
        {
            _context = context;
        }

        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            
            return true;
        }

		public User GetByUserId(string userId)
        {
            var dbSet = _context.Users.AsQueryable().Where(x => x.UserId == userId);
            return dbSet.FirstOrDefault();
        }
        
        public bool CheckUserExists(string name)
        {
            var dbSet = _context.Users.AsQueryable();
            var foundUser = dbSet.Any()? dbSet.Where(x => x.UserId == name):null;
            return foundUser != null && foundUser.Any();
        }

        public List<User> ExplorerLevel()
        {
            var dbSet = _context.Users.AsQueryable();
            var users = dbSet
                            .Include(x => x.UserFriendFriends)
                            .Include(x => x.UserFriendUsers)
                            .Include(x => x.UserBadges)
                            .ToList();
            return users;
        }

        public User GetUser(string userId)
        {
            var dbSet = _context.Users.AsQueryable();
            var user = dbSet
                            .Include(x => x.UserFriendFriends)
                            .Include(x => x.UserFriendUsers)
                            .Include(x => x.UserBadges)
                            .ThenInclude(x => x.Badge)
                            .FirstOrDefault(x => x.UserId == userId);
            return user;
        }

        public List<User> GetAllUsers()
        {
            var dbSet = _context.Users.AsQueryable();
            var users = dbSet
                        .Include(x => x.UserFriendFriends)
                        .Include(x => x.UserFriendUsers)
                        .Include(x => x.UserBadges)
                        .ThenInclude(x => x.Badge)
                        .Where(x => x.ExplorerBoard)
                        .ToList();

            return users;
        }

        public List<User> GetAllUsersPortal()
        {
            var dbSet = _context.Users.AsQueryable();
            var users = dbSet
                        .Include(x => x.UserFriendFriends)
                        .Include(x => x.UserFriendUsers)
                        .Include(x => x.UserBadges)
                        .ThenInclude(x => x.Badge)
                        .ToList();

            return users;
        }

        public void AmendFollower(UserRequest userData)
        {
            var dbSet = _context.UserFriends.AsQueryable();
            var user = dbSet.FirstOrDefault(x => x.UserId == int.Parse(userData.UserId) && x.FriendId == int.Parse(userData.FriendId));

            if (user != null)
            {
                _context.UserFriends.Remove(user);
                _context.SaveChanges();
            }
            else
            {
                var newUser = new UserFriend()
                {
                    UserId = int.Parse(userData.UserId),
                    FriendId = int.Parse(userData.FriendId)
                };
                _context.UserFriends.Add(newUser);
                _context.SaveChanges();
            }
        }

        public List<BadgeType> GetBadgeTypes()
        {
            var dbSet = _context.BadgeTypes.AsQueryable();
            var badgeTypes = dbSet.ToList();

            return badgeTypes;
        }

        public void UpdateBadgeType(BadgeType badgeType)
        {
            var dbSet = _context.BadgeTypes.AsQueryable();

            var foundBadgeType = dbSet.FirstOrDefault(b => b.Id == badgeType.Id);

            if(foundBadgeType != null)
            {
                foundBadgeType.Name = badgeType.Name;
                foundBadgeType.Points = badgeType.Points; 
                _context.BadgeTypes.Update(foundBadgeType);
                _context.SaveChanges();
            }
        }

        public List<BadgePoint> GetBadgeLevels()
        {
            var dbSet = _context.BadgePoints.AsQueryable();
            var badgeLevels = dbSet.ToList();

            return badgeLevels;
        }

        public void UpdateBadgeLevel(BadgePoint badgeLevel)
        {
            var dbSet = _context.BadgePoints.AsQueryable();

            var foundBadgeLevel = dbSet.FirstOrDefault(l => l.Id == badgeLevel.Id);

            if(foundBadgeLevel != null)
            {
                foundBadgeLevel.BadgeName = badgeLevel.BadgeName;
                foundBadgeLevel.Points = badgeLevel.Points;

                _context.BadgePoints.Update(foundBadgeLevel);
                _context.SaveChanges();
            }
        }

        public List<CategoryPoint> GetCategoryPoints()
        {
            var dbSet = _context.CategoryPoints.AsQueryable();
            var CategoryPoints = dbSet
                .Include(x => x.CategoryPointBadgeTypes)
                .ToList();
            return CategoryPoints;
        }

        public void UpdateCategoryPoints(CategoryPoint categoryPoint)
        {
            var dbSet = _context.CategoryPoints.AsQueryable();

            var foundCategoryPoint = dbSet.FirstOrDefault(l => l.Id == categoryPoint.Id);

            if (foundCategoryPoint != null)
            {
                foundCategoryPoint.Id = categoryPoint.Id;
                foundCategoryPoint.Adventurer = categoryPoint.Adventurer;
                foundCategoryPoint.Champion = categoryPoint.Champion;
                foundCategoryPoint.Hero = categoryPoint.Hero;
                foundCategoryPoint.Rockstar = categoryPoint.Rockstar;

                _context.CategoryPoints.Update(foundCategoryPoint);
                _context.SaveChanges();
            }
        }


        public void DeleteUser(string userId)
        {
            var user = GetByUserId(userId);
            if (user == null)
            {
                return;
            }

            var dbBadgeSet = _context.UserBadges.AsQueryable();
            var badges = dbBadgeSet.Where(x => x.UserId == user.Id);
            if (badges.Any())
            {
                _context.Set<UserBadge>().RemoveRange(badges);
            }

            var dbFriendSet = _context.UserFriends.AsQueryable();
            var friends = dbFriendSet.Where(x => x.FriendId == user.Id);
            if (friends.Any())
            {
                _context.Set<UserFriend>().RemoveRange(friends);
            }

            var userFriend = dbFriendSet.Where(x => x.UserId == user.Id);
            if (userFriend.Any())
            {
                _context.Set<UserFriend>().RemoveRange(userFriend);
            }

            _context.Set<User>().Remove(user);
            _context.SaveChanges();
        }

        public void UpdateExplorerBoard(int userId, bool include)
        {
            var dbSet = _context.Users.AsQueryable().Where(x => x.Id == userId);
            var user = dbSet.FirstOrDefault();

            if (user == null)
            {
                return;
            }

            user.ExplorerBoard = include;
            _context.Set<User>().Update(user);
            _context.SaveChanges();
        }
    }
}
