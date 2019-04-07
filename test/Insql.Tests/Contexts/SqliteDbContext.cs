using Insql.Models.ModelOne;
using System.Linq;

namespace Insql.Tests.Contexts
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }

        public void Insert(DbContextTestInfo info)
        {
            var id = this.ExecuteScalar<int>(nameof(Insert), info);

            info.Id = id;
        }

        public DbContextTestInfo SelectById(int id)
        {
            return this.Query<DbContextTestInfo>(nameof(SelectById), new { id }).SingleOrDefault();
        }
    }
}
