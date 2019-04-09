using System;

namespace Insql.SqlServer
{
    internal class SqlServerDialect : IDbDialect
    {
        public static SqlServerDialect Instance = new SqlServerDialect();

        public string Name => "SqlServer";

        public char OpenQuote => '[';

        public char CloseQuote => ']';

        public  char ParameterPrefix => '@';

        public  string BatchSeperator => $";{Environment.NewLine}";

        public  bool SupportsBatchStatements => true;
    }
}
