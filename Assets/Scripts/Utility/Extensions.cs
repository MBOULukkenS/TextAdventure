using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public static class Extensions
    {
        public static bool IsEqual<T>(this T[] array, T[] other)
        {
            return other.Length == array.Length && array.All(other.Contains);
        }
    }
}