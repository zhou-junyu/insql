using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql.Resolvers.Elements
{
    public class IncludeSectionElement : IInsqlSectionElement
    {
        public string RefId { get; }

        public IncludeSectionElement(string refid)
        {
            if (string.IsNullOrWhiteSpace(refid))
            {
                throw new ArgumentNullException(nameof(refid));
            }

            this.RefId = refid;
        }

        public string Resolve(ResolveContext context)
        {
            var sectionMatcher = context.ServiceProvider.GetRequiredService<IInsqlSectionMatcher>();

            var insqlSection = sectionMatcher.Match(context.InsqlDescriptor, this.RefId, context.Param, context.Environment);

            if (insqlSection == null)
            {
                throw new ArgumentException($"insql section : {this.RefId} not found !");
            }

            return insqlSection.Resolve(new ResolveContext
            {
                ServiceProvider = context.ServiceProvider,
                InsqlDescriptor = context.InsqlDescriptor,
                InsqlSection = insqlSection,
                Param = context.Param
            });
        }
    }
}
