using Insql.Resolvers;
using Insql.Resolvers.Elements;
using Insql.Resolvers.Sections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Insql.Providers
{
    public class InsqlDescriptorXmlParser
    {
        public static readonly InsqlDescriptorXmlParser Instance = new InsqlDescriptorXmlParser();

        public InsqlDescriptor ParseDescriptor(Stream insqlStream, string insqlNamespace)
        {
            using (insqlStream)
            {
                var doc = XDocument.Load(insqlStream);

                var root = doc.Root;

                if (root.Name != XName.Get("insql", insqlNamespace))
                {
                    return null;
                }

                var typeAttr = root.Attribute(XName.Get("type", insqlNamespace));

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
                if (node.NodeType == XmlNodeType.CDATA)
                {
                    XCData xcdata = (XCData)node;

                    return new TextSectionElement(xcdata.Value);
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
                        case "each":
                            {
                                var repeatSection = new EachSectionElement(
                                     xelement.Attribute(XName.Get("name", ""))?.Value
                                     )
                                {
                                    Prefix = xelement.Attribute(XName.Get("prefix", ""))?.Value,
                                    Suffix = xelement.Attribute(XName.Get("suffix", ""))?.Value,
                                    Open = xelement.Attribute(XName.Get("open", ""))?.Value,
                                    Close = xelement.Attribute(XName.Get("close", ""))?.Value,
                                    Separator = xelement.Attribute(XName.Get("separator", ""))?.Value
                                };

                                return repeatSection;
                            }
                    }
                }

                return (IInsqlSectionElement)null;
            }).Where(o => o != null).ToList();
        }
    }
}
