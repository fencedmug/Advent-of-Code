using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    public static class SpanExtensions
    {
        public static ReadOnlySpan<TData> Pick<TData>(this ref ReadOnlySpan<TData> span, int val)
        {
            var removed = span[0..val];
            span = span[val..];
            return removed;
        }
    }
}
