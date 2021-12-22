using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021;

public class Day22
{
    private const string PartOneSample =
@"on x=-20..26,y=-36..17,z=-47..7
on x = -20..33, y = -21..23, z = -26..28
on x = -22..28, y = -29..23, z = -38..16
on x = -46..7, y = -6..46, z = -50..-1
on x = -49..1, y = -3..46, z = -24..28
on x = 2..47, y = -22..22, z = -23..27
on x = -27..23, y = -28..26, z = -21..29
on x = -39..5, y = -6..47, z = -3..44
on x = -30..21, y = -8..43, z = -13..34
on x = -22..26, y = -27..20, z = -29..19
off x = -48..-32, y = 26..41, z = -47..-37
on x = -12..35, y = 6..50, z = -50..-2
off x = -48..-32, y = -32..-16, z = -15..-5
on x = -18..26, y = -33..15, z = -7..46
off x = -40..-22, y = -38..-28, z = 23..41
on x = -16..35, y = -41..10, z = -47..6
off x = -32..-23, y = 11..30, z = -14..3
on x = -49..-5, y = -3..45, z = -29..18
off x = 18..30, y = -20..-8, z = -3..13
on x = -41..9, y = -7..43, z = -33..15
on x = -54112..-39298, y = -85059..-49293, z = -27449..7877
on x = 967..23432, y = 45373..81175, z = 27513..53682";

    // Answer = 590784
    public static void SolveOne_WithSample(string _)
    {
        Cout.WriteLine("Solving PartOne with test input");
        SolveOne(PartOneSample);
    }

    // Answer = 648023 (input)
    public static void SolveOne(string input)
    {
        var cubesOn = new HashSet<Cube>();
        foreach (var line in input.IntoLines())
        {
            var cuboid = GetCuboid(line);
            var x1 = Math.Max(cuboid.MinX, -50);
            var x2 = Math.Min(cuboid.MaxX, 50);
            var y1 = Math.Max(cuboid.MinY, -50);
            var y2 = Math.Min(cuboid.MaxY, 50);
            var z1 = Math.Max(cuboid.MinZ, -50);
            var z2 = Math.Min(cuboid.MaxZ, 50);

            foreach (var cube in GetRange(x1, x2, y1, y2, z1, z2))
            {
                if (cuboid.On)
                    cubesOn.Add(cube);
                else
                    cubesOn.Remove(cube);
            }
        }

        Cout.WriteLine($"Cubes on: {cubesOn.Count}");
    }

    // Answer: 2758514936282235 (test), 1285677377848549 (input)
    // unfortunately, couldn't figure out part two on my own
    // solution was derived from /u/r_so9 
    public static void SolveTwo(string input)
    {
        var cuboidsFromInput = input.IntoLines().Select(x => GetCuboid(x)).ToList();
        var cuboidsToSum = new List<Cuboid>();

        // the solution involves tracking all ON cuboids and ON/OFF intersections
        // the intersections help to remove double counted cubes
        // (e.g. "ON" "ON" -> create 1 intersection to "OFF")

        foreach (var cuboid in cuboidsFromInput)
        {
            var temp = new List<Cuboid>();
            if (cuboid.On) temp.Add(cuboid);

            // every step we will compare with every cuboid/intersection added
            // this helps to find intersections with all existing cuboids
            foreach (var cuboidAdded in cuboidsToSum)
            {
                if (DoNotIntersect(cuboid, cuboidAdded))
                    continue;

                // assuming we have an ON cuboid and OFF intersection in existing list
                // an incoming OFF cuboid will create an OFF intersection with the first ON cuboid
                // to counter double counted areas, the OFF cuboid will create another intersection
                // with the existing OFF intersection to cancel overlapped OFF sections
                // => OFF cuboid + OFF intersection will create an ON intersection (if overlap)
                
                var intersection = new Cuboid
                {
                    // to find overlap of two lines
                    // min1 ----------------- max1
                    //           min2 ---------------- max2
                    //                ^^^^^^^
                    // the highest min + lowest max
                    // apparently to find intersection of a cube
                    // we do that 3 times (all axes)

                    On = !cuboidAdded.On,
                    MinX = Math.Max(cuboid.MinX, cuboidAdded.MinX),
                    MaxX = Math.Min(cuboid.MaxX, cuboidAdded.MaxX),
                    MinY = Math.Max(cuboid.MinY, cuboidAdded.MinY),
                    MaxY = Math.Min(cuboid.MaxY, cuboidAdded.MaxY),
                    MinZ = Math.Max(cuboid.MinZ, cuboidAdded.MinZ),
                    MaxZ = Math.Min(cuboid.MaxZ, cuboidAdded.MaxZ),
                };
                temp.Add(intersection);
            }

            cuboidsToSum.AddRange(temp);
        }

        var total = cuboidsToSum
            .Select(c => 
                (c.MaxX - c.MinX + 1L) * 
                (c.MaxY - c.MinY + 1L) * 
                (c.MaxZ - c.MinZ + 1L) * 
                (c.On ? 1 : -1))
            .Aggregate((acc, val) => acc + val);

        Cout.WriteLine($"Total on: {total}");
    }

    private static Cuboid GetCuboid(string input)
    {
        var parts = input.Split(new[] { ',', '=', '.', 'x', 'y', 'z' }, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        var x1 = int.Parse(parts[1]);
        var x2 = int.Parse(parts[2]);
        var y1 = int.Parse(parts[3]);
        var y2 = int.Parse(parts[4]);
        var z1 = int.Parse(parts[5]);
        var z2 = int.Parse(parts[6]);
        return new Cuboid(parts[0] == "on", x1, x2, y1, y2, z1, z2);
    }

    private static IEnumerable<Cube> GetRange(int x1, int x2, int y1, int y2, int z1, int z2)
    {
        if (x2 < x1) yield break;
        if (y2 < y1) yield break;
        if (z2 < z1) yield break;

        var vals = from x in Enumerable.Range(x1, x2 - x1 + 1)
                   from y in Enumerable.Range(y1, y2 - y1 + 1)
                   from z in Enumerable.Range(z1, z2 - z1 + 1)
                   select new Cube(x, y, z);

        foreach (var val in vals)
            yield return val;
    }

    private static bool DoNotIntersect(Cuboid a, Cuboid b)
    {
        // lines do not intersect when min > max (top), or max < min (below)
        return
            a.MinX > b.MaxX || a.MaxX < b.MinX ||
            a.MinY > b.MaxY || a.MaxY < b.MinY ||
            a.MinZ > b.MaxZ || a.MaxZ < b.MinZ;
    }
}

public readonly record struct Cube(int X, int Y, int Z);
public readonly record struct Cuboid(bool On, int MinX, int MaxX, int MinY, int MaxY, int MinZ, int MaxZ);