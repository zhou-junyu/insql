using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    public class InsqlEntityMap : IInsqlEntityMap
    {
        public InsqlEntityMap(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            this.EntityType = entityType;
            this.TableName = entityType.Name;
        }
        public InsqlEntityMap(Type entityType, string tableName)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            if (string.IsNullOrWhiteSpace(tableName))
            {
                throw new ArgumentNullException(nameof(tableName));
            }

            this.EntityType = entityType;
            this.TableName = tableName;
        }

        public Type EntityType { get; }

        public string TableName { get; }

        public IList<IInsqlPropertyMap> PropertyMaps { get; } = new List<IInsqlPropertyMap>();
    }
}
