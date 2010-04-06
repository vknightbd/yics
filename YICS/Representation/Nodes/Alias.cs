using System;
using System.Collections.Generic;

namespace YICS.Representation
{
    public class Alias : Node, IComparable
    {
        internal Alias(string handle, Node anchor)
        {
            AnchorHandle = handle;
            Anchor = anchor;
            Tag = Anchor.Tag;
        }

        public Node Anchor { get; set; }

        //public override string CanonicalContent { get { return Anchor.CanonicalContent; } }
        public override string CanonicalContent { get { return "*" + AnchorHandle; } }

        public override string PresentContent(YICS.Serialization.Serializer serializer)
        {
            return "*" + AnchorHandle;
        }

        public override bool IsCollection() { return Anchor.IsCollection(); }

        public override bool IsScalar() { return Anchor.IsScalar(); }

        /* commented out because overriding Equals causes dictionary[aliasAsIndex] to fail
        public override bool Equals(object obj) { return Anchor.Equals(obj); }
        public override int GetHashCode() { return Anchor.GetHashCode(); }
        public override int GetNodeHashCode(ref List<int> existingNodeHashCodes) { return Anchor.GetNodeHashCode(ref existingNodeHashCodes); }
         */
        public override int GetNodeHashCode(ref List<int> existingNodeHashCodes) { return GetHashCode(); }

        #region IComparable<Alias> Members

        public int CompareTo(object other)
        {
            Alias alias = (Alias)other;
            return AnchorHandle.CompareTo(alias.AnchorHandle);
        }

        #endregion
    }
}
