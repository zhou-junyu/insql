using System;

namespace Insql.MySql
{
    internal class MySqlDialect : IDbDialect
    {
        public static MySqlDialect Instance = new MySqlDialect();

        public string Name => "MySql";

        public char OpenQuote => '`';

        public char CloseQuote => '`';

        public  char ParameterPrefix => '@';

        public  string BatchSeperator => $";{Environment.NewLine}";

        public  bool SupportsBatchStatements => true;
    }
}
