using System;

namespace YICS.Representation
{
    public abstract class Node
    {
        public Tag Tag { get; set; }

        public virtual string CanonicalContent
        {
            get
            {
                return Tag.CanonicalFormat(this); // usually Tag determines how the node is outputted to string
            }
        }

        public virtual bool IsCollection()
        {
            if (Tag == null) throw new InvalidOperationException("Node does not have a tag.");

            return Tag.IsCollection();
        }

        public virtual bool IsScalar()
        {
            if (Tag == null) throw new InvalidOperationException("Node does not have a tag.");

            return Tag.IsScalar();
        }

        public override string ToString()
        {
            return CanonicalContent;
        }
    }
}
