﻿using System.Collections.Generic;

namespace YICS.Representation
{
    public class Sequence : Node, IList<Node>
    {
        public Sequence() : this(new TagSequence())
        {
        }

        public Sequence(Tag tag)
        {
            Tag = tag;
            AnchorHandle = null;
            list = new List<Node>();
        }

        private List<Node> list { get; set; }

        #region AddWithCheck
        public override int GetNodeHashCode(ref List<int> existingNodeHashCodes)
        {
            var sb = new System.Text.StringBuilder();
            foreach (Node item in list)
            {
                int itemHashCode = item.GetHashCode();
                if (existingNodeHashCodes.Contains(itemHashCode))
                {
                    sb.Append(itemHashCode);
                }
                else
                {
                    existingNodeHashCodes.Add(itemHashCode);
                    sb.Append(item.GetNodeHashCode(ref existingNodeHashCodes));
                }

                sb.AppendLine();
            }

            return sb.ToString().GetHashCode();
        }
        #endregion

        #region IList<Node> Members

        public int IndexOf(Node item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, Node item)
        {
            list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        public Node this[int index]
        {
            get
            {
                return list[index];
            }
            set
            {
                list[index] = value;
            }
        }

        #endregion

        #region ICollection<Node> Members

        public void Add(Node item)
        {
            list.Add(item);
        }

        public void Add(object value) { list.Add(new Scalar(value)); }

        public void Clear()
        {
            list.Clear();
        }

        public bool Contains(Node item)
        {
            return list.Contains(item);
        }

        public void CopyTo(Node[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(Node item)
        {
            return list.Remove(item);
        }

        #endregion

        #region IEnumerable<Node> Members

        public IEnumerator<Node> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        #endregion

        #region override Equals
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Sequence sequence = obj as Sequence;
            if ((object)sequence == null) return false;

            return this.Equals(sequence);
        }

        public bool Equals(Sequence sequence)
        {
            if (this == sequence) return true; // if object references are identical, no need to check values

            if (this.Count != sequence.Count) return false;

            for (int i = 0; i < this.Count; i++)
            {
                if (!this[i].Equals(sequence[i])) return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
