using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YICS.Representation
{
    public partial class Tag
    {
        public enum KindType { Mapping, Sequence, Scalar };

        public string Name { get; set; }
        public KindType Kind { get; set; }

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
