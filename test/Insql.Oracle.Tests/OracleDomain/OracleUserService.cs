using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Insql.Resolvers;
using Insql.Tests.Domain.Models;
using Oracle.ManagedDataAccess.Client;
using Xunit;

namespace Insql.Tests.OracleDomain
{
    public class OracleUserService : IOracleUserService
    {
        private readonly ISqlResolver<OracleUserService> sqlResolver;

        public OracleUserService(ISqlResolver<OracleUserService> sqlResolver)
        {
            this.sqlResolver = sqlResolver;
        }

        public void InsertUser(UserInfo userInfo)
        {
            var resolveResult = this.sqlResolver.Resolve(nameof(InsertUser), userInfo);

            using (var connection = new OracleConnection("user id=root;password=root;data source=//127.0.0.1:1521/XE"))
            {
                using (var results = connection.QueryMultiple(resolveResult.Sql, resolveResult.Param))
                {

                }

                //userInfo.UserId = results;
            }

        }
    }
}
