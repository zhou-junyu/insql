using Insql.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Insql
{
    public static partial class InsqlBuilderExtensions
    {
        public static IInsqlBuilder UserDbConnection(this IInsqlBuilder builder, IDbSessionFactory sessionFactory)
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IDbSessionFactory>(sessionFactory));

            return builder;
        }

        public static IInsqlBuilder UserDbConnection<T>(this IInsqlBuilder builder) where T : class, IDbSessionFactory
        {
            builder.Services.TryAdd(ServiceDescriptor.Singleton<IDbSessionFactory, T>());

            return builder;
        }

        public static IInsqlBuilder UserDbConnection(this IInsqlBuilder builder, Func<Type, IDbSession> factory)
        {
            return builder.UserDbConnection(new ActionSessionFactory(factory));
        }
    }
}
