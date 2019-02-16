using Insql.Resolvers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Insql.Tests.Filters
{
    public class LogResolveFilter : ISqlResolveFilter
    {
        private readonly ILogger<LogResolveFilter> logger;

        public LogResolveFilter(ILogger<LogResolveFilter> logger)
        {
            this.logger = logger;
        }

        public void OnResolved(InsqlDescriptor insqlDescriptor, ResolveContext resolveContext, ResolveResult resolveResult)
        {
            this.logger.LogInformation($"insql resolved id : {resolveContext.InsqlSection.Id} , sql : {resolveResult.Sql}");
        }

        public void OnResolving(InsqlDescriptor insqlDescriptor, ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam)
        {
        }
    }
}
