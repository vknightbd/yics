using System;

namespace YICS.Representation
{
    public abstract partial class Tag
    {
        public enum KindType { Unknown, Mapping, Sequence, Scalar };

        public string Name { get; set; }
        public abstract KindType Kind { get; }

        public string Prefix
        {
            get
            {
                if (!Name.StartsWith("tag:")) return null;

                int split = Name.LastIndexOf(':');
                if (split == 3) throw new InvalidOperationException("Unable to find tag URI prefix for an invalid tag name.");

                return Name.Substring(0, split);
            }
        }

        public string Suffix
        {
            get
            {
                if (!Name.StartsWith("tag:")) return Name;

                int split = Name.LastIndexOf(':');
                if (split == 3) return null;

                return Name.Substring(split + 1);
            }
        }

        public bool IsValidName()
        {
            if (!IsNameContainsOnlyURICharacters()) return false;

            if (IsLocalTag()) return true;

            if (IsURI()) return true;

            return false;
        }

        public bool IsCollection()
        {
            if (Kind == KindType.Unknown) throw new InvalidOperationException("Tag does not have a Kind set.");

            return Kind != KindType.Scalar;
        }

        public bool IsScalar()
        {
            if (Kind == KindType.Unknown) throw new InvalidOperationException("Tag does not have a Kind set.");

            return Kind == KindType.Scalar;
        }

        /// <summary>
        /// Returns a formatted string for content. Performed during Serialize.GetChearacterStream()
        /// </summary>
        /// <param name="indentWidth"></param>
        public abstract string PresentContent(Node node, Serialization.PresentationOptions options);

        /// <summary>
        /// Returns a formatted string for content. Required for Scalar nodes. Unused otherwise.
        /// </summary>
        /// <param name="node"></param>
        public abstract string CanonicalFormat(Node node);

        public override string ToString()
        {
            return Name;
        }

        private bool IsNameContainsOnlyURICharacters()
        {
            var uriChars = new System.Text.RegularExpressions.Regex("^(%[0-9a-fA-F][0-9a-fA-F]|[0-9a-zA-Z-#;/?:@&=+$,_.!~*'()[\\]])+$");
            return uriChars.IsMatch(Name);
        }

        private bool IsLocalTag()
        {
            return Name.StartsWith("!") && Name.Length > 1; // NOTE: local tags cannot contain !,[]{} characters in shorthand tag format; however, it is fine inside a verbatim tag format
        }

        private bool IsURI()
        {
            /* source: http://www.ietf.org/rfc/rfc2396.txt
             * 3.1 scheme component:
             * scheme = alpha *( alpha | digit | "+" | "-" | "." )
             */
            var startsWithValidSchemeColon = new System.Text.RegularExpressions.Regex("^[a-zA-Z][0-9a-zA-Z+-.]*:");
            return startsWithValidSchemeColon.IsMatch(Name);
        }
    }
}
