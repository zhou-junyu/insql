using System;
using System.Collections.Generic;
using System.Text;

namespace Insql.Tests.Contexts
{
    public class SqliteDbContext : DbContext
    {
        public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options)
        {
        }


    }
}
