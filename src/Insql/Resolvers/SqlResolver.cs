using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolver : ISqlResolver
    {
        private readonly InsqlDescriptor insqlDescriptor;
        private readonly IServiceProvider serviceProvider;
        private readonly ISqlResolveMatcher resolveMatcher;
        private readonly IEnumerable<ISqlResolveFilter> resolveFilters;

        public SqlResolver(InsqlDescriptor insqlDescriptor, IServiceProvider serviceProvider, ISqlResolveMatcher resolveMatcher, IEnumerable<ISqlResolveFilter> resolveFilters)
        {
            this.insqlDescriptor = insqlDescriptor;
            this.serviceProvider = serviceProvider;
            this.resolveMatcher = resolveMatcher;
            this.resolveFilters = resolveFilters;
        }

        public ResolveResult Resolve(string dbType, string sqlId, IDictionary<string, object> sqlParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            if (sqlParam == null)
            {
                sqlParam = new Dictionary<string, object>();
            }

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolving(this.insqlDescriptor, dbType, sqlId, sqlParam);
            }

            var insqlSection = this.resolveMatcher.Match(this.insqlDescriptor, dbType, sqlId, sqlParam);

            if (insqlSection == null)
            {
                throw new Exception($"insql sqlId : {sqlId} [InsqlSection] not found !");
            }

            var resolveContext = new ResolveContext
            {
                ServiceProvider = this.serviceProvider,
                InsqlDescriptor = this.insqlDescriptor,
                InsqlSection = insqlSection,
                Param = sqlParam,
            };

            var resolveResult = new ResolveResult
            {
                Sql = insqlSection.Resolve(resolveContext),
                Param = resolveContext.Param
            };

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolved(resolveContext, resolveResult);
            }

            return resolveResult;
        }
    }
}
