using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YICS.Representation
{
    public abstract class Node
    {
        Tag Tag { get; set; }

        public abstract string CanonicalContent { get; }

        public override string ToString()
        {
            return CanonicalContent;
        }
    }
}
