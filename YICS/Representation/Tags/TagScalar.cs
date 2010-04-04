using System;

namespace YICS.Representation
{
    public class TagScalar : Tag
    {
        public TagScalar()
        {
            Name = Tag.SecondaryNamespace + ":str";
        }

        public TagScalar(string name)
        {
            Name = name;
        }

        public override Tag.KindType Kind { get { return KindType.Scalar; } }

        public override string CanonicalFormat(Node node)
        {
            if (node.GetType() != typeof(Scalar) && !node.GetType().IsSubclassOf(typeof(Scalar)))
                throw new InvalidOperationException("TagScalar can only be applied to Scalar nodes.");

            return ((Scalar)node).Value.ToString();
        }

        public override string PresentContent(Node node, Serialization.PresentationOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
