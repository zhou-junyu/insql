using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolver : ISqlResolver
    {
        private readonly InsqlDescriptor descriptor;
        private readonly IServiceProvider serviceProvider;

        public SqlResolver(InsqlDescriptor descriptor, IServiceProvider serviceProvider)
        {
            this.descriptor = descriptor;
            this.serviceProvider = serviceProvider;
        }

        public ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            //sqlId = this.MatchSqlId(sqlId);

            if (this.descriptor.Sections.TryGetValue(sqlId, out IInsqlSection insqlSection))
            {
                var resolveResult = new ResolveResult
                {
                    Param = sqlParam ?? new Dictionary<string, object>()
                };

                resolveResult.Sql = insqlSection.Resolve(new ResolveContext
                {
                    ServiceProvider = this.serviceProvider,
                    InsqlDescriptor = this.descriptor,
                    InsqlSection = insqlSection,
                    Param = resolveResult.Param
                });

                return resolveResult;
            }

            throw new Exception($"sqlId : {sqlId} [InsqlSection] not found !");
        }

        //private string MatchSection(string sqlId)
        //{
        //    var sqlIdSplit = sqlId.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

        //    //functionName
        //    if (sqlIdSplit.Length == 1)
        //    {
        //        return sqlIdSplit[0];
        //    }

        //    //functionName.serverName
        //    if (sqlIdSplit.Length == 2)
        //    {
        //        return $"{sqlIdSplit[0]}.{sqlIdSplit[1]}";
        //    }

        //    //functionName.serverName.serverVersion
        //    if (sqlIdSplit.Length == 3)
        //    {
        //        var fsName = $"{sqlIdSplit[0]}.{sqlIdSplit[1]}";

        //        var matchIds = this.descriptor.Sections.Keys.Where(key => key.StartsWith(fsName)).ToArray();


        //    }

        //    throw new Exception($"sqlId: { sqlId } format error !");
        //}
    }
}
