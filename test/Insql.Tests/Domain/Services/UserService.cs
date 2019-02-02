using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Models;
using System.Collections.Generic;

namespace Insql.Tests.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext dbContext;

        public UserService(UserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void DeleteUser(int userId)
        {
            this.dbContext.DeleteUser(userId);
        }

        public UserInfo GetUser(int userId)
        {
            return this.dbContext.GetUser(userId);
        }

        public IEnumerable<UserInfo> GetUserList(string userName, UserGender? userGender)
        {
            return this.dbContext.GetUserList(userName, userGender);
        }

        public void InsertUser(UserInfo info)
        {
            this.dbContext.InsertUser(info);
        }

        public void InsertUserSelective(UserInfo info)
        {
            this.dbContext.InsertUserSelective(info);
        }

        public void UpdateUser(UserInfo info)
        {
            this.dbContext.UpdateUser(info);
        }

        public void UpdateUserSelective(UserInfo info)
        {
            this.dbContext.UpdateUserSelective(info);
        }
    }
}
