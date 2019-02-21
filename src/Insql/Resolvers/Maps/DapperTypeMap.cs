using Dapper;
using System;
using System.Reflection;

namespace Insql.Resolvers
{
    internal class DapperTypeMap : SqlMapper.ITypeMap
    {
        private readonly CustomPropertyTypeMap typeMap;
        private readonly IInsqlMapSection mapSection;

        public DapperTypeMap(IInsqlMapSection mapSection)
        {
            this.mapSection = mapSection;

            this.typeMap = new CustomPropertyTypeMap(mapSection.Type, this.GetProperty);
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return this.typeMap.FindConstructor(names, types);
        }

        public ConstructorInfo FindExplicitConstructor()
        {
            return this.typeMap.FindExplicitConstructor();
        }

        public SqlMapper.IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName)
        {
            return this.typeMap.GetConstructorParameter(constructor, columnName);
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            return this.typeMap.GetMember(columnName);
        }

        private PropertyInfo GetProperty(Type type, string columnName)
        {
            if (this.mapSection.Elements.TryGetValue(columnName, out IInsqlMapSectionElement sectionElement))
            {
                var propertyInfo = type.GetProperty(sectionElement.To);

                if (propertyInfo == null)
                {
                    throw new Exception($"insql map element name : {sectionElement.Name} to : {sectionElement.To} Property not found!");
                }

                return propertyInfo;
            }

            return null;
        }
    }
}
