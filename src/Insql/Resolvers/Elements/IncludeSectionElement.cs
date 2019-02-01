using System;

namespace Insql.Resolvers.Elements
{
    public class IncludeSectionElement : ISqlSectionElement
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
            if (!context.InsqlDescriptor.Sections.ContainsKey(this.RefId))
            {
                throw new ArgumentException($"mapper section id : {this.RefId} not found !");
            }

            IInsqlSection sectionDescriptor = context.InsqlDescriptor.Sections[this.RefId];

            return (string)sectionDescriptor.Resolve(new ResolveContext
            {
                ServiceProvider = context.ServiceProvider,
                InsqlDescriptor = context.InsqlDescriptor,
                SectionDescriptor = sectionDescriptor,
                Param = context.Param
            });
        }
    }

}
