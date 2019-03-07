using Insql.Example.Model;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Example.Contexts
{
    public class UserDbContext
    {
        private readonly IDbContext<UserDbContext> insql;

        public UserDbContext(IDbContext<UserDbContext> insql)
        {
            this.insql = insql;
        }

        public UserPo GetUser(int userId)
        {
            return this.insql.Query<UserPo>(nameof(GetUser), new { userId }).SingleOrDefault();
        }

        public IEnumerable<UserPo> GetUsers()
        {
            return this.insql.Query<UserPo>(nameof(GetUsers));
        }
    }
}
