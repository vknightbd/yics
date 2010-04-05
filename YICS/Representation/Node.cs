using System;
using System.Collections.Generic;

namespace YICS.Representation
{
    public abstract class Node
    {
        public Tag Tag { get; set; }
        public string AnchorHandle { get; set; }

        /// <summary>
        /// uses Tag.CanonicalFormat() to convert a representational model directly to string.
        /// Note this method does not detect cyclical paths for Mappings and Sequences which causes stack overflow. Use YICS.Serializer instead.
        /// </summary>
        public virtual string CanonicalContent
        {
            get
            {
                return Tag.CanonicalFormat(this); // usually Tag determines how the node is outputted to string
            }
        }

        /// <summary>
        /// uses Tag.PresentContent() to convert a event tree directly to string.
        /// </summary>
        public virtual string PresentContent(Serialization.Serializer serializer)
        {
            return Tag.PresentContent(this, serializer); // Tag determines how the node is outputted to string
        }
        
        /// <summary>
        /// Get a HashCode for the Node. Used by collection nodes when determining if node already in list.
        /// </summary>
        /// <param name="existingNodeHashCodes">List of int from Object.GetHashCode() to ensure we don't follow cycles in tree.</param>
        public abstract int GetNodeHashCode(ref List<int> existingNodeHashCodes);

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

        public virtual bool IsAlias() {
            return this.GetType() == typeof(Alias);
        }

        public override string ToString()
        {
            return CanonicalContent;
        }
    }
}
