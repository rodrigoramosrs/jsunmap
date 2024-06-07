using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsunmap.Utils
{
    internal static class PathUtils
    {
        private static char[] AllInvalidChars = Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray();
        private static char[] PathInvalidChars = Path.GetInvalidPathChars();
        private static char[] FileInvalidChars = Path.GetInvalidFileNameChars();
        public static string RemoveInvalidFilenameCharacters(string input)
        {
            return InternalRemoveInvalidChar(FileInvalidChars, input);
        }

        public static string RemoveInvalidPathCharacters(string input)
        {
            return InternalRemoveInvalidChar(PathInvalidChars, input)
                ;
        }
        public static string RemoveAllInvalidCharacters(string input)
        {
            return InternalRemoveInvalidChar(AllInvalidChars, input);
        }
        private static string InternalRemoveInvalidChar(char[] invalidChars, string input)
        {
            var output = new StringBuilder(input.Length);
            foreach (var c in input)
            {
                if (!invalidChars.Contains(c))
                {
                    output.Append(c);
                }
            }
            return output.ToString()
                .Replace("%20", "_")
                .Replace("^", string.Empty)
                .Replace("$", string.Empty)
                .Replace("*", string.Empty) ;
        }
    }
}
