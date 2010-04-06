using YICS.Representation;
using System.Collections.Generic;

namespace YICS.Serialization
{
    public partial class Serializer
    {
        public enum TagStyle { Verbatim, NonSpecific, Shorthand };
        /* Verbatim - !<tag:yaml.org,2002:str> or !<!baz>
         * NonSpecific - ! (for scalars) or (nothing for scalars/collections)
         * Shorthand - !!str or !baz
         */


        public int IndentWidth = 2;
        public bool UseYAML12Directive = true;
        public TagStyle UseTagStyle = TagStyle.NonSpecific;
        public bool UseTagDirective = false; // shorten tags via tag handlers (for Shorthand) i.e. %TAG !e! tag:example.com,2000:app/
        public bool MappingAlignColons = true;
        public int LineLengthBreakOnTag = 50; // line length 

        public bool AliasLongScalars = true;
        public int AliasLongScalarLength = 40; // length of scalars 

        public bool DetectMerge = false; // attempt to clean up mappings by detecting common elements and using merge keys http://yaml.org/type/merge.html

        private List<Node> eventTreeRoots;
        private AnchorList anchorList;
        private Dictionary<string, string> tagPrefixes;

        public Serializer(Node docRoot) : this(new List<Node> { docRoot }) { }

        public Serializer(Node[] docList) : this(new List<Node>(docList)) { } // hack to take in a list of nodes, can't use IEnumerable<Node> since Sequence node is an IEnumerable

        public Serializer(List<Node> docList) {
            if (docList == null) throw new System.ArgumentNullException("docList");

            eventTreeRoots = new List<Node>(docList);
            if (eventTreeRoots.Count == 0)
            {
                eventTreeRoots.Add(new Scalar(null)); /* todo: use the Null node instead */
            }

            anchorList = new AnchorList();
            tagPrefixes = new Dictionary<string, string>();
        }

        private string GetTagHandler(string tagPrefix)
        {
            if (tagPrefixes.ContainsKey(tagPrefix))
            {
                return tagPrefixes[tagPrefix];
            }
            else
            {
                string newTagPrefix = 't' + tagPrefixes.Count.ToString().PadLeft(2, '0');
                tagPrefixes.Add(tagPrefix, newTagPrefix);
                return newTagPrefix;
            }
        }
    }
}
