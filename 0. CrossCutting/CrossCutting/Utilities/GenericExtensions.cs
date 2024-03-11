using System;

namespace Exxis.Addon.RegistroCompCCRR.CrossCutting.Utilities
{
    public static class GenericExtensions
    {
        public static void IsNotDefaultThen<T>(this T source, Action<T> action)
        {
            if (!source.Equals(default(T)))
                action(source);
        }

        public static void IsDefaultThen<T>(this T source, Action<T> action)
        {
            if (source.Equals(default(T)))
                action(source);
        }
    }
}