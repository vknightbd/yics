using System;
using System.Text;

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

            foreach (var kvp in (Mapping)node)
            {
                canonForm.Append("? ");
                canonForm.AppendLine(kvp.Key.CanonicalContent.IndentContent()); // NOTE: possible stack overflow here if node contains cycles
                canonForm.Append(": ");
                canonForm.AppendLine(kvp.Value.CanonicalContent.IndentContent()); // NOTE: possible stack overflow here if node contains cycles
            }

            return canonForm.ToString();
        }
    }
}
