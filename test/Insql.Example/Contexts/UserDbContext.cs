using Insql.Example.Model;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Example.Contexts
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public UserPo GetUser(int userId)
        {
            return this.Query<UserPo>(nameof(GetUser), new { userId }).SingleOrDefault();
        }

        public IEnumerable<UserPo> GetUsers()
        {
            return this.Query<UserPo>(nameof(GetUsers));
        }
    }
}
