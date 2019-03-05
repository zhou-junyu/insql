using System;
using System.Collections.Generic;
using System.Text;

namespace Insql.Mappers.Annotation
{
    public class InsqlTableAttribute : Attribute
    {
        public string Name { get; set; }
    }
}
