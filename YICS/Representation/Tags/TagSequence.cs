using System;
using System.Text;

namespace YICS.Representation
{
    public class TagSequence : Tag
    {
        public TagSequence()
        {
            Name = Tag.SecondaryNamespace + ":seq";
        }

        public TagSequence(string name)
        {
            Name = name;
        }

        public override Tag.KindType Kind { get { return KindType.Sequence; } }

        /// <summary>
        /// CanonicalFormat() converts a representational model direct to string.
        /// Note this method does not detect cyclical paths which causes stack overflow. Use YICS.Serializer instead.
        /// </summary>
        //[System.Obsolete("CanonicalFormat() converts a representational model direct to string, but does not detect cyclical paths which causes stack overflow. Use YICS.Serializer instead.")]
        public override string CanonicalFormat(Node node)
        {
            if (node.GetType() != typeof(Sequence) && !node.GetType().IsSubclassOf(typeof(Sequence)))
                throw new InvalidOperationException("TagSequence can only be applied to Sequence nodes.");

            if (!string.IsNullOrEmpty(node.AnchorHandle)) { return "DEBUG<" + node.AnchorHandle + ">"; }

            StringBuilder canonForm = new StringBuilder();

            if (!string.IsNullOrEmpty(node.AnchorHandle))
            {
                canonForm.AppendLine("&" + node.AnchorHandle);
            }

            foreach (Node n in (Sequence)node)
            {
                canonForm.Append("- ");
                canonForm.AppendLine(n.CanonicalContent.IndentContent()); // NOTE: possible stack overflow here if node contains cycles
            }

            return canonForm.ToString();
        }

        public override string PresentContent(Node node, Serialization.Serializer serializer)
        {
            if (node.GetType() != typeof(Sequence) && !node.GetType().IsSubclassOf(typeof(Sequence)))
                throw new InvalidOperationException("TagSequence can only be applied to Sequence nodes.");
         
            Sequence sequence = (Sequence)node;

            StringBuilder sb = new StringBuilder();
            foreach (Node n in sequence)
            {
                sb.Append("- ");
                sb.Append(serializer.GetCharacterStream(n).IndentContent(serializer.IndentWidth));
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}
