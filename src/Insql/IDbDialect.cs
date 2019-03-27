namespace Insql
{
    public interface IDbDialect
    {
        char OpenQuote { get; }

        char CloseQuote { get; }

        char ParameterPrefix { get; }

        string BatchSeperator { get; }

        bool SupportsBatchStatements { get; }


        bool SupportsSelectIdentity { get; }

        string GetIdentitySelectString(string table);
    }
}
