using System;
using System.Reflection;

namespace Insql.Mappers
{
    public class InsqlEntityBuilder : IInsqlEntityBuilder
    {
        private readonly InsqlEntityMap entityMap;

        public InsqlEntityBuilder(Type entityType)
        {
            this.entityMap = new InsqlEntityMap(entityType);
        }

        public InsqlEntityBuilder Table(string table)
        {
            this.entityMap.Table = table;

            return this;
        }

        public InsqlEntityBuilder Table(string table, string schema)
        {
            this.entityMap.Table = table;
            this.entityMap.Schema = schema;

            return this;
        }

        public InsqlEntityPropertyBuilder Property(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var propertyInfo = this.entityMap.EntityType.GetProperty(propertyName);

            if (propertyInfo == null)
            {
                throw new ArgumentException($"`{propertyName}` property not found!");
            }

            return this.Property(propertyInfo);
        }

        public InsqlEntityPropertyBuilder Property(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return new InsqlEntityPropertyBuilder(this.entityMap, new InsqlPropertyMap(propertyInfo));
        }

        public IInsqlEntityMap Build()
        {
            return this.entityMap;
        }
    }
}
