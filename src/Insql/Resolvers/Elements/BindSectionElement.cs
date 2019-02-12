using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql.Resolvers.Elements
{
    public class BindSectionElement : ISqlSectionElement
    {
        public string Name { get; }

        public string Value { get; }

        public BindSectionElement(string name, string value)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Name = name;
            this.Value = value;
        }

        public string Resolve(ResolveContext context)
        {
            var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlCodeResolver>();

            var executeResult = codeExecuter.Resolve(typeof(object), this.Value, context.Param);

            context.Param[this.Name] = executeResult;

            return string.Empty;
        }
    }
}
