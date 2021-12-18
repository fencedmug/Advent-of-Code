using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021;

public class Day17
{
    // Answer y = 9, max = 45 (tests); y = 95, max = 4560
    public static void SolveOne(string input)
    {
        var range = input.IntoWords().Last().Split('=').Last().Split("..");
        var lowestPt = Math.Min(int.Parse(range[0]), int.Parse(range[1]));

        // highestPoint - distanceTravelled = lowestPt
        // highestPoint - n(n+1)/2 = lowestPt

        // y = -delta - 1;
        // delta in this case is 1 step it is required to reach the lowest point
        // since we always start from 0,0, we will always hit 0,0 when coming down
        // hence the delta is 0 to lowest point
        var y = -lowestPt - 1;
        var highestPoint = y * (y + 1) / 2;
        Cout.WriteLine($"Velocity: {y}; Highest point: {highestPoint}");

        // i'm not sure if this answer is mathematically correct
        // it's just based on intuition
    }

    // couldn't figure out how to solve this mathematically :(
    // went for a solution that simulates all possible points that can hit target
    public static void SolveTwo(string input)
    {
        var parts = input.IntoWords();
        var x_coords = parts[2].Replace("x=", "").Replace(",", "").Split("..");
        var y_coords = parts[3].Split('=').Last().Split("..");
        var boundary = new Boundary
        {
            Left = int.Parse(x_coords[0]),
            Right = int.Parse(x_coords[1]),
            Top = int.Parse(y_coords[1]),
            Btm = int.Parse(y_coords[0]),
        };

        // using linq methods
        var launches = Enumerable
            .Range(1, boundary.Right + 1)
            .Select(velX =>
                Enumerable
                .Range(boundary.Btm, Math.Abs(boundary.Btm * 2))
                .Select(velY => SimulateLaunch(velX, velY, boundary))
                .SelectMany(li => li))
            .SelectMany(li => li)
            .ToList();

        Cout.WriteLine($"Velocities: {launches.Count}");

        // using linq queries
        var launches2 =
            (from x in Enumerable.Range(1, boundary.Right + 1)
             from y in Enumerable.Range(boundary.Btm, Math.Abs(boundary.Btm * 2))
             let info = SimulateLaunch(x, y, boundary)
             select info into infoList
             from info in infoList
             select info) //looks ugly to use another bracket just so i can use ToList()
            .ToList();

        Cout.WriteLine($"Velocities: {launches2.Count()}");

        // wrote two versions to compare which I would prefer more
        // linq queries probably look better for non select many operations
    }

    private static IEnumerable<LaunchInfo> SimulateLaunch(int initX, int initY, Boundary bd)
    {
        var posX = 0;
        var posY = 0;
        var step = 0;

        while (true)
        {
            // X velocity goes to zero and stays there
            posX += (initX - step) >= 0 ? initX - step : 0;
            // Y velocity constantly decreases (to -ve infinite)
            posY += initY - step;
            step++;

            //check if we are in target boundary (both pos must be inside)
            if (posX >= bd.Left && posX <= bd.Right && posY <= bd.Top && posY >= bd.Btm)
            {
                yield return new LaunchInfo { X = initX, Y = initY, Steps = step };

                // it is possible that the same velocity can hit the boundary more than once
                // but we are only looking for unique velocities so we break after 1 result
                break;
            }

            // overshoot => will never be in boundary again hence we break
            // only Y can overshoot because X goes to zero (horizontally static)
            if (posY < bd.Btm)
                break;
        }
    }

    public struct Boundary
    {
        public int Left, Right;
        public int Btm, Top;
    }

    public struct LaunchInfo
    {
        public int X;
        public int Y;
        public int Steps;
        public override string ToString() => $"({X}x {Y}y {Steps}s)";
    }
}
