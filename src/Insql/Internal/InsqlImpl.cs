using System;
using System.Collections.Generic;
using System.Text;
using Insql.Mappers;

namespace Insql
{
    internal class InsqlImpl : IInsql
    {
        public InsqlImpl()
        {

        }

        public IDbSession Session => throw new NotImplementedException();

        public IInsqlModel Model => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
