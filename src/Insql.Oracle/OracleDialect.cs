using System;

namespace Insql.Oracle
{
    internal class OracleDialect : IDbDialect
    {
        public static OracleDialect Instance = new OracleDialect();

        public string DbType => "Oracle";

        public char OpenQuote => '"';

        public char CloseQuote => '"';

        public char ParameterPrefix => ':';

        public string BatchSeperator => $";{Environment.NewLine}";

        public bool SupportsBatchStatements => false;
    }
}
