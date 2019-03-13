using System;
using System.Collections.Generic;

namespace Insql.Mappers
{
    internal class InsqlEntityMap : IInsqlEntityMap
    {
        public InsqlEntityMap(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            this.EntityType = entityType;
            this.Table = entityType.Name;
        }
        public InsqlEntityMap(Type entityType, string table, string schema)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            if (string.IsNullOrWhiteSpace(table))
            {
                table = entityType.Name;
            }

            this.EntityType = entityType;
            this.Table = table;
            this.Schema = schema;
        }

        public Type EntityType { get; }

        public string Table { get; set; }

        public string Schema { get; set; }

        public IList<IInsqlPropertyMap> Properties { get; } = new List<IInsqlPropertyMap>();
    }
}
