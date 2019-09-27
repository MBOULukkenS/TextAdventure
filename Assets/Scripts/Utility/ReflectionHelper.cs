using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// Deze klasse helpt met Reflection
    /// </summary>
    public static class ReflectionHelper
    {
        public static IEnumerable<T> GetDerivingInstances<T>()
        {
            return GetDerivingTypes<T>()
                .Select(t => (T)Activator.CreateInstance(t));
        }

        public static IEnumerable<Type> GetDerivingTypes<T>()
        {
            return typeof(T).Assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(T)) && !t.IsAbstract && !t.IsInterface);
        }
    }
}