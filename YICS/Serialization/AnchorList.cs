using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YICS.Representation;

namespace YICS.Serialization
{
    internal class AnchorList
    {
        Dictionary<int, AnchorRecord> list { get; set; }
        int handleCounter;

        public AnchorList()
        {
            list = new Dictionary<int, AnchorRecord>();
            handleCounter = 1;
        }

        public AnchorRecord this[int key]
        {
            get
            {
                return list[key];
            }
        }

        public void Add(Node node)
        {
            var record = new AnchorRecord()
            {
                Node = node,
                Handle = "id" + handleCounter.ToString().PadLeft(3, '0'),
                HashCode = node.GetHashCode(),
                HasAlias = false,
            };

            list.Add(record.HashCode, record);
            node.AnchorHandle = record.Handle;

            handleCounter++;
        }

        public bool Contains(Node node)
        {
            return list.ContainsKey(node.GetHashCode());
        }

        public Alias GetAlias(Node node)
        {
            var record = list[node.GetHashCode()];
            record.HasAlias = true;
            return new Alias(record.Handle, record.Node);
        }

        public bool HasAlias(Node node)
        {
            int hashCode = node.GetHashCode();
            if (!list.ContainsKey(hashCode)) return false;

            var record = list[hashCode];
            return record.HasAlias;
        }
    }
}
