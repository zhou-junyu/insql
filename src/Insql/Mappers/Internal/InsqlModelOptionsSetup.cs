using Microsoft.Extensions.Options;

namespace Insql.Mappers
{
    internal class InsqlModelOptionsSetup : IConfigureOptions<InsqlModelOptions>
    {
        public void Configure(InsqlModelOptions options)
        {
            options.IncludeAnnotationMaps = false;
            options.IncludeFluentMaps = false;
            options.ExcludeXmlMaps = true;
        }
    }
}
