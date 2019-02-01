using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Resolvers.Elements
{
    public class IfSectionElement : ISqlSectionElement
    {
        public string Test { get; }

        public string RefId { get; }

        public List<ISqlSectionElement> Children { get; }

        public IfSectionElement(string test, string refid)
        {
            if (string.IsNullOrWhiteSpace(test) && string.IsNullOrWhiteSpace(refid))
            {
                throw new ArgumentNullException($"{nameof(test)},{nameof(refid)}");
            }
            if (!string.IsNullOrWhiteSpace(this.Test) && !string.IsNullOrWhiteSpace(this.RefId))
            {
                throw new Exception("if element test or refid not coexist ! ");
            }

            this.Test = test;
            this.RefId = refid;

            this.Children = new List<ISqlSectionElement>();
        }

        public string Resolve(ResolveContext context)
        {
            bool isTest = false;

            if (!string.IsNullOrWhiteSpace(this.RefId))
            {
                if (!context.InsqlDescriptor.Sections.ContainsKey(this.RefId))
                {
                    throw new ArgumentException($"mapper section id : {this.RefId} not found !");
                }

                IInsqlSection sectionDescriptor = context.InsqlDescriptor.Sections[this.RefId];

                isTest = (bool)sectionDescriptor.Resolve(new ResolveContext
                {
                    ServiceProvider = context.ServiceProvider,
                    InsqlDescriptor = context.InsqlDescriptor,
                    SectionDescriptor = sectionDescriptor,
                    Param = context.Param
                });
            }
            else if (!string.IsNullOrWhiteSpace(this.Test))
            {
                var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlCodeResolver>();

                isTest = (bool)codeExecuter.Resolve(typeof(bool), this.Test, context.Param);
            }

            if (!isTest)
            {
                return string.Empty;
            }

            //parse
            var childrenResult = this.Children.Select(children =>
            {
                return children.Resolve(context);
            });

            return string.Join(" ", childrenResult).Trim();
        }
    }
}
