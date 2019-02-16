using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Insql.Resolvers.Matchers
{
    public class DefaultSectionMatcher : IInsqlSectionMatcher
    {
        private readonly IOptions<DefaultSectionMatcherOptions> options;

        public DefaultSectionMatcher(IOptions<DefaultSectionMatcherOptions> options)
        {
            this.options = options;
        }

        public IInsqlSection Match(InsqlDescriptor insqlDescriptor, ResolveEnviron resolveEnviron, string sqlId, IDictionary<string, object> sqlParam)
        {
            var optionsValue = this.options.Value;

            IInsqlSection insqlSection;

            if (optionsValue.CorssDbEnabled)
            {
                var dbType = resolveEnviron.GetDbType();

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
