using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// Extension functies voor allerlij klasses en objecten.
    /// </summary>
    public static class Extensions
    {
        public static bool IsEqual<T>(this T[] array, T[] other)
        {
            return other.Length == array.Length && array.All(other.Contains);
        }

        public static bool IsEqual<T>(this IEnumerable<T> enumerable, IEnumerable<T> other)
        {
            return enumerable
                .ToArray()
                .IsEqual(other.ToArray());
        }

        public static string ToActualString(this IEnumerable<char> enumerable)
        {
            return new string(enumerable.ToArray());
        }

        public static string Combine(this IEnumerable<string> enumerable)
        {
            return string.Join(Globals.Separator.ToActualString(), enumerable);
        }
    }
}