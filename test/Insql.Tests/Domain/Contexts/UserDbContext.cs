using Insql.Tests.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Tests.Domain.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public IEnumerable<UserInfo> GetUserList(string userName, UserGender? userGender)
        {
            return this.Query<UserInfo>(nameof(GetUserList), new { userName, userGender });
        }

        public IEnumerable<UserInfo> GetUserInList(string[] userNames)
        {
            return this.Query<UserInfo>(nameof(GetUserInList), new { userNames });
        }

        public UserInfo GetUser(int userId)
        {
            return this.Query<UserInfo>(nameof(GetUser), new { userId }).SingleOrDefault();
        }

        public void InsertUser(UserInfo info)
        {
            if (this.DbSession.SupportStatements)
            {
                var userId = this.ExecuteScalar<int>(nameof(InsertUser), info);

                info.UserId = userId;
            }
            else
            {
                //todo
            }
        }

        public void InsertUserSelective(UserInfo info)
        {
            var userId = this.ExecuteScalar<int>(nameof(InsertUserSelective), info);

            info.UserId = userId;
        }

        public void UpdateUser(UserInfo info)
        {
            this.Execute(nameof(UpdateUser), info);
        }

        public void UpdateUserSelective(UserInfo info)
        {
            this.Execute(nameof(UpdateUserSelective), info);
        }

        public void DeleteUser(int userId)
        {
            this.Execute(nameof(DeleteUser), new { userId });
        }

        public MapUserInfo GetMapUser(int userId)
        {
            return this.Query<MapUserInfo>(nameof(GetMapUser), new { userId }).SingleOrDefault();
        }
    }
}
