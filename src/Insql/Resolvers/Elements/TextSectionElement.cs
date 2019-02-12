namespace Insql.Resolvers.Elements
{
    public class TextSectionElement : IInsqlSectionElement
    {
        public string Text { get; }

        public TextSectionElement(string text)
        {
            this.Text = text;
        }

        public string Resolve(ResolveContext context)
        {
            if (string.IsNullOrWhiteSpace(this.Text))
            {
                return string.Empty;
            }

            return this.Text.Trim();
        }
    }
}
