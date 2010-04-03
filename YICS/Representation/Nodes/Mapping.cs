using System.Collections.Generic;

namespace YICS.Representation
{
    public partial class Mapping : Node, IDictionary<Node, Node>
    {
        public Mapping()
            : this(new TagMapping())
        {
        }

        public Mapping(Tag tag)
        {
            Tag = tag;
            AnchorHandle = null;
            dictionary = new Dictionary<Node, Node>();
        }

        /// <summary>Add nodes to mapping but will add Alias instead if key node is already in the mapping.</summary>
        public void AddAsAliasIfDuplicate(Node key, Node value) {
            throw new System.NotImplementedException();
        }

        protected Dictionary<Node, Node> dictionary;

        #region IDictionary<Node,Node> Members

        public void Add(Node key, Node value)
        {
            dictionary.Add(key, value);
        }

        public void Add(object key, Node value) { Add(new Scalar(key), value); }
        public void Add(object key, object value) { Add(new Scalar(key), new Scalar(value)); }

        public bool ContainsKey(Node key)
        {
            return dictionary.ContainsKey(key);
        }

        public ICollection<Node> Keys
        {
            get { return dictionary.Keys; }
        }

        public bool Remove(Node key)
        {
            return dictionary.Remove(key);
        }

        public bool TryGetValue(Node key, out Node value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        public ICollection<Node> Values
        {
            get { return dictionary.Values; }
        }

        public Node this[Node key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<Node,Node>> Members

        public void Add(KeyValuePair<Node, Node> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<Node, Node> item)
        {
            if (!dictionary.ContainsKey(item.Key)) return false;
            return dictionary[item.Key].Equals(item.Value);
        }

        public void CopyTo(KeyValuePair<Node, Node>[] array, int arrayIndex)
        {
            foreach (var kvp in dictionary)
            {
                array[arrayIndex] = kvp;
                arrayIndex++;
            }
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<Node, Node> item)
        {
            var value = dictionary[item.Key];
            if (value.Equals(item.Value))
            {
                return dictionary.Remove(item.Key);
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<Node,Node>> Members

        public IEnumerator<KeyValuePair<Node, Node>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        #endregion

        #region override Equals
        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            Mapping mapping = obj as Mapping;
            if ((object)mapping == null) return false;

            return this.Equals(mapping);
        }

        public bool Equals(Mapping mapping)
        {
            if (this == mapping) return true; // if object references are identical, no need to check values

            if (this.Count != mapping.Count) return false;

            foreach (Node key in this.Keys)
            {
                if (!mapping.ContainsKey(key)) return false;
            }

            foreach (Node key in this.Keys)
            {
                if (!this[key].Equals(mapping[key])) return false;
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
