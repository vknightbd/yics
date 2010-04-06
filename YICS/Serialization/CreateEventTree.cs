using System.Collections.Generic;
using YICS.Representation;

namespace YICS.Serialization
{
    public partial class Serializer
    {
        /// <summary>
        /// Add alias nodes to the Representation graph.  Note: This will modify your docRoot node you pass in.
        /// </summary>
        public Serializer CreateEventTree()
        {
            foreach (Node root in eventTreeRoots)
            {
                Serialize(root);
            }

            return this;
        }

        private void Serialize(Node node)
        {
            if (node.IsCollection())
            {
                anchorList.Add(node);

                if (node.Tag.Kind == Tag.KindType.Sequence)
                {
                    Sequence sequence = (Sequence)node;
                    SerializeSequence(sequence);
                }
                else if (node.Tag.Kind == Tag.KindType.Mapping)
                {
                    Mapping mapping = (Mapping)node;
                    SerializeMapping(mapping);

                }
            }
            else if (AliasLongScalars && node.IsScalar())
            {
                string content = node.CanonicalContent;
                if (content.Contains("\n") || content.Length > AliasLongScalarLength)
                {
                    anchorList.Add(node);
                }
            }
        }

        private void SerializeMapping(Mapping mapping)
        {

            var keys = new List<Node>(mapping.Keys);

            foreach (var node in keys)
            {
                Node key = node;
                Node value = mapping[key];

                if (anchorList.Contains(key))
                {
                    Alias aliasKey = anchorList.GetAlias(key);
                    mapping.Remove(key);
                    mapping.Add(aliasKey, value);
                    key = aliasKey;
                }
                else
                {
                    Serialize(key);
                }

                if (anchorList.Contains(value))
                {
                    Alias aliasValue = anchorList.GetAlias(value);
                    mapping[key] = aliasValue;
                }
                else
                {
                    Serialize(value);
                }
            }
        }

        private void SerializeSequence(Sequence sequence)
        {
            for (int i = 0; i < sequence.Count; i++)
            {
                Node item = sequence[i];
                if (anchorList.Contains(item))
                {
                    Alias alias = anchorList.GetAlias(item);

                    sequence.RemoveAt(i);
                    sequence.Insert(i, alias);
                }
                else
                {
                    Serialize(item);
                }
            }
        }
    }
}
