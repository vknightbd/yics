using System.Collections.Generic;
using System;

namespace YICS.Representation
{
    public partial class Mapping : Node, IDictionary<Node, Node>
    {
        public virtual IEnumerable<Node> OrderedKeys
        {
            get
            {
                // check if keys are scalar (therefore sortable)
                bool isAllScalarKeys = true;
                foreach (Node key in Keys)
                {
                    if (key.GetType() != typeof(Alias) && key.IsCollection())
                    {
                        isAllScalarKeys = false;
                        break;
                    }
                }

                // don't sort unless all keys are scalar
                if (!isAllScalarKeys) return Keys;

                // sort keys
                List<Node> tmpKeys = new List<Node>();
                foreach (Node key in Keys)
                {
                    tmpKeys.Add(key);
                }

                tmpKeys.Sort(); // sort scalar keys by Scalar CompareTo (default string CompareTo)

                return tmpKeys;
            }
        }
    }
}
