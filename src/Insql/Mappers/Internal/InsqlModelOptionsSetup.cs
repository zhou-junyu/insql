using Microsoft.Extensions.Options;

namespace Insql.Mappers
{
    internal class InsqlModelOptionsSetup : IConfigureOptions<InsqlModelOptions>
    {
        public void Configure(InsqlModelOptions options)
        {
            options.AnnotationMapScanEnabled = false;
            options.FluentMapScanEnabled = false;
            options.XmlMapEnabled = true;
        }
    }
}
