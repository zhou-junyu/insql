using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Resolvers
{
    public class InsqlSection
    {
        public string Id { get; }

        public List<IInsqlSectionElement> Elements { get; }

        public InsqlSection(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
            this.Elements = new List<IInsqlSectionElement>();
        }

        public virtual string Resolve(ResolveContext context)
        {
            var elementsResult = this.Elements.Select(element =>
            {
                return element.Resolve(context);
            });

            return string.Join(" ", elementsResult).Trim();
        }
    }
}
