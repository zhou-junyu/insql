using Insql.Resolvers;
using Insql.Resolvers.Elements;
using Insql.Resolvers.Sections;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Insql.Providers.Embedded
{
    public class EmbeddedDescriptorProvider : IInsqlDescriptorProvider
    {
        private readonly IOptions<EmbeddedDescriptorOptions> options;

        private readonly ConcurrentDictionary<Assembly, List<InsqlDescriptor>> insqlDescriptors;

        public EmbeddedDescriptorProvider(IOptions<EmbeddedDescriptorOptions> options)
        {
            this.options = options;
            this.insqlDescriptors = new ConcurrentDictionary<Assembly, List<InsqlDescriptor>>();
        }

        public IEnumerable<InsqlDescriptor> GetDescriptors()
        {
            IEnumerable<Assembly> assemblies = this.options.Value.Assemblies;

            if (assemblies == null || assemblies.Count() < 1)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }

            assemblies = assemblies.Where(assembly => !assembly.IsDynamic && !assembly.ReflectionOnly && !this.insqlDescriptors.Keys.Contains(assembly));

            var descriptorPairs = this.GetAssemblyDescriptors(assemblies);

            foreach (var pair in descriptorPairs)
            {
                this.insqlDescriptors.TryAdd(pair.Key, pair.Value);
            }

            return insqlDescriptors.Values.SelectMany(o => o).ToList();
        }

        private IEnumerable<KeyValuePair<Assembly, List<InsqlDescriptor>>> GetAssemblyDescriptors(IEnumerable<Assembly> assemblies)
        {
            return assemblies.Select(assembly =>
             {
                 var resourceNames = assembly.GetManifestResourceNames();

                 resourceNames = GlobHelper.Filter(resourceNames, this.options.Value.Locations).ToArray();

                 var resourceDescriptors = resourceNames.Select(name =>
                 {
                     return this.ParseDescriptor(assembly.GetManifestResourceStream(name));
                 }).Where(o => o != null).ToList();

                 return new KeyValuePair<Assembly, List<InsqlDescriptor>>(assembly, resourceDescriptors);
             }).ToList();
        }

        private InsqlDescriptor ParseDescriptor(Stream stream)
        {
            using (stream)
            {
                var doc = XDocument.Load(stream);

                var root = doc.Root;

                if (root.Name != XName.Get("insql", this.options.Value.Namespace))
                {
                    return null;
                }

                var typeAttr = root.Attribute(XName.Get("type", this.options.Value.Namespace));

                if (typeAttr == null)
                {
                    return null;
                }

                var type = Type.GetType(typeAttr.Value);

                if (type == null)
                {
                    throw new Exception($"insql type : {typeAttr.Value} not found !");
                }

                var descriptor = new InsqlDescriptor(type);

                foreach (var section in this.ParseSectionDescriptors(root))
                {
                    descriptor.Sections.Add(section.Id, section);
                }

                return descriptor;
            }
        }

        private IEnumerable<IInsqlSection> ParseSectionDescriptors(XElement root)
        {
            var sqlSections = root.Elements(XName.Get("sql", "")).Select(element =>
            {
                var id = element.Attribute(XName.Get("id", ""));

                if (id == null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new Exception("insql sql section element `id` is empty !");
                }

                var section = new SqlInsqlSection(id.Value);

                section.Elements.AddRange(this.ParseSqlSections(element));

                return section;
            }).Cast<IInsqlSection>();

            var selectSqlSections = root.Elements(XName.Get("select", "")).Select(element =>
            {
                var id = element.Attribute(XName.Get("id", ""));

                if (id == null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new Exception("insql select insert sql section element `id` is empty !");
                }

                var section = new SqlInsqlSection(id.Value);

                section.Elements.AddRange(this.ParseSqlSections(element));

                return section;
            }).Cast<IInsqlSection>();

            var insertSqlSections = root.Elements(XName.Get("insert", "")).Select(element =>
            {
                var id = element.Attribute(XName.Get("id", ""));

                if (id == null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new Exception("insql insert sql section element `id` is empty !");
                }

                var section = new SqlInsqlSection(id.Value);

                section.Elements.AddRange(this.ParseSqlSections(element));

                return section;
            }).Cast<IInsqlSection>();

            var updateSqlSections = root.Elements(XName.Get("update", "")).Select(element =>
            {
                var id = element.Attribute(XName.Get("id", ""));

                if (id == null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new Exception("insql update sql section element `id` is empty !");
                }

                var section = new SqlInsqlSection(id.Value);

                section.Elements.AddRange(this.ParseSqlSections(element));

                return section;
            }).Cast<IInsqlSection>();

            var deleteSqlSections = root.Elements(XName.Get("delete", "")).Select(element =>
            {
                var id = element.Attribute(XName.Get("id", ""));

                if (id == null || string.IsNullOrWhiteSpace(id.Value))
                {
                    throw new Exception("insql delete sql section element `id` is empty !");
                }

                var section = new SqlInsqlSection(id.Value);

                section.Elements.AddRange(this.ParseSqlSections(element));

                return section;
            }).Cast<IInsqlSection>();

            return sqlSections
                .Concat(selectSqlSections)
                .Concat(insertSqlSections)
                .Concat(updateSqlSections)
                .Concat(deleteSqlSections)
                .ToList();
        }

        private IEnumerable<IInsqlSectionElement> ParseSqlSections(XElement element)
        {
            return element.Nodes().Select(node =>
             {
                 if (node.NodeType == XmlNodeType.Text)
                 {
                     XText xtext = (XText)node;

                     return new TextSectionElement(xtext.Value);
                 }
                 else if (node.NodeType == XmlNodeType.Element)
                 {
                     XElement xelement = (XElement)node;

                     switch (xelement.Name.LocalName)
                     {
                         case "bind":
                             {
                                 return new BindSectionElement(
                                     xelement.Attribute(XName.Get("name", ""))?.Value,
                                     xelement.Attribute(XName.Get("value", ""))?.Value
                                 );
                             }
                         case "if":
                             {
                                 var ifSection = new IfSectionElement(
                                     xelement.Attribute(XName.Get("test", ""))?.Value
                                 );

                                 ifSection.Children.AddRange(this.ParseSqlSections(xelement));

                                 return ifSection;
                             }
                         case "include":
                             {
                                 return new IncludeSectionElement(
                                     xelement.Attribute(XName.Get("refid", ""))?.Value
                                 );
                             }
                         case "trim":
                             {
                                 var trimSection = new TrimSectionElement
                                 {
                                     Prefix = xelement.Attribute(XName.Get("prefix", ""))?.Value,
                                     Suffix = xelement.Attribute(XName.Get("suffix", ""))?.Value,
                                     PrefixOverrides = xelement.Attribute(XName.Get("prefixOverrides", ""))?.Value,
                                     SuffixOverrides = xelement.Attribute(XName.Get("suffixOverrides", ""))?.Value,
                                 };

                                 trimSection.Children.AddRange(this.ParseSqlSections(xelement));

                                 return trimSection;
                             }
                         case "where":
                             {
                                 var whereSection = new WhereSectionElement();

                                 whereSection.Children.AddRange(this.ParseSqlSections(xelement));

                                 return whereSection;
                             }
                         case "set":
                             {
                                 var setSection = new SetSectionElement();

                                 setSection.Children.AddRange(this.ParseSqlSections(xelement));

                                 return setSection;
                             }
                     }
                 }

                 return (IInsqlSectionElement)null;
             }).Where(o => o != null).ToList();
        }
    }
}
