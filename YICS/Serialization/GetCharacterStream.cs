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
                //cs.AppendLine();
            }

            if (eventTreeRoots.Count > 1)
            {
                cs.AppendLine("...");
            }

            return cs.ToString();
        }

        internal string GetCharacterStream(Node node)
        {
            string property = string.Empty;

            // add node anchor handle
            if (!node.IsAlias() && anchorList.HasAlias(node))
            {
                property = "&" + node.AnchorHandle + " ";
                /*
                nodeCS.Append("&");
                nodeCS.Append(node.AnchorHandle);
                nodeCS.Append(" ");
                 */
            }

            // add node tag
            if (!node.IsAlias())
            {
                switch (UseTagStyle)
                {
                    case TagStyle.Verbatim:
                        property += node.Tag.Name;
                        /*
                        nodeCS.Append(node.Tag.Name);
                         */
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
                        if (node.Tag.Prefix == Tag.SecondaryNamespace)
                        {
                            property += "!!" + node.Tag.Suffix;
                            /*
                            nodeCS.Append("!!");
                            nodeCS.Append(node.Tag.Suffix);
                             */
                        }
                        else if (UseTagDirective && node.Tag.Name.StartsWith("tag:"))
                        {
                            property += "!" + GetTagHandler(node.Tag.Prefix) + "!" + node.Tag.Suffix;
                            /*
                            nodeCS.Append("!");
                            nodeCS.Append(GetTagHandler(node.Tag.Prefix));
                            nodeCS.Append("!");
                            nodeCS.Append(node.Tag.Suffix);
                             */
                        }
                        else
                        {
                            property += node.Tag.Name;
                            /*
                            nodeCS.Append(node.Tag.Name); // local tags or full uris
                             */
                        }

                        break;

                    default:
                        throw new System.NotImplementedException("Unknown tag style " + UseTagStyle.ToString() + ".");
                }
            }

            // add node properties to character stream
            StringBuilder cs = new StringBuilder();
            bool hasProperty = property.Length > 0;
            if (hasProperty)
                cs.Append(property);

            // line break after directive end marker and/or node properties
            if (onDirectiveLine)
            {
                cs.AppendLine();
                onDirectiveLine = false;
                cs.Append(node.PresentContent(this)); // insert content immediately on line after directive end marker ---
            }
            else
            {
                string content = node.PresentContent(this); // gotta calculate length and find \n in content to determine line break after properties

                if (hasProperty && (property.Length + content.Length > LineLengthBreakOnTag || content.Contains("\n")))
                {
                    cs.AppendLine();
                }
                else
                {
                    if (hasProperty && !property.EndsWith(" "))
                        cs.Append(" ");
                }

                cs.Append(content);
            }

            return cs.ToString();
        }
    }
}
