namespace Insql.SqlServer
{
    internal class SqlServerDialect : DbDialect
    {
        public static SqlServerDialect Instance = new SqlServerDialect();

        public override string DbType => "SqlServer";

        public override char OpenQuote => '[';

        public override char CloseQuote => ']';
    }
}
