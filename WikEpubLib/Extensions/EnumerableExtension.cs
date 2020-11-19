using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WikEpubLib.Extensions
{
    public static class EnumerableExtension
    {
        public static async Task ForEachAsync<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
        {
            foreach (var item in enumerable)
            {
                await action(item);
            }
        }
    }
}