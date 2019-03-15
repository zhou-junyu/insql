using System;
using System.Collections.Generic;
using System.Text;

namespace Insql
{
    public class DbContextOptions<T> : DbContextOptions
        where T : class
    {
    }
}
