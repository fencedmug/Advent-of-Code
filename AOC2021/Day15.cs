using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day15
    {
        public struct Node
        {
            public bool Visited;
            public int Weight;
            public Point Last;
        }

        //Answer: 40 (test); 429 (inputs)
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            var height = lines.Length;
            var width = lines.First().Length;

            Span<int> risks = stackalloc int[width * height];
            Span<Node> nodes = stackalloc Node[width * height];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    risks[x + y * height] = lines[y][x] - '0';
                    nodes[x + y * height].Weight = int.MaxValue;
                }
            }

            nodes[0].Weight = 0;
            var queue = new PriorityQueue<Point, int>();
            queue.Enqueue(new Point(0, 0), 0);
            while (queue.TryDequeue(out var currPt, out var currRisk))
            {
                var curr = currPt.X + currPt.Y * width;
                if (nodes[curr].Visited)
                    continue;

                nodes[curr].Visited = true;

                foreach (var nb in currPt.GetStraightNeighbours(width, height))
                {
                    // if new weight is lower than recorded weight it means we found a shorter path
                    // set last to this node; and set new weight in
                    var next = nb.X + nb.Y * width;
                    var newWeight = currRisk + risks[next];
                    if (newWeight < nodes[next].Weight)
                    {
                        nodes[next].Last = currPt;
                        nodes[next].Weight = newWeight;
                        queue.Enqueue(nb, newWeight);
                    }
                }
            }

            Cout.WriteLine($"Total risks: {nodes[width * height - 1].Weight}");
        }


        // Answer = 315 (test); 2844 (input)
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var tileHeight = lines.Length;
            var tileWidth = lines.First().Length;

            var maxWidth = tileWidth * 5;
            var maxHeight = tileHeight * 5;

            var risks = new List<int>(maxWidth * maxHeight); //switched to using a list (easier to add values)
            foreach (var line in lines)
                risks.AddRange(line.Select(x => x - '0'));

            var nodes = new Node[maxWidth * maxHeight]; //stackoverflow if i try stackalloc
            for (int i = 0; i < nodes.Length; i++)
                nodes[i].Weight = int.MaxValue;
            nodes[0].Weight = 0;

            // Dijkstra’s Shortest Path Algorithm requires us to visit the shortest path
            // a priority queue here will give us the node with least risk to fulfill the algorithm
            var queue = new PriorityQueue<Point, int>(); 
            queue.Enqueue(new Point(0, 0), 0);
            while (queue.TryDequeue(out var currPt, out var currWeight))
            {
                var curr = currPt.X + currPt.Y * maxWidth;
                if (nodes[curr].Visited)
                    continue;

                nodes[curr].Visited = true;

                foreach (var nbPt in currPt.GetStraightNeighbours(maxWidth, maxHeight))
                {
                    var nb = nbPt.X + nbPt.Y * maxWidth;
                    var newWeight = currWeight + GetRisk(risks, nbPt);
                    if (newWeight < nodes[nb].Weight)
                    {
                        nodes[nb].Last = currPt;
                        nodes[nb].Weight = newWeight;
                        queue.Enqueue(nbPt, newWeight); //doesn't affect results if i move it out of "if" scope
                    }
                }
            }

            //instead of creating an expanded map, use maths instead to calculate risk at expanded positions
            int GetRisk(List<int> riskList, Point pt)
            {
                var cx = pt.X % tileWidth;
                var cy = pt.Y % tileHeight;
                return ((riskList[cx + cy * tileWidth] + (pt.X / tileWidth) + (pt.Y / tileHeight) - 1) % 9) + 1;
            }

            Cout.WriteLine($"Total risks: {nodes.Last().Weight}");
        }
    }
}
