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

            if (this.descriptor.Sections.TryGetValue(sqlId, out InsqlSection insqlSection))
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
    }
}
