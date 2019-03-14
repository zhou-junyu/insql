using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder AddMapper(this IInsqlBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlModel, InsqlModel>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IConfigureOptions<InsqlModelOptions>, InsqlModelOptionsSetup>());
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IInsqlModelMapper, DapperModelMapper>());

            return builder;
        }

        public static IInsqlBuilder AddMapper(this IInsqlBuilder builder, Action<IInsqlMapperBuilder> configre)
        {
            builder.AddMapper();

            configre(new InsqlMapperBuilder(builder.Services));

            return builder;
        }
    }
}
