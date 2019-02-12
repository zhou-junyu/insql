using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Resolvers.Sections
{
    public class SqlSection : IInsqlSection
    {
        public string Id { get; }

        public List<ISqlSectionElement> Elements { get; }

        public SqlSection(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
            this.Elements = new List<ISqlSectionElement>();
        }

        public string Resolve(ResolveContext context)
        {
            var elementsResult = this.Elements.Select(element =>
            {
                return element.Resolve(context);
            });

            return string.Join(" ", elementsResult).Trim();
        }
    }
}
