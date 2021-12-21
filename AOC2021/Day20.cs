using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021;

public class Day20
{
    // Answer: 35 (test); 5249 (inputs)
    public static void SolveOne(string input)
    {
        var lines = input.IntoLines();
        var algorithmKey = lines[0];
        var lightPts = GetLightPoints(lines);

        for (var steps = 0; steps < 2; steps++)
        {
            lightPts = RunStep(algorithmKey, lightPts, steps % 2 == 0); 
            Cout.WriteLine($"Step {steps}; No. {lightPts.Count}");
        }
    }

    // Answer: 3351 (test); 15714 (inputs)
    public static void SolveTwo(string input)
    {
        var lines = input.IntoLines();
        var algorithmKey = lines[0];
        var lightPts = GetLightPoints(lines);

        for (var steps = 0; steps < 50; steps++)
            lightPts = RunStep(algorithmKey, lightPts, steps % 2 == 0);

        Cout.WriteLine($"No. {lightPts.Count}");
    }

    private static HashSet<Point> GetLightPoints(string[] lines)
    {
        var lightPoints = new HashSet<Point>();
        var height = lines.Length - 1;  // first line is algorithm
        var width = lines[1].Length;    // assuming all rows are equal

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (lines[y + 1][x] == '#')
                    lightPoints.Add(new Point(x, y));
            }
        }

        return lightPoints;
    }

    // Solution inspired by help from reddit
    private static HashSet<Point> RunStep(string algorithm, HashSet<Point> lightPts, bool ignoreUniverse = true)
    {
        // todo: figure out why I need 2 here (1 doesn't solve the problem)
        var expand = 2; // seen some examples with expansion to left by 1 and right by 2

        // todo: get an extension/helper to calculate min/max in a single loop
        var minY = lightPts.Min(pt => pt.Y);
        var maxY = lightPts.Max(pt => pt.Y);
        var minX = lightPts.Min(pt => pt.X);
        var maxX = lightPts.Max(pt => pt.X);

        var points = new HashSet<Point>();
        for (var y = minY - expand; y < maxY + expand; y++)
        {
            for (var x = minX - expand; x < maxX + expand; x++)
            {
                var dx = new int[] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
                var dy = new int[] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
                var strBin = string.Empty;
                for (var i = 0; i < dx.Length; i++)
                {
                    var cx = x + dx[i];
                    var cy = y + dy[i];
                    // vvv 1st condition: when we don't need to care about lights flipping every turn
                    //                    vvv 2nd condition: when algo doesn't flip the lights every other turn or pt is within our defined space
                    if (ignoreUniverse || (algorithm[0] == '.' || (cx >= minX && cx <= maxX && cy >= minY && cy <= maxY)))
                    {
                        
                        strBin += lightPts.Contains(new Point(cx, cy)) ? '1' : '0';
                    }
                    else
                    {
                        strBin += "1";
                    }
                }

                // til: easy way to convert binary string to int!
                var num = Convert.ToInt32(strBin, 2); 
                if (algorithm[num] == '#')
                    points.Add(new Point(x, y));
            }
        }

        return points;
    }

    private static void PrintImage(HashSet<Point> lightPts)
    {
        var minY = lightPts.Min(pt => pt.Y) - 1;
        var maxY = lightPts.Max(pt => pt.Y) + 1;
        var minX = lightPts.Min(pt => pt.X) - 1;
        var maxX = lightPts.Max(pt => pt.X) + 1;

        for (var y = minY; y < maxY; y++)
        {
            for (var x = minX; x < maxX; x++)
            {
                var pt = new Point(x, y);
                var c = lightPts.Contains(pt) ? '#' : '.';
                Console.Write(c);
            }
            Console.WriteLine();
        }            
    }
}
