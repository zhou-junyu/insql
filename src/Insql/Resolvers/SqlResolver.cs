using System;
using System.Collections.Generic;
using System.Linq;

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

            //this.MatchSection(sqlId);

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

        //private IInsqlSection MatchSection(string sqlId)
        //{
        //    if (this.descriptor.Sections.TryGetValue(sqlId, out IInsqlSection insqlSection))
        //    {
        //        return insqlSection;
        //    }




        //    return null;
        //}
    }

    //internal class SegmentSqlId
    //{
    //    public string IdName { get; set; }

    //    public string ServerName { get; set; }

    //    public int? ServerVersion { get; set; }

    //    public SegmentSqlId(string sqlId)
    //    {
    //        var sqlIdSplit = sqlId.Split(new char[] { '.' }, StringSplitOptions.None);

    //        if (sqlIdSplit.Length < 3)
    //        {
    //            throw new Exception($"sqlId : {sqlId} format error !");
    //        }

    //        if (int.TryParse(sqlIdSplit[sqlIdSplit.Length - 1], out int serverVersion))
    //        {
    //            this.ServerVersion = serverVersion;
    //        }
    //        this.ServerName = sqlIdSplit[sqlIdSplit.Length - 2];
    //        this.IdName = sqlIdSplit[sqlIdSplit.Length - 3];
    //    }
    //}
}
