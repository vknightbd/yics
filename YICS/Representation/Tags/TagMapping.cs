using System;
using System.Text;
using System.Collections.Generic;

namespace YICS.Representation
{
    public class TagMapping : Tag
    {
        public TagMapping()
        {
            Name = Tag.SecondaryNamespace + ":map";
        }

        public TagMapping(string name)
        {
            Name = name;
        }

        public override Tag.KindType Kind { get { return KindType.Mapping; } }

        /// <summary>
        /// CanonicalFormat() converts a representational model direct to string.
        /// Note this method does not detect cyclical paths which causes stack overflow. Use YICS.Serializer instead.
        /// </summary>
        //[System.Obsolete("CanonicalFormat() converts a representational model direct to string, but does not detect cyclical paths which causes stack overflow. Use YICS.Serializer instead.")]
        public override string CanonicalFormat(Node node)
        {
            if (node.GetType() != typeof(Mapping) && !node.GetType().IsSubclassOf(typeof(Mapping)))
                throw new InvalidOperationException("TagMapping can only be applied to Mapping nodes.");

            StringBuilder canonForm = new StringBuilder();

            if (!string.IsNullOrEmpty(node.AnchorHandle))
            {
                canonForm.AppendLine("&" + node.AnchorHandle);
            }

            foreach (var kvp in (Mapping)node)
            {
                canonForm.Append("? ");
                canonForm.AppendLine(kvp.Key.CanonicalContent.IndentContent()); // NOTE: possible stack overflow here if node contains cycles
                canonForm.Append(": ");
                canonForm.AppendLine(kvp.Value.CanonicalContent.IndentContent()); // NOTE: possible stack overflow here if node contains cycles
            }

            return canonForm.ToString();
        }

        public virtual IEnumerable<Node> OrderKeys(Mapping node)
        {
            // check if keys are scalar (therefore sortable)
            bool isAllScalarKeys = true;
            foreach (Node key in node.Keys)
            {
                if (key.GetType() != typeof(Alias) && key.IsCollection())
                {
                    isAllScalarKeys = false;
                    break;
                }
            }

            // don't sort unless all keys are scalar
            if (!isAllScalarKeys) return node.Keys;

            // sort keys
            List<Node> tmpKeys = new List<Node>();
            foreach (Node key in node.Keys)
            {
                tmpKeys.Add(key);
            }

            tmpKeys.Sort(); // sort scalar keys by Scalar CompareTo (default string CompareTo)

            return tmpKeys;
        }

        public override string PresentContent(Node node, Serialization.PresentationOptions options)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(CanonicalFormat(node));
            return sb.ToString();
        }
    }
}
