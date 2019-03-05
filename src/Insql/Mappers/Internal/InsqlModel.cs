using Insql.Providers;
using Insql.Resolvers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Mappers
{
    internal class InsqlModel : IInsqlModel
    {
        private readonly Dictionary<Type, IInsqlEntityMap> entityMaps = new Dictionary<Type, IInsqlEntityMap>();

        public InsqlModel(IOptions<InsqlModelOptions> options, IInsqlDescriptorLoader descriptorLoader, IInsqlEntityMapper entityMapper)
        {
            var optionsValue = options.Value;

            //todo add other

            if (options.Value.XmlMapEnabled)
            {
                this.LoadXmlEntityMaps(descriptorLoader);
            }

            entityMapper.Mapping(this.entityMaps);
        }

        public void LoadXmlEntityMaps(IInsqlDescriptorLoader descriptorLoader)
        {
            var mapSections = new Dictionary<Type, IInsqlMapSection>();

            var insqlDescriptors = descriptorLoader.Load().Values;

            foreach (var descriptor in insqlDescriptors)
            {
                foreach (var map in descriptor.Maps.Values)
                {
                    mapSections[map.Type] = map;
                }
            }

            var resultMaps = mapSections.Values.Select(mapSection =>
            {
                var entityMap = string.IsNullOrWhiteSpace(mapSection.Table) ?
                    new InsqlEntityMap(mapSection.Type) :
                    new InsqlEntityMap(mapSection.Type, mapSection.Table);

                foreach (var mapElement in mapSection.Elements.Values)
                {
                    if (entityMap.PropertyMaps.Any(o => o.ColumnName == mapElement.Name))
                    {
                        throw new Exception($"insql entity type : {mapSection.Type} `{mapElement.Name}` column name already exist!");
                    }
                    if (entityMap.PropertyMaps.Any(o => o.IsIdentity && mapElement.Identity))
                    {
                        throw new Exception($"insql entity type : {mapSection.Type} `{mapElement.Name}` identity column cannot have multiple!");
                    }

                    var propertyInfo = mapSection.Type.GetProperty(mapElement.To);

                    if (propertyInfo == null)
                    {
                        throw new Exception($"insql entity type : {mapSection.Type} `{mapElement.To}` property is not exist!");
                    }

                    entityMap.PropertyMaps.Add(new InsqlPropertyMap(propertyInfo, mapElement.Name)
                    {
                        IsIdentity = mapElement.Identity,
                        IsKey = mapElement.ElementType == InsqlMapElementType.Key
                    });
                }

                return entityMap;
            }).ToDictionary(item => item.EntityType, item => (IInsqlEntityMap)item);

            //-----
            foreach (var itemMap in resultMaps)
            {
                this.entityMaps[itemMap.Key] = itemMap.Value;
            }
        }

        public IInsqlEntityMap GetMap(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            if (this.entityMaps.TryGetValue(entityType, out IInsqlEntityMap entityMap))
            {
                return entityMap;
            }

            return null;
        }

        public IEnumerable<IInsqlEntityMap> GetMaps()
        {
            return this.entityMaps.Values;
        }
    }
}
