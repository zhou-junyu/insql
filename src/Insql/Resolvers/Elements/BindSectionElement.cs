using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql.Resolvers.Elements
{
    public class BindSectionElement : ISqlSectionElement
    {
        public string Name { get; }

        public string Value { get; }

        public string RefId { get; }

        public BindSectionElement(string name, string value, string refid)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(refid))
            {
                throw new ArgumentNullException($"{nameof(name)},{nameof(refid)}");
            }
            if (!string.IsNullOrWhiteSpace(this.Value) && !string.IsNullOrWhiteSpace(this.RefId))
            {
                throw new Exception("if element test or refid not coexist ! ");
            }

            this.Name = name;
            this.Value = value;
            this.RefId = refid;
        }

        public string Resolve(ResolveContext context)
        {
            object executeResult = null;

            if (!string.IsNullOrWhiteSpace(this.RefId))
            {
                if (!context.InsqlDescriptor.Sections.ContainsKey(this.RefId))
                {
                    throw new ArgumentException($"mapper section id : {this.RefId} not found !");
                }

                IInsqlSection sectionDescriptor = context.InsqlDescriptor.Sections[this.RefId];

                executeResult = sectionDescriptor.Resolve(new ResolveContext
                {
                    ServiceProvider = context.ServiceProvider,
                    InsqlDescriptor = context.InsqlDescriptor,
                    SectionDescriptor = sectionDescriptor,
                    Param = context.Param
                });
            }
            else if (!string.IsNullOrWhiteSpace(this.Value))
            {
                var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlCodeResolver>();

                executeResult = codeExecuter.Resolve(typeof(object), this.Value, context.Param);
            }

            //给上下文设置参数
            context.Param[this.Name] = executeResult;

            //返回空内容
            return string.Empty;
        }
    }
}
