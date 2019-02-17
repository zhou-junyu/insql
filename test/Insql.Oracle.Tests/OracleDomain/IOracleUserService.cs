using Insql.Tests.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insql.Tests.OracleDomain
{
    public interface IOracleUserService
    {
        void InsertUser(UserInfo userInfo);
    }
}
