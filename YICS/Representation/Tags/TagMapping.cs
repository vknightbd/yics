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

            if (!string.IsNullOrEmpty(node.AnchorHandle)) { return "DEBUG<" + node.AnchorHandle + ">"; }

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

            // don't sort unless all keys are scalar /* todo: not scalar, but check if they fit on one line, less than 30 chars */
            if (!isAllScalarKeys) return node.Keys;

            // sort keys
            List<Node> tmpAlias = new List<Node>();
            List<Node> tmpKeys = new List<Node>();
            foreach (Node key in node.Keys)
            {
                if (key.IsAlias())
                {
                    tmpAlias.Add(key);
                }
                else
                {
                    tmpKeys.Add(key);
                }
            }

            tmpAlias.Sort();
            tmpKeys.Sort(); // sort scalar keys by Scalar CompareTo (default string CompareTo)

            tmpKeys.InsertRange(0, tmpAlias);
            return tmpKeys;
        }

        public override string PresentContent(Node node, Serialization.Serializer serializer)
        {
             if (node.GetType() != typeof(Mapping) && !node.GetType().IsSubclassOf(typeof(Mapping)))
                throw new InvalidOperationException("TagMapping can only be applied to Mapping nodes. This node is of " + node.GetType().ToString());
            
            Mapping mapping = (Mapping)node;
            int maxKeyLength = 0;

            // check if keys are scalar (therefore sortable)
            bool isShortKeys = true;
            foreach (Node key in mapping.Keys)
            {
                if (!key.IsAlias() && !key.IsScalar())
                {
                    isShortKeys = false;
                    break;
                }

                string keyContent;
                if (key.IsAlias())
                {
                    keyContent = "*" + key.AnchorHandle;
                }
                else
                {
                    keyContent = key.CanonicalContent;
                }

                if (keyContent.Contains("\n"))
                {
                    isShortKeys = false;
                    break;
                }

                int keyLength = keyContent.Length;
                if (keyLength > maxKeyLength)
                    maxKeyLength = keyLength;
            }

            IEnumerable<Node> keys;
            if (isShortKeys)
            {
                keys = OrderKeys(mapping);
            }
            else
            {
                keys = mapping.Keys;
            }

            StringBuilder sb = new StringBuilder();
            foreach (Node key in keys)
            {
                Node value = mapping[key];

                if (!isShortKeys)
                    sb.Append("? ");

                string keyName = serializer.GetCharacterStream(key).IndentContent(serializer.IndentWidth); /* todo check if key has : symbol */
                if (serializer.MappingAlignColons)
                {
                    keyName = keyName.PadRight(maxKeyLength);
                }

                sb.Append(keyName);

                if (!isShortKeys)
                    sb.AppendLine();

                sb.Append(": ");

                if (!value.IsScalar() && !value.IsAlias())
                    sb.AppendLine();

                sb.Append(serializer.GetCharacterStream(value).IndentContent(serializer.IndentWidth));

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
