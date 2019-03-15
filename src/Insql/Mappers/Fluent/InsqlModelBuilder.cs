using System;
using System.Reflection;

namespace Insql.Mappers
{
    public class InsqlModelBuilder
    {
        private readonly InsqlEntityMap entityMap;

        public InsqlModelBuilder(Type entityType)
        {
            this.entityMap = new InsqlEntityMap(entityType);
        }

        public InsqlModelBuilder Table(string table)
        {
            this.entityMap.Table = table;

            return this;
        }

        public InsqlModelBuilder Table(string table, string schema)
        {
            this.entityMap.Table = table;
            this.entityMap.Schema = schema;

            return this;
        }

        public InsqlModelPropertyBuilder Property(string propertyName)
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

        public InsqlModelPropertyBuilder Property(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return new InsqlModelPropertyBuilder(this.entityMap, propertyInfo);
        }

        public IInsqlEntityMap Build()
        {
            return this.entityMap;
        }
    }
}
