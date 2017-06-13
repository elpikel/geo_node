using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace TagLife.Helpers
{
    public static class CollectionHelpers
    {
        public static ImmutableList<T> GetMissings<T>(this IReadOnlyList<T> old, IReadOnlyList<T> @new)
        {
            return old.Where(o => !@new.Contains(o)).ToImmutableList();

        }

        public static ImmutableList<T> GetNews<T>(this IReadOnlyList<T> old, IReadOnlyList<T> @new)
        {
            return @new.Where(n => !old.Contains(n)).ToImmutableList();
        }
    }
}