using System.Linq;

namespace Insql
{
    public static partial class DbDialectExtensions
    {
        public static bool IsQuoted(this IDbDialect dialect, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            if (value.Trim().First() == dialect.OpenQuote)
            {
                return value.Trim().Last() == dialect.CloseQuote;
            }

            return false;
        }

        public static string QuoteString(this IDbDialect dialect, string value)
        {
            if (IsQuoted(value) || value == "*")
            {
                return value;
            }
            return string.Format("{0}{1}{2}", OpenQuote, value.Trim(), CloseQuote);
        }
    }
}
