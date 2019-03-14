using System;

namespace Insql
{
    public abstract class DbDialectBase : IDbDialect
    {
        public virtual char OpenQuote => '"';

        public virtual char CloseQuote => '"';

        public virtual char ParameterPrefix => '@';

        public virtual string BatchSeperator => $";{Environment.NewLine}";

        public virtual bool SupportsMultipleStatements => true;

        public abstract bool SupportsSelectIdentity { get; }

        public abstract string GetIdentitySelectString(string table);
    }
}
