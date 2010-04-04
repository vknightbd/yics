
namespace YICS.Representation
{
    public static class StringExtensions
    {
        /// <summary>Trims end of string of whitespace and indents lines after first by two spaces.</summary>
        public static string IndentContent(this string s)
        {
            return s.TrimEnd().Replace("\n", "\n  ");
        }

        /// <summary>Trims end of string of whitespace and indents lines after first by two spaces.</summary>
        public static string IndentContent(this string s, int indent)
        {
            return s.TrimEnd().Replace("\n", "\n".PadRight(indent + 1, ' '));
        }
    }
}
