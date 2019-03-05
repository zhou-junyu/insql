using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Insql.Resolvers.Matchers
{
    internal class DefaultResolveMatcher : IInsqlResolveMatcher
    {
        private readonly IOptions<DefaultResolveMatcherOptions> options;

        public DefaultResolveMatcher(IOptions<DefaultResolveMatcherOptions> options)
        {
            this.options = options;
        }

        public IInsqlSection Match(InsqlDescriptor insqlDescriptor, string dbType, string sqlId, IDictionary<string, object> sqlParam)
        {
            var optionsValue = this.options.Value;

            IInsqlSection insqlSection;

            if (optionsValue.CorssDbEnabled)
            {
                if (!string.IsNullOrWhiteSpace(dbType))
                {
                    if (insqlDescriptor.Sections.TryGetValue($"{sqlId}{optionsValue.CorssDbSeparator}{dbType}", out insqlSection))
                    {
                        return insqlSection;
                    }
                }
            }

            if (insqlDescriptor.Sections.TryGetValue(sqlId, out insqlSection))
            {
                return insqlSection;
            }

            return null;
        }
    }
}
