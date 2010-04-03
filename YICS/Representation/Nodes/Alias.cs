using System;
using System.Collections.Generic;

namespace YICS.Representation
{
    public class Alias : Node
    {
        internal Alias(string handle)
        {
            Tag = null;
            AnchorHandle = handle;
        }

        public Node Anchor
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string CanonicalContent { get { return Anchor.CanonicalContent; } }

        public override bool IsCollection() { return Anchor.IsCollection(); }

        public override bool IsScalar() { return Anchor.IsScalar(); }

        public override bool Equals(object obj) { return Anchor.Equals(obj); }

        public override int GetHashCode() { return Anchor.GetHashCode(); }
    }
}
