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

                    var aliasList = new List<Node>();
                    foreach (Node item in sequence)
                    {
                        if (anchorList.Contains(item))
                        {
                            aliasList.Add(item);
                        }
                        else
                        {
                            Serialize(item);
                        }
                    }

                    foreach (Node item in aliasList)
                    {
                        Alias alias = anchorList.GetAlias(item);

                        int index = sequence.IndexOf(node);
                        sequence.Remove(item);
                        sequence.Insert(index, alias);
                    }
                }
                else if (node.Tag.Kind == Tag.KindType.Mapping)
                {
                    Mapping mapping = (Mapping)node;

                    var aliasKeys = new List<Node>();

                    foreach (var kvp in mapping)
                    {
                        if (anchorList.Contains(kvp.Key))
                        {
                            aliasKeys.Add(kvp.Key);
                        }
                        else
                        {
                            Serialize(kvp.Key);
                        }

                        if (anchorList.Contains(kvp.Value))
                        {
                            mapping[kvp.Key] = anchorList.GetAlias(kvp.Value);
                        }
                        else
                        {
                            Serialize(kvp.Value);
                        }
                    }

                    foreach (Node anchorKey in aliasKeys)
                    {
                        Alias key = anchorList.GetAlias(anchorKey);
                        Node value = mapping[anchorKey];
                        mapping.Remove(anchorKey);
                        mapping.Add(key, value);
                    }
                }
            }
        }
    }
}
