using System;
using System.Collections.Generic;
using System.Text;

namespace Insql.Internal
{
    internal class ActionSessionFactory : IDbSessionFactory
    {
        private readonly Func<Type, IDbSession> factory;

        public ActionSessionFactory(Func<Type, IDbSession> factory)
        {
            this.factory = factory;
        }

        public IDbSession CreateSession(Type contextType)
        {
            return this.factory(contextType);
        }
    }
}
