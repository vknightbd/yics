using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YICS.Representation
{
    public class TagMapping:Tag
    {
                public TagMapping()
        {
            Name = Tag.SecondaryNamespace + ":map";
        }

                public TagMapping(string name)
        {
            Name = name;
        }

        public override string CanonicalFormat(Node node)
        {
            if (node.GetType() != typeof(Mapping) && !node.GetType().IsSubclassOf(typeof(Mapping)))
                throw new InvalidOperationException("TagMapping can only be applied to Mapping nodes.");

            StringBuilder canonForm = new StringBuilder();

            foreach (var kvp in (Mapping)node)
            {
                canonForm.Append("? ");
                canonForm.AppendLine(kvp.Key.CanonicalContent.IndentContent());
                canonForm.Append(": ");
                canonForm.AppendLine(kvp.Value.CanonicalContent.IndentContent());
            }

            return canonForm.ToString();
        }
    }
}
