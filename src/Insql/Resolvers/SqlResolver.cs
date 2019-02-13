using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolver : ISqlResolver
    {
        private readonly InsqlDescriptor descriptor;
        private readonly IServiceProvider serviceProvider;
        private readonly IInsqlSectionMatcher sectionMatcher;
        private readonly IEnumerable<ISqlResolveFilter> resolveFilters;

        public SqlResolver(InsqlDescriptor descriptor, IServiceProvider serviceProvider)
        {
            this.descriptor = descriptor;
            this.serviceProvider = serviceProvider;

            this.sectionMatcher = serviceProvider.GetRequiredService<IInsqlSectionMatcher>();
            this.resolveFilters = serviceProvider.GetServices<ISqlResolveFilter>();
        }

        public ResolveResult Resolve(string sqlId, IDictionary<string, object> sqlParam, IDictionary<string, string> envParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            if (sqlParam == null)
            {
                sqlParam = new Dictionary<string, object>();
            }
            if (envParam == null)
            {
                envParam = new Dictionary<string, string>();
            }

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolving(this.descriptor, sqlId, sqlParam, envParam);
            }

            var insqlSection = this.sectionMatcher.Match(this.descriptor, sqlId, sqlParam, envParam);

            if (insqlSection == null)
            {
                throw new Exception($"insql sqlId : {sqlId} [InsqlSection] not found !");
            }

            var resolveContext = new ResolveContext
            {
                ServiceProvider = this.serviceProvider,
                InsqlDescriptor = this.descriptor,
                InsqlSection = insqlSection,
                Param = sqlParam,
                Environment = envParam
            };

            var resolveResult = new ResolveResult
            {
                Sql = insqlSection.Resolve(resolveContext),
                Param = resolveContext.Param
            };

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolved(this.descriptor, resolveContext, resolveResult);
            }

            return resolveResult;
        }
    }
}
