using System;
using System.Collections.Generic;

namespace Insql.Resolvers
{
    public class InsqlDescriptor
    {
        public Type Type { get; }

        public Dictionary<string, IInsqlSection> Sections { get; }

        public Dictionary<Type, IInsqlMapSection> Maps { get; }

        public InsqlDescriptor(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Type = type;
            this.Sections = new Dictionary<string, IInsqlSection>();
            this.Maps = new Dictionary<Type, IInsqlMapSection>();
        }

        public InsqlDescriptor(string typeName)
        {
            var type = Type.GetType(typeName);

            if (type == null)
            {
                throw new Exception($"insql type : {typeName} not found !");
            }

            this.Type = type;
            this.Sections = new Dictionary<string, IInsqlSection>();
            this.Maps = new Dictionary<Type, IInsqlMapSection>();
        }
    }
}
