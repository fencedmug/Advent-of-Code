using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace AOC2021
{
    public class Day13
    {
        // Tests: 17; Inputs: 751
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            var points = new HashSet<Point>();

            foreach (var line in lines)
            {
                if (!line.Contains("fold"))
                {
                    var splits = line.Split(',');
                    points.Add(new Point(int.Parse(splits[0]), int.Parse(splits[1])));
                    continue;
                }

                var fold = line.Substring(11).Split('='); //"fold along " = 11 chars
                var num = int.Parse(fold[1]);

                foreach (var pt in points.ToList())
                {
                    if (fold[0] == "x" && pt.X > num)
                    {
                        points.Remove(pt);
                        points.Add(pt.Shift(-2 * (pt.X - num), 0));
                    }
                    else if (fold[0] == "y" && pt.Y > num)
                    {
                        points.Remove(pt);
                        points.Add(pt.Shift(0, -2 * (pt.Y - num)));
                    }
                }

                Cout.WriteLine($"Total points: {points.Count}");
                return; // fold once
            }
        }

        // Inputs: PGHRKLKL
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var points = new HashSet<Point>();

            int maxX = int.MaxValue;
            int maxY = int.MaxValue;

            foreach (var line in lines)
            {
                if (!line.Contains("fold"))
                {
                    var splits = line.Split(',');
                    points.Add(new Point(int.Parse(splits[0]), int.Parse(splits[1])));
                    continue;
                }

                var fold = line[11..].Split('='); //"fold along " = 11 chars
                var num = int.Parse(fold[1]);
                maxX = fold[0] == "x" ? Math.Min(num, maxX) : maxX;
                maxY = fold[0] == "y" ? Math.Min(num, maxY) : maxY;

                foreach (var pt in points.ToList())
                {
                    if (fold[0] == "x" && pt.X > num)
                    {
                        points.Remove(pt);
                        points.Add(pt.Shift(-2 * (pt.X - num), 0));
                    }
                    else if (fold[0] == "y" && pt.Y > num)
                    {
                        points.Remove(pt);
                        points.Add(pt.Shift(0, -2 * (pt.Y - num)));
                    }
                }
            }

            var pad = new char[maxY][];
            for (var i = 0; i < pad.Length; i++)
            {
                pad[i] = new char[maxX];
                for (var j = 0; j < pad[i].Length; j++)
                    pad[i][j] = '.';
            }

            foreach (var point in points)
                pad[point.Y][point.X] = '█';

            for (var i = 0; i < pad.Length; i++)
                Cout.WriteLine($"{string.Join("", pad[i])}");
        }

        // todo:
        // each fold is a transformation
        // read lines for folding first and building a list of transformations (lambdas)
        // then read each point and perform the transformation
        // might be worth it see if the transformations can be "compressed" (math?)

        // todo:
        // initialize the final array
        // read folding into list of transforms or compress to single transform (not sure if it's possible)
        // transform each point when reading and directly place on array (avoid creating Point)
        // print array
    }
}
