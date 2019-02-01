using Example.Domain.Contexts;
using Insql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Example.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly DbContext dbContext;

        public UserService(SqliteDbContext<UserService> dbContext)
        {
            this.dbContext = dbContext;
        }

        public void DeleteUser(int userId)
        {
            this.dbContext.Execute(nameof(DeleteUser), new { userId });
        }

        public IEnumerable<UserInfo> GetUserList(string userName, Gender? userGender)
        {
            return this.dbContext.Query<UserInfo>(nameof(GetUserList), new { userName, userGender });
        }

        public void InsertUser(UserInfo info)
        {
            var userId = this.dbContext.ExecuteScalar<int>(nameof(InsertUser), info);

            info.UserId = userId;
        }

        public void InsertUserSelective(UserInfo info)
        {
            var userId = this.dbContext.ExecuteScalar<int>(nameof(InsertUserSelective), info);

            info.UserId = userId;
        }

        public void UpdateUser(UserInfo info)
        {
            this.dbContext.Execute(nameof(UpdateUser), info);
        }

        public void UpdateUserSelective(UserInfo info)
        {
            this.dbContext.Execute(nameof(UpdateUserSelective), info);
        }
    }
}
