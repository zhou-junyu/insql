using Dapper;
using System;
using System.Linq;
using System.Reflection;

namespace Insql.Mappers
{
    internal class DapperTypeMap : SqlMapper.ITypeMap
    {
        private readonly CustomPropertyTypeMap typeMap;
        private readonly IInsqlEntityMap entityMap;

        public DapperTypeMap(IInsqlEntityMap entityMap)
        {
            this.entityMap = entityMap;

            this.typeMap = new CustomPropertyTypeMap(entityMap.EntityType, this.GetProperty);
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
            var propertyMap = this.entityMap.Properties.SingleOrDefault(o => o.ColumnName == columnName);

            if (propertyMap == null || propertyMap.IsIgnored)
            {
                return null;
            }

            return propertyMap.PropertyInfo;
        }
    }
}
