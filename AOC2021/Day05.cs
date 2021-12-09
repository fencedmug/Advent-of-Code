using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day05
    {
        public static void SolveOne(string input)
        {
            var mapCounter = new Dictionary<Point, int>();

            foreach (var line in input.IntoLines())
            {
                var endToEnd = GetEndToEnd(line);
                var Points = GetPointsV1(endToEnd);

                foreach (var pt in Points)
                {
                    if (mapCounter.ContainsKey(pt))
                    {
                        mapCounter[pt]++;
                    }
                    else
                    {
                        mapCounter[pt] = 1;
                    }
                }
            }

            var count = mapCounter.Count(kvp => kvp.Value > 1);
            Cout.WriteLine($"Points: {count}");
        }

        private static List<Point> GetEndToEnd(string line)
        {
            var options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
            var endToEnd = line.Split("->", options);

            var start = endToEnd[0].Split(",", options);
            var startPt = new Point() { X = int.Parse(start[0]), Y = int.Parse(start[1]) };

            var end = endToEnd[1].Split(",", options);
            var endPt = new Point() { X = int.Parse(end[0]), Y = int.Parse(end[1]) };

            return new List<Point> { startPt, endPt };
        }

        private static List<Point> GetPointsV1(List<Point> endToEnd)
        {
            var list = new List<Point>();

            if (endToEnd[0].X == endToEnd[1].X)
            {
                //handle horizontal line
                var delta = endToEnd[1].Y - endToEnd[0].Y;
                var sign = delta > 0 ? 1 : -1;
                for (int y = 0; y != delta; y = y + sign)
                {
                    list.Add(new Point { X = endToEnd[0].X, Y = endToEnd[0].Y + y });
                }
                list.Add(endToEnd[1]);
            }
            else if (endToEnd[0].Y == endToEnd[1].Y)
            {
                //handle vertical line
                var delta = endToEnd[1].X - endToEnd[0].X;
                var sign = delta > 0 ? 1 : -1;
                for (int x = 0; x != delta; x = x + sign)
                {
                    list.Add(new Point { X = endToEnd[0].X + x, Y = endToEnd[0].Y });
                }
                list.Add(endToEnd[1]);
            }

            return list;
        }

        public static void SolveTwo(string input)
        {
            var mapCounter = new Dictionary<Point, int>();

            foreach (var line in input.IntoLines())
            {
                var endToEnd = GetEndToEnd(line);
                var coords = GetPointsV2(endToEnd);

                foreach (var coord in coords)
                {
                    if (mapCounter.ContainsKey(coord))
                    {
                        mapCounter[coord]++;
                    }
                    else
                    {
                        mapCounter[coord] = 1;
                    }
                }
            }

            var count = mapCounter.Where(kvp => kvp.Value > 1).Count();
            Cout.WriteLine($"Points: {count}");
        }

        private static List<Point> GetPointsV2(List<Point> endToEnd)
        {
            var list = new List<Point>();

            if (endToEnd[0].X == endToEnd[1].X)
            {
                //handle horizontal line
                var delta = endToEnd[1].Y - endToEnd[0].Y;
                var sign = delta > 0 ? 1 : -1;
                for (int y = 0; y != delta; y = y + sign)
                {
                    list.Add(new Point { X = endToEnd[0].X, Y = endToEnd[0].Y + y });
                }
                list.Add(endToEnd[1]);
            }
            else if (endToEnd[0].Y == endToEnd[1].Y)
            {
                //handle vertical line
                var delta = endToEnd[1].X - endToEnd[0].X;
                var sign = delta > 0 ? 1 : -1;
                for (int x = 0; x != delta; x = x + sign)
                {
                    list.Add(new Point { X = endToEnd[0].X + x, Y = endToEnd[0].Y });
                }
                list.Add(endToEnd[1]);
            }
            else if (Math.Abs(endToEnd[0].X - endToEnd[1].X) == Math.Abs(endToEnd[0].Y - endToEnd[1].Y))
            {
                //handle diagonal line
                var deltaX = endToEnd[0].X - endToEnd[1].X;
                var deltaY = endToEnd[0].Y - endToEnd[1].Y;

                var signX = deltaX < 0 ? 1 : -1;
                var signY = deltaY < 0 ? 1 : -1;

                for (int inc = 0; inc != Math.Abs(deltaX); inc++)
                {
                    list.Add(new Point { X = endToEnd[0].X + (inc * signX), Y = endToEnd[0].Y + (inc * signY) });
                }

                list.Add(endToEnd[1]);
            }

            return list;
        }
    }
}
