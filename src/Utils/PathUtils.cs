using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jsunmap.Utils
{
    internal static class PathUtils
    {
        public static string RemoveInvalidFilenameCharacters(string input)
        {
            return InternalRemoveInvalidChar(Path.GetInvalidFileNameChars(),input);
        }
        
        public static string RemoveInvalidPathCharacters(string input)
        {
            return InternalRemoveInvalidChar(Path.GetInvalidPathChars() ,input);
        }
        public static string RemoveAllInvalidCharacters(string input)
        {
            return InternalRemoveInvalidChar(Path.GetInvalidFileNameChars().Concat(Path.GetInvalidPathChars()).ToArray(), input);
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
            return output.ToString();
        }
    }
}
