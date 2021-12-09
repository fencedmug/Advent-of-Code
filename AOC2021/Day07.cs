using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day07
    {
        public static void SolveOne(string input)
        {
            var positions = input
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x));

            var min = int.MaxValue;
            var max = int.MinValue;
            var map = new Dictionary<int, int>();

            foreach (var pos in positions)
            {
                if (!map.ContainsKey(pos))
                    map[pos] = 0;

                map[pos]++;
                min = Math.Min(min, pos);
                max = Math.Max(max, pos);
            }

            var leastMoves = int.MaxValue;
            for (int i = positions.Min(); i <= positions.Max(); i++)
            {
                var temp = 0;
                foreach (var kvp in map)
                {
                    temp += Math.Abs(kvp.Key - i) * kvp.Value;
                }

                leastMoves = Math.Min(leastMoves, temp);
            }

            Cout.WriteLine($"Least moves: {leastMoves}");
        }

        public static void SolveTwo(string input)
        {
            var positions = input
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => int.Parse(x));

            var min = int.MaxValue;
            var max = int.MinValue;
            var map = new Dictionary<int, int>();

            foreach (var pos in positions)
            {
                if (!map.ContainsKey(pos))
                    map[pos] = 0;

                map[pos]++;
                min = Math.Min(min, pos);
                max = Math.Max(max, pos);
            }

            var leastMoves = int.MaxValue;
            for (int i = positions.Min(); i <= positions.Max(); i++)
            {
                var temp = 0;
                foreach (var kvp in map)
                {
                    var delta = Math.Abs(kvp.Key - i);
                    var moves = (delta + 1) * delta / 2;
                    temp += moves * kvp.Value;
                }

                leastMoves = Math.Min(leastMoves, temp);
            }

            Cout.WriteLine($"Least moves: {leastMoves}");
        }

        public static void SolveOne_UsingMedian(string input)
        {
            var positions = input
                   .Split(",", StringSplitOptions.RemoveEmptyEntries)
                   .Select(x => int.Parse(x))
                   .ToArray();

            Array.Sort(positions);

            var mIndex = (int)Math.Ceiling(positions.Length / 2.0);
            var median = positions[mIndex];

            var moves = positions.Select(x => Math.Abs(x - median)).Sum();
            Cout.WriteLine($"Moves: {moves}");
        }

        public static void SolveTwo_UsingMean(string input)
        {
            var positions = input
                    .Split(",", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => long.Parse(x))
                    .ToArray();

            var avg = (double)positions.Sum() / positions.Length;
            var ceiling = (int)Math.Ceiling(avg);
            var floor = (int)Math.Floor(avg);

            var gaussFormula = new Func<long, long>(x => ((x + 1L) * x / 2L));

            var cMoves = positions.Select(x => gaussFormula(Math.Abs(x - ceiling))).Sum();
            var fMoves = positions.Select(x => gaussFormula(Math.Abs(x - floor))).Sum();

            Cout.WriteLine($"CMoves: {cMoves}; FMoves: {fMoves}; Least: {Math.Min(cMoves, fMoves)}");
        }
    }
}
