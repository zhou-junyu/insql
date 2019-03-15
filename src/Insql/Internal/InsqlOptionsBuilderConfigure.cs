using System;

namespace Insql
{
    internal class InsqlOptionsBuilderConfigure
    {
        public Action<IInsqlOptionsBuilder> Configure { get; set; }
    }
}
