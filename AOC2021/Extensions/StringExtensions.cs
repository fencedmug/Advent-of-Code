using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021.Extensions
{
    public static class StringExtensions
    {
        public static string[] IntoLines(this string text) =>
            text.Split(Environment.NewLine, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public static string[] IntoWords(this string text) =>
            text.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        public static int[][] IntoIntArrays(this string text) =>
            text.IntoLines().Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();
    }
}
