using Insql.Mappers;

namespace Insql
{
    internal class InsqlImpl<TScope> : IInsql<TScope> 
        where TScope : class
    {
        private readonly IInsql insql;

        public InsqlImpl(IInsqlFactory factory)
        {
            this.insql = factory.Create(typeof(TScope));
        }

        public IDbSession Session => throw new System.NotImplementedException();

        public IInsqlModel Model => throw new System.NotImplementedException();

        public void Dispose()
        {
            this.insql.Dispose();
        }
    }
}
