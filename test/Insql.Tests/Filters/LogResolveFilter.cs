using Insql.Resolvers;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace Insql.Tests.Filters
{
    public class LogResolveFilter : IInsqlResolveFilter
    {
        private readonly ILogger<LogResolveFilter> logger;

        public LogResolveFilter(ILogger<LogResolveFilter> logger)
        {
            this.logger = logger;
        }

        public void OnResolved(ResolveContext resolveContext, ResolveResult resolveResult)
        {
            this.logger.LogInformation($"insql resolved id : {resolveContext.InsqlSection.Id} , sql : {resolveResult.Sql}");
        }

        public void OnResolving(InsqlDescriptor insqlDescriptor, string sqlId, IDictionary<string, object> sqlParam)
        {
        }
    }
}
