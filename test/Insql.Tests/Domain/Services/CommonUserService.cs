using Insql.Tests.Domain.Contexts;
using Insql.Tests.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Tests.Domain.Services
{
    public class CommonUserService : ICommonUserService
    {
        private readonly CommonDbContext<CommonUserService> dbContext;

        public CommonUserService(CommonDbContext<CommonUserService> dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<UserInfo> GetUserList(string userName, UserGender? userGender)
        {
            return this.dbContext.Query<UserInfo>(nameof(GetUserList), new { userName, userGender });
        }

        public UserInfo GetUser(int userId)
        {
            return this.dbContext.Query<UserInfo>(nameof(GetUser), new { userId }).SingleOrDefault();
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

        public void DeleteUser(int userId)
        {
            this.dbContext.Execute(nameof(DeleteUser), new { userId });
        }
    }
}
