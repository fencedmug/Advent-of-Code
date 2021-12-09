using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<TData>(this IEnumerable<TData> enumerable, Action<TData> call)
        {
            foreach (var e in enumerable)
            {
                call(e);
            }
        }
    }
}
