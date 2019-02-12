using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class InsqlDescriptor
    {
        public Type Type { get; }

        public Dictionary<string, IInsqlSection> Sections { get; }

        public InsqlDescriptor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Type = type;
            this.Sections = new Dictionary<string, IInsqlSection>();
        }
    }
}
