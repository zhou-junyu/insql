namespace Insql
{
    public interface IDbDialect
    {
        string Name { get; }

        char OpenQuote { get; }

        char CloseQuote { get; }

        char ParameterPrefix { get; }

        string BatchSeperator { get; }

        bool SupportsBatchStatements { get; }
    }
}
