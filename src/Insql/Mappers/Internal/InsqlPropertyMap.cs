using System;
using System.Reflection;

namespace Insql.Mappers
{
    internal class InsqlPropertyMap : IInsqlPropertyMap
    {
        public InsqlPropertyMap(PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            this.PropertyInfo = propertyInfo;
            this.ColumnName = propertyInfo.Name;
        }
        public InsqlPropertyMap(PropertyInfo propertyInfo, string columnName)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            if (string.IsNullOrWhiteSpace(columnName))
            {
                columnName = propertyInfo.Name;
            }

            this.PropertyInfo = propertyInfo;
            this.ColumnName = columnName;
        }

        public string ColumnName { get; set; }

        public PropertyInfo PropertyInfo { get; }

        public bool IsKey { get; set; }

        public bool IsIdentity { get; set; }

        public bool IsIgnored { get; set; }
    }
}
