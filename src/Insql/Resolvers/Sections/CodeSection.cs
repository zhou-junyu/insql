using Insql.Resolvers.Codes;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql.Resolvers.Sections
{
    public class CodeSection : IInsqlSection
    {
        public string Id { get; }

        public string Code { get; }

        public CodeSection(string id, string code)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Id = id;
            this.Code = code;
        }

        public object Resolve(ResolveContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Code))
            {
                return null;
            }

            var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlCodeResolver>();

            return codeExecuter.Resolve(typeof(object), this.Code, context.Param);
        }
    }
}
