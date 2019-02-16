using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class SqlResolver : ISqlResolver
    {
        private readonly InsqlDescriptor insqlDescriptor;
        private readonly IServiceProvider serviceProvider;
        private readonly IInsqlSectionMatcher sectionMatcher;
        private readonly IEnumerable<ISqlResolveFilter> resolveFilters;

        public SqlResolver(InsqlDescriptor insqlDescriptor, IServiceProvider serviceProvider)
        {
            this.insqlDescriptor = insqlDescriptor;
            this.serviceProvider = serviceProvider;

            this.sectionMatcher = serviceProvider.GetRequiredService<IInsqlSectionMatcher>();
            this.resolveFilters = serviceProvider.GetServices<ISqlResolveFilter>();
        }

        public ResolveResult Resolve(ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam)
        {
            if (string.IsNullOrWhiteSpace(sqlId))
            {
                throw new ArgumentNullException(nameof(sqlId));
            }

            if (sqlParam == null)
            {
                sqlParam = new Dictionary<string, object>();
            }
            if (resolveEnviron == null)
            {
                resolveEnviron = new ResolveEnviron();
            }

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolving(this.insqlDescriptor, resolveEnviron, sqlId, sqlParam);
            }

            var insqlSection = this.sectionMatcher.Match(this.insqlDescriptor, resolveEnviron, sqlId, sqlParam);

            if (insqlSection == null)
            {
                throw new Exception($"insql sqlId : {sqlId} [InsqlSection] not found !");
            }

            var resolveContext = new ResolveContext
            {
                ServiceProvider = this.serviceProvider,
                InsqlDescriptor = this.insqlDescriptor,
                InsqlSection = insqlSection,
                Environ = resolveEnviron,
                Param = sqlParam,
            };

            var resolveResult = new ResolveResult
            {
                Sql = insqlSection.Resolve(resolveContext),
                Param = resolveContext.Param
            };

            foreach (var filter in this.resolveFilters)
            {
                filter.OnResolved(this.insqlDescriptor, resolveContext, resolveResult);
            }

            return resolveResult;
        }
    }
}
