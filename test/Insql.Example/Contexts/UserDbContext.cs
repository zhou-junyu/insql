using Insql.Example.Model;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Example.Contexts
{
    public class UserDbContext
    {
        private readonly IInsql<UserDbContext> insql;

        public UserDbContext(IInsql<UserDbContext> insql)
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
