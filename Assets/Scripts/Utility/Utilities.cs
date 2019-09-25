using System.Linq;
using DefaultNamespace;

namespace Utility
{
    public static class Utilities
    {
        /// <summary>
        /// Splits and sanitizes argumentStr
        /// </summary>
        /// <param name="argumentStr">string to sanitize and split into arguments</param>
        /// <returns></returns>
        public static string[] SplitArgumentString(string argumentStr)
        {
            return argumentStr
                .Split(Globals.Separator)
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .ToArray();
        }
    }
}