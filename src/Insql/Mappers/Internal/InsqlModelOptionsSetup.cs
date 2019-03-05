using Microsoft.Extensions.Options;

namespace Insql.Mappers
{
    internal class InsqlModelOptionsSetup : IConfigureOptions<InsqlModelOptions>
    {
        public void Configure(InsqlModelOptions options)
        {
            options.AnnotationMapEnabled = false;
            options.FluentMapEnabled = false;
            options.XmlMapEnabled = true;
        }
    }
}
