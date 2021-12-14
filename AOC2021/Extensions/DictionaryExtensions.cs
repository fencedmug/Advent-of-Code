using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    /// <summary>
    /// Helpers to reduce code clutter on solutions
    /// </summary>
    public static class DictionaryExtensions
    {
        public static void Increment<TKey>(this Dictionary<TKey, int> dict, TKey key)
        {
            dict.Increment(key, 1);
        }

        public static void Increment<TKey>(this Dictionary<TKey, ulong> dict, TKey key)
        {
            dict.Increment(key, 1);
        }

        public static void Increment<TKey>(this Dictionary<TKey, int> dict, TKey key, int value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = 0;

            dict[key] += value;
        }

        public static void Increment<TKey>(this Dictionary<TKey, ulong> dict, TKey key, ulong value)
        {
            if (!dict.ContainsKey(key))
                dict[key] = 0;

            dict[key] += value;
        }

        public static void CopyTo<TKey, TValue>(this Dictionary<TKey, TValue> source, Dictionary<TKey, TValue> destination)
        {
            foreach (var kvp in source)
            {
                destination[kvp.Key] = kvp.Value;
            }
        }
    }
}
