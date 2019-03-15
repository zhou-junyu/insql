using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddOptions(this IInsqlBuilder builder, Action<IInsqlOptionsBuilder> configure)
        {
            builder.Services.Configure<InsqlOptionsBuilderConfigure>(options =>
            {
                options.Configure = configure;
            });

            return builder;
        }
    }
}
