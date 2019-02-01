using Insql;
using System.Collections.Generic;

namespace Example.Domain.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public IEnumerable<UserInfo> GetUserList(string userName)
        {
            return this.Query<UserInfo>(nameof(GetUserList), new { userName, userGender = Gender.M });
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
