using Microsoft.Extensions.DependencyInjection;
using System;

namespace Insql.Tests
{
    public abstract class TestsBase : IDisposable
    {
        private readonly ServiceProvider _globalServiceProvider;

        protected virtual ServiceProvider GlobalServiceProvider => this._globalServiceProvider;

        public TestsBase()
        {
            this._globalServiceProvider = this.CreateServiceProvider(this.GlobalServiceProviderConfigure);
        }

        public void Dispose()
        {
            if (this._globalServiceProvider != null)
            {
                this._globalServiceProvider.Dispose();
            }
        }

        protected virtual void GlobalServiceProviderConfigure(IInsqlBuilder builder)
        {
        }

        protected virtual ServiceProvider CreateServiceProvider(Action<IInsqlBuilder> configure = null)
        {
            var serviceCollection = new ServiceCollection();

            if (configure == null)
            {
                serviceCollection.AddInsql();
            }
            else
            {
                serviceCollection.AddInsql(configure);
            }

            return serviceCollection.BuildServiceProvider();
        }
    }
}
