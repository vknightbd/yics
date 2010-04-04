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
        public TagStyle UseTagStyle = TagStyle.Verbatim;
        public bool UseTagDirective = false; // shorten tags via tag handlers (for Shorthand) i.e. %TAG !e! tag:example.com,2000:app/

        public bool DetectMerge = false; // attempt to clean up mappings by detecting common elements and using merge keys http://yaml.org/type/merge.html

        private List<Node> eventTreeRoots;
        private AnchorList anchorList;

        public Serializer(Node docRoot)
        {
            if (docRoot == null) throw new System.ArgumentNullException("docRoot");

            eventTreeRoots = new List<Node>() { docRoot };
            anchorList = new AnchorList();
        }

        public Serializer(IEnumerable<Node> docList) {
            if (docList == null) throw new System.ArgumentNullException("docList");

            eventTreeRoots = new List<Node>(docList);
            if (eventTreeRoots.Count == 0)
            {
                eventTreeRoots.Add(new Scalar(null)); /* todo: use the Null node instead */
            }

            anchorList = new AnchorList();
        }
    }
}
