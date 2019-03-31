using System;

namespace Insql
{
    public abstract class DbDialect : IDbDialect
    {
        public abstract string DbType { get; }

        public virtual char OpenQuote => '"';

        public virtual char CloseQuote => '"';

        public virtual char ParameterPrefix => '@';

        public virtual string BatchSeperator => $";{Environment.NewLine}";

        public virtual bool SupportsBatchStatements => true;
    }
}
