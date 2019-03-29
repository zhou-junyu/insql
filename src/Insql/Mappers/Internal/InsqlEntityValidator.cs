using System;
using System.Linq;

namespace Insql.Mappers
{
    internal class InsqlEntityValidator
    {
        public static InsqlEntityValidator Instance = new InsqlEntityValidator();

        public void Validate(IInsqlEntityMap entityMap)
        {
            foreach (var groupItem in entityMap.Properties.Where(item => !item.IsIgnored).GroupBy(item => item.ColumnName))
            {
                if (groupItem.Count() > 1)
                {
                    throw new Exception($"insql entity type : {entityMap.EntityType} `{groupItem.Key}` column name already exist!");
                }
            }

            if (entityMap.Properties.Where(item => !item.IsIgnored).Count(item => item.IsIdentity) > 1)
            {
                throw new Exception($"insql entity type : {entityMap.EntityType} identity column already exist!");
            }
        }
    }
}
