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

        public override string CanonicalFormat(Node node)
        {
            if (node.GetType() != typeof(Sequence) && !node.GetType().IsSubclassOf(typeof(Sequence)))
                throw new InvalidOperationException("TagSequence can only be applied to Sequence nodes.");

            StringBuilder canonForm = new StringBuilder();

            foreach (Node n in (Sequence)node)
            {
                canonForm.Append("- ");
                canonForm.AppendLine(n.CanonicalContent.IndentContent());
            }

            return canonForm.ToString();
        }
    }
}
