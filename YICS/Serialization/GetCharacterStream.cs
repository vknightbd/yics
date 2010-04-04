using System.Collections.Generic;
using YICS.Representation;
using System.Text;

namespace YICS.Serialization
{
    public partial class Serializer
    {
        private bool onDirectiveLine;

        public string GetCharacterStream()
        {
            StringBuilder cs = new StringBuilder();

            /* layout:
             * directives
             * --- root-seq-tags
             * - child-scalar-tags child-scalar
             * - child-scalar-tags child-scalar
             * - child-mapping-tags
             *   child-key : child-value
             *   child-key : child-value
             *   child-key-tags child-key : child-value-tags child-value
             * - !!null
             * - child-key : child-value # compact block mapping
             *   child-key : child-value
             * ...
             * directives
             * --- root node tags
             * etc...
             */

            for (int i = 0; i < eventTreeRoots.Count; i++)
            {
                onDirectiveLine = true;
                if (i == 0)
                {
                    // add directives at top
                    if (UseYAML12Directive)
                    {
                        cs.AppendLine("%YAML 1.2");
                    }

                    if (UseTagDirective)
                    {
                        /* todo: add document specific directives */
                    }

                    if (UseYAML12Directive || UseTagDirective || eventTreeRoots.Count > 1)
                    {
                        cs.Append("--- ");
                    }
                }
                else
                {
                    cs.AppendLine("...");

                    if (UseTagDirective)
                    {
                        /* todo: add document specific directives */
                    }

                    cs.Append("--- ");
                }

                // add each document
                cs.Append(GetCharacterStream(eventTreeRoots[i]));
                cs.AppendLine();
            }

            if (eventTreeRoots.Count > 1)
            {
                cs.AppendLine("...");
            }

            return cs.ToString();
        }

        private string GetCharacterStream(Node node)
        {
            StringBuilder nodeCS = new StringBuilder();
            bool hasProperty = false;

            // add node anchor handle
            if (anchorList.HasAlias(node))
            {
                nodeCS.Append("&");
                nodeCS.Append(node.AnchorHandle);
                nodeCS.Append(" ");
                hasProperty = true;
            }

            // add node tag
            switch (UseTagStyle)
            {
                case TagStyle.Verbatim:
                    nodeCS.Append(node.Tag.Name);
                    nodeCS.Append(" ");
                    hasProperty = true;
                    break;

                case TagStyle.NonSpecific:
                    bool isFailsafeSchema;
                    switch (node.Tag.Suffix)
                    {
                        case "str":
                        case "seq":
                        case "map":
                            isFailsafeSchema = true; break;
                        default:
                            isFailsafeSchema = false; break;
                    }

                    if (!isFailsafeSchema)
                        goto case TagStyle.Shorthand;
                    else
                        break;

                case TagStyle.Shorthand:
                    if (UseTagDirective)
                    {
                        /* todo: store all prefixes and insert in directive spot */
                    }

                    if (node.Tag.Prefix == Tag.SecondaryNamespace)
                    {
                        nodeCS.Append("!!");
                        nodeCS.Append(node.Tag.Suffix);
                    }
                    else
                    {
                        nodeCS.Append(node.Tag.Name); // local tags or full uris  /* todo: remove full uri if UseTagDirective = true */
                    }

                    nodeCS.Append(" ");
                    hasProperty = true;
                    break;

                default:
                    throw new System.NotImplementedException("Unknown tag style " + UseTagStyle.ToString() + ".");
            }

            // node options
            var options = new PresentationOptions
            {
                IndentWidth = Tag.DefaultIndentWidth,
                HasProperty = hasProperty
            };

            // line break after directive end marker and/or node properties
            if (onDirectiveLine)
            {
                nodeCS.AppendLine();
                onDirectiveLine = false;
            }
            else if (hasProperty)
            {
                nodeCS.AppendLine();
                nodeCS.Append(' ', IndentWidth);
            }

            switch (node.Tag.Kind)
            {   
                case Tag.KindType.Mapping:
                    nodeCS.Append(node.PresentContent(options));
                    break;
                case Tag.KindType.Sequence:
                    nodeCS.Append(((Sequence)node).CanonicalContent);
                    break;
                case Tag.KindType.Scalar:
                    nodeCS.Append(((Scalar)node).CanonicalContent);
                    break;
                default:
                    throw new System.NotImplementedException("Cannot present node of type " + node.Tag.Kind.ToString() + ".");
            }

            return nodeCS.ToString();
        }
    }
}
