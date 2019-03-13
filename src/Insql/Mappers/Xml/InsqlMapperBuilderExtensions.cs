﻿using Insql.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace Insql
{
    public static partial class InsqlMapperBuilderExtensions
    {
        public static IInsqlMapperBuilder DisableXmlMap(this IInsqlMapperBuilder builder)
        {
            builder.Services.Configure<InsqlModelOptions>(options =>
            {
                options.XmlMapEnabled = false;
            });

            return builder;
        }
    }
}