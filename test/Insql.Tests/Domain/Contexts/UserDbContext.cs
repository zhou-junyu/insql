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

        public UserInfo GetUser(int userId)
        {
            return this.Query<UserInfo>(nameof(GetUser), new { userId }).SingleOrDefault();
        }

        public void InsertUser(UserInfo info)
        {
            var userId = this.ExecuteScalar<int>(nameof(InsertUser), info);

            info.UserId = userId;
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
    }
}
