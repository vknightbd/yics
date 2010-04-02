using System;

namespace YICS.Representation
{
    public class Scalar : Node, IComparable<Scalar>
    {
        public Scalar() : this(null)
        {
        }

        public Scalar(object value) : this(value, new TagScalar())
        {
        }

        public Scalar(object value, Tag tag)
        {
            Value = value;
            Tag = tag;
            AnchorHandle = null;
        }

        public object Value { get; set; }

        #region override Equals
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            
            Scalar scalar = obj as Scalar;
            if ((object)scalar == null) return false;

            return this.Equals(scalar);
        }

        public bool Equals(Scalar scalar)
        {
            return this.Value.Equals(scalar.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
        #endregion

        #region IComparable<Scalar> Members

        public int CompareTo(Scalar other)
        {
            return CanonicalContent.CompareTo(other.CanonicalContent);
        }

        #endregion
    }
}
