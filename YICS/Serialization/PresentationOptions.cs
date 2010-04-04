using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YICS.Serialization
{
    public class PresentationOptions
    {
        public int IndentWidth { get; set; } // number of spaces to indent on child nodes/content
        public bool HasProperty { get; set; } // if node has properties defined, affects if first line will be blocked on a new line
    }
}
