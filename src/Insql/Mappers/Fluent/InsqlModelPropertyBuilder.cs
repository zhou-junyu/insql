using System;
using System.Linq;
using System.Reflection;

namespace Insql.Mappers
{
    public class InsqlModelPropertyBuilder
    {
        private readonly IInsqlEntityMap entityMap;
        private readonly InsqlPropertyMap propertyMap;

        public InsqlModelPropertyBuilder(IInsqlEntityMap entityMap, PropertyInfo propertyInfo)
        {
            if (entityMap.Properties.Any(o => o.PropertyInfo.Name == propertyInfo.Name))
            {
                throw new Exception($"insql entity builder type : {entityMap.EntityType} `{propertyInfo.Name}` property have been mapped!");
            }

            this.entityMap = entityMap;

            this.propertyMap = new InsqlPropertyMap(propertyInfo);

            this.entityMap.Properties.Add(this.propertyMap);
        }

        public InsqlModelPropertyBuilder Column(string columnName)
        {
            if (string.IsNullOrWhiteSpace(columnName))
            {
                throw new ArgumentNullException(nameof(columnName));
            }

            this.propertyMap.ColumnName = columnName;

            return this;
        }

        public InsqlModelPropertyBuilder Key()
        {
            this.propertyMap.IsKey = true;

            return this;
        }

        public InsqlModelPropertyBuilder Identity()
        {
            if (this.entityMap.Properties.Any(o => o.PropertyInfo.Name != this.propertyMap.PropertyInfo.Name && o.IsIdentity))
            {
                throw new Exception($"insql entity builder type : {this.entityMap.EntityType} `{this.propertyMap.PropertyInfo.Name}` identity column cannot have multiple!");
            }

            this.propertyMap.IsIdentity = true;

            return this;
        }

        public InsqlModelPropertyBuilder Ignore()
        {
            this.propertyMap.IsIgnored = true;

            return this;
        }
    }
}
