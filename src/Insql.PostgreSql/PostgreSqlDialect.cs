using System;

namespace Insql.PostgreSql
{
    internal class PostgreSqlDialect : IDbDialect
    {
        public static PostgreSqlDialect Instance = new PostgreSqlDialect();

        public string Name => "PostgreSql";

        public char OpenQuote => '"';

        public char CloseQuote => '"';

        public char ParameterPrefix => '@';

        public string BatchSeperator => $";{Environment.NewLine}";

        public bool SupportsBatchStatements => true;
    }
}
