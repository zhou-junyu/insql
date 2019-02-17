using Dapper;
using Insql.Resolvers;
using Insql.Tests.Domain.Models;
using Oracle.ManagedDataAccess.Client;

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
            var insertResolveResult = this.sqlResolver.Resolve("InsertUser", userInfo);
            var selectResolveResult = this.sqlResolver.Resolve("SelectInsertUserId");

            using (var connection = new OracleConnection("user id=root;password=root;data source=//127.0.0.1:1521/XE"))
            {
                connection.Open();
                var tt = connection.BeginTransaction();

                connection.Execute(insertResolveResult.Sql, insertResolveResult.Param);
                var userId = connection.ExecuteScalar<int>(selectResolveResult.Sql, selectResolveResult.Param);

                tt.Commit();

                userInfo.UserId = userId;
            }
        }
    }
}
