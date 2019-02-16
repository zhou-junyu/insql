using Insql.Resolvers.Scripts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Insql.Resolvers.Elements
{
    public class IfSectionElement : IInsqlSectionElement
    {
        public string Test { get; }

        public List<IInsqlSectionElement> Children { get; }

        public IfSectionElement(string test)
        {
            if (string.IsNullOrWhiteSpace(test))
            {
                throw new ArgumentNullException(nameof(test));
            }

            this.Test = test;

            this.Children = new List<IInsqlSectionElement>();
        }

        public string Resolve(ResolveContext context)
        {
            var codeExecuter = context.ServiceProvider.GetRequiredService<IInsqlScriptResolver>();

            var isTest = (bool)codeExecuter.Resolve(typeof(bool), this.Test, context.Param);

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
