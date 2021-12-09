using AOC2021.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day09
    {
        // First solution for part one
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            var height = lines.Length;
            var width = lines[0].Length;

            var pts = new List<int>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var current = lines[y][x];
                    if (CheckIsLowest(lines, height, width, x, y, current))
                    {
                        // got to remember the characters are not parsed to int
                        // so minus '0' to get numerical value
                        pts.Add(current - '0');
                    }
                }
            }

            Cout.WriteLine($"Risk level: {pts.Select(x => x + 1).Sum()}");
        }

        private static bool CheckIsLowest(string[] lines, int height, int width, int x, int y, char current)
        {
            var left  = (x == 0)            || lines[y][x - 1] > current;
            var right = (x == (width - 1))  || lines[y][x + 1] > current;
            var top   = (y == 0)            || lines[y - 1][x] > current;
            var btm   = (y == (height - 1)) || lines[y + 1][x] > current;

            return left && right && top && btm;
        }

        // First solution for part two
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var height = lines.Length;
            var width = lines[0].Length;

            var numbers = new List<int>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var current = lines[y][x];
                    if (CheckIsLowest(lines, height, width, x, y, current))
                    {
                        var points = new HashSet<Point>();
                        FloodFill(points, lines, new Point(x, y), current);

                        numbers.Add(points.Count);
                    }
                }
            }

            var multiplied = numbers.OrderByDescending(x => x).Take(3).Aggregate((acc, val) => acc * val);
            Cout.WriteLine($"Sizes: {multiplied}");
        }

        // I heard it's called Flood Fill
        private static void FloodFill(HashSet<Point> points, string[] lines, Point point, char value)
        {
            // note: if i do not check for points.Contains(point), i will get stack overflow
            // quite unsure if this is a good solution
            var current = lines[point.Y][point.X];
            if (current == '9' || current < value || !points.Add(point)) return; 

            if (point.X != 0) 
                FloodFill(points, lines, point.Shift(-1, 0), current); 

            if (point.X < lines[0].Length - 1)
                FloodFill(points, lines, point.Shift(1, 0), current);

            if (point.Y != 0)
                FloodFill(points, lines, point.Shift(0, -1), current);

            if (point.Y < lines.Length - 1)
                FloodFill(points, lines, point.Shift(0, 1), current);
        }

        // let's try using a queue to prevent recursion
        public static void SolveTwo_v2_NoRecursion(string input)
        {
            var lines = input.IntoLines();
            var height = lines.Length;
            var width = lines[0].Length;

            var numbers = new List<int>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var current = lines[y][x];
                    if (CheckIsLowest(lines, height, width, x, y, current))
                    {
                        var points = FloodFill_NonRecursive(lines, new Point(x, y));
                        numbers.Add(points.Count);
                    }
                }
            }

            var multiplied = numbers.OrderByDescending(x => x).Take(3).Aggregate((acc, val) => acc * val);
            Cout.WriteLine($"Sizes: {multiplied}");
        }

        private static HashSet<Point> FloodFill_NonRecursive(string[] lines, Point point)
        {
            var points = new HashSet<Point>();
            var stack = new Stack<Point>();
            stack.Push(point);

            while (stack.TryPop(out point))
            {
                var current = lines[point.Y][point.X];
                if (current == '9' || !points.Add(point)) continue; 
                // apparently I do not have to compare values because basins are surrounded by '9'
                // ponder: what if that wasn't true?

                if (point.X != 0) 
                    stack.Push(point.Shift(-1, 0));

                if (point.X < lines[0].Length - 1)
                    stack.Push(point.Shift(1, 0));

                if (point.Y != 0)
                    stack.Push(point.Shift(0, -1));

                if (point.Y < lines.Length - 1)
                    stack.Push(point.Shift(0, 1));
            }

            return points;
        }

        // Additional exercises:
        //  - use Parallel.For because we are not modifying the original data (good candidate for paralleling)
        //  - what if we have multiple basins joined
        //  - will need some changes to make use of BDN
    }
}
