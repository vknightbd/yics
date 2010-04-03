using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YICS.Representation;

namespace YICS.Serialization
{
    public partial class Serializer
    {
        public int IndentWidth = 2;
        public bool UseVerbatimTags = true; // i.e. !<tag:yaml.org,2002:str> or !<!baz>
        public bool UserNonSpecificTags = false; // i.e. ! (for scalars) or (nothing for scalars/collections)
        public bool UseShorthandTags = false; // i.e. !!str or !baz
        public bool UserTagDirective = false; // shorten tags via tag handlers i.e. %TAG !e! tag:example.com,2000:app/

        public bool DetectMerge = false; // attempt to clean up mappings by detecting common elements and using merge keys http://yaml.org/type/merge.html

        private Node eventTreeRoot;
        private AnchorList anchorList;

        public void CreateEventTree(Node docRoot)
        {
            eventTreeRoot = docRoot;
            anchorList = new AnchorList();

            Serialize(docRoot);
        }

        private void Serialize(Node node) {
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
