using System;

namespace Insql.Sqlite
{
    internal class SqliteDialect : IDbDialect
    {
        public static SqliteDialect Instance = new SqliteDialect();

        public string Name => "Sqlite";

        public char OpenQuote => '"';

        public char CloseQuote => '"';

        public char ParameterPrefix => '@';

        public string BatchSeperator => $";{Environment.NewLine}";

        public bool SupportsBatchStatements => true;
    }
}
