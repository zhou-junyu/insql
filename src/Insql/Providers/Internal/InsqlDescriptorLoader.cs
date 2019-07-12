using Insql.Resolvers;
using System;
using System.Collections.Generic;

namespace Insql.Providers
{
   internal class InsqlDescriptorLoader : IInsqlDescriptorLoader
   {
      private readonly IEnumerable<IInsqlDescriptorProvider> descriptorProviders;

      private readonly Dictionary<Type, InsqlDescriptor> insqlDescriptors;

      public InsqlDescriptorLoader(IEnumerable<IInsqlDescriptorProvider> descriptorProviders)
      {
         this.descriptorProviders = descriptorProviders;

         this.insqlDescriptors = this.LoadInsqlDescriptors();
      }

      public IDictionary<Type, InsqlDescriptor> Load()
      {
         return this.insqlDescriptors;
      }

      private Dictionary<Type, InsqlDescriptor> LoadInsqlDescriptors()
      {
         var allDescriptors = new Dictionary<Type, InsqlDescriptor>();

         foreach (var provider in this.descriptorProviders)
         {
            //同一个提供者不支持同一类型（作用域）内的Section,Map重名
            var providerDescriptors = new Dictionary<Type, InsqlDescriptor>();

            foreach (var descriptor in provider.GetDescriptors())
            {
               if (providerDescriptors.TryGetValue(descriptor.Type, out InsqlDescriptor insqlDescriptor))
               {
                  //sections
                  foreach (var section in descriptor.Sections)
                  {
                     if (insqlDescriptor.Sections.ContainsKey(section.Key))
                     {
                        throw new Exception($"{descriptor.Type} section: {section.Key} already exists!");
                     }

                     insqlDescriptor.Sections[section.Key] = section.Value;
                  }

                  //maps
                  foreach (var map in descriptor.Maps)
                  {
                     if (insqlDescriptor.Maps.ContainsKey(map.Key))
                     {
                        throw new Exception($"{descriptor.Type} map: {map.Key} already exists!");
                     }

                     insqlDescriptor.Maps[map.Key] = map.Value;
                  }
               }
               else
               {
                  providerDescriptors.Add(descriptor.Type, descriptor);
               }
            }

            //merge provider
            foreach (var descriptor in providerDescriptors.Values)
            {
               if (allDescriptors.TryGetValue(descriptor.Type, out InsqlDescriptor insqlDescriptor))
               {
                  //sections
                  foreach (var section in descriptor.Sections)
                  {
                     insqlDescriptor.Sections[section.Key] = section.Value;
                  }

                  //maps
                  foreach (var map in descriptor.Maps)
                  {
                     insqlDescriptor.Maps[map.Key] = map.Value;
                  }
               }
               else
               {
                  allDescriptors.Add(descriptor.Type, descriptor);
               }
            }
         }

         return allDescriptors;
      }
   }
}
