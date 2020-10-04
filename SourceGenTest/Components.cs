using StyledComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SourceGenTest
{
    public class BorderedDiv : StyledComponents.Styled
    {
        public BorderedDiv() : base("div")
        {
            CssProperties = "border:1px solid #ccc";
        }
    }
}
