using System;

namespace Insql.Resolvers.Elements
{
    public class ColumnMapSectionElement : IInsqlMapSectionElement
    {
        public string Name { get; }

        public string To { get; }

        public ColumnMapSectionElement(string name, string to)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(to))
            {
                throw new ArgumentNullException(nameof(to));
            }

            this.Name = name;
            this.To = to;
        }
    }
}
