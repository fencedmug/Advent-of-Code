using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day11
    {
        public static void SolveOne(string input)
        {
            var octo = input.IntoIntArrays();

            var total = 0;
            for (var i = 0; i < 100; i++)
            {
                total += GetNumberOfFlashes(octo);
            }

            Cout.WriteLine($"Flashes = {total}");
        }

        // note to self: once an octopus has flashed, it cannot be incremented in the same step
        // adjacent octopus's flash will not energize an octopus that has already flashed
        private static int GetNumberOfFlashes(int[][] octo)
        {
            var height = octo.Length;
            var width = octo[0].Length;
            
            var toFlash = new Queue<Point>();
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (octo[y][x] == 9)
                    {
                        toFlash.Enqueue(new Point(x, y));
                        continue;
                    }

                    octo[y][x]++;
                }
            }

            var flashed = new HashSet<Point>();
            while (toFlash.TryDequeue(out var pt))
            {
                if (!flashed.Add(pt))
                    continue;

                octo[pt.Y][pt.X] = 0;

                foreach (var nb in pt.GetAllNeighbours(width, height))
                {
                    if (flashed.Contains(nb))
                        continue;

                    if (octo[nb.Y][nb.X] == 9)
                    {
                        toFlash.Enqueue(nb);
                        continue;
                    }

                    octo[nb.Y][nb.X]++;
                }
            }
            return flashed.Count;
        }

        public static void SolveTwo(string input)
        {
            var octo = input.IntoIntArrays();
            var totalOcts = octo.Length * octo[0].Length;

            for (var i = 1; ; i++)
            {
                var flashes = GetNumberOfFlashes(octo);
                if (flashes == totalOcts)
                {
                    Cout.WriteLine($"First time all flashed: {i} steps");
                    break;
                }
            }
        }
    }
}
