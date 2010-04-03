using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YICS.Representation;

namespace YICS.Serialization
{
    internal class AnchorRecord
    {
        public int HashCode { get; set; }
        public string Handle { get; set; }
        public Node Node { get; set; }
        public bool HasAlias { get; set; }
    }
}
