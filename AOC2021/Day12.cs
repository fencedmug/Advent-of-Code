using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day12
    {
        private static Dictionary<string, HashSet<string>> CreateMap(string input)
        {
            var map = new Dictionary<string, HashSet<string>>();
            foreach (var line in input.IntoLines())
            {
                var pts = line.Split('-', StringSplitOptions.TrimEntries);
                if (!map.ContainsKey(pts[0]))
                    map[pts[0]] = new HashSet<string>();
                if (!map.ContainsKey(pts[1]))
                    map[pts[1]] = new HashSet<string>();

                map[pts[0]].Add(pts[1]);
                map[pts[1]].Add(pts[0]);
            }

            return map;
        }

        public static void SolveOne(string input)
        {
            var map = CreateMap(input);

            //let's not start at "start" but at it's neighbours instead
            //this way I do not have to check for "start" in the list
            var starting = map["start"].Select(node => new List<string> { node });
            var stack = new Stack<List<string>>(starting);

            var done = new List<List<string>>();
            while (stack.TryPop(out var path))
            {
                foreach (var nb in map[path.Last()])
                {
                    if (nb == "start")
                        continue;

                    if (nb == nb.ToLower() && path.Contains(nb))
                        continue;

                    if (nb == "end")
                    {
                        done.Add(path);
                        continue;
                    }    

                    var list = path.ToList();
                    list.Add(nb);
                    stack.Push(list);
                }
            }

            Cout.WriteLine($"No. of paths: {done.Count}");
        }

        public static void SolveTwo(string input)
        {
            var map = CreateMap(input);

            //let's not start at "start" but at it's neighbours instead
            //this way I do not have to check for "start" in the list
            var starting = map["start"].Select(node => new List<string> { node });
            var stack = new Stack<List<string>>(starting);
            
            var done = new List<List<string>>();
            while (stack.TryPop(out var path))
            {
                foreach (var nb in map[path.Last()])
                {
                    if (nb == "start")
                        continue;

                    if (nb == "end")
                    {
                        done.Add(path);
                        continue;
                    }

                    //faster way to check for lower case
                    if (nb[0] >= 'a' && nb[0] <= 'z')
                    {
                        var dict = new Dictionary<string, int>();
                        dict[nb] = 1;

                        foreach (var node in path)
                        {
                            if (!(node[0] >= 'a' && node[0] <= 'z'))
                                continue;

                            if (!dict.ContainsKey(node))
                                dict[node] = 0;

                            dict[node]++;
                        }

                        // TODO: to optimize counting because this is a very slow way to check for condition
                        if (dict.Values.Any(x => x > 2) || dict.Values.Count(x => x > 1) > 1)
                            continue;
                    }

                    var list = path.ToList();
                    list.Add(nb);
                    stack.Push(list);
                }
            }

            Cout.WriteLine($"No. of paths: {done.Count}");
        }

        // Attempt to optimize the counting :: not going well, removed half a sec maybe
        public static void SolveTwo_v2(string input)
        {
            var map = CreateMap(input);

            //let's not start at "start" but at it's neighbours instead
            //this way I do not have to check for "start" in the list
            var starting = map["start"].Select(node => Path.Create(node));
            var stack = new Stack<Path>(starting);

            var done = 0;
            while (stack.TryPop(out var path))
            {
                foreach (var nb in map[path.Nodes.Last()])
                {
                    if (nb.Length == 5) //"start"
                        continue;

                    if (nb.Length == 3) //"end"
                    {
                        done++;
                        continue;
                    }

                    var newPath = path.Clone();
                    if (newPath.AddCave(nb))
                    {
                        continue;
                    }

                    stack.Push(newPath);
                }
            }

            Cout.WriteLine($"No. of paths: {done}");
        }

        public struct Path
        {
            public List<string> Nodes;
            public Dictionary<string, int> SmallCaves;

            public bool AddCave(string cave)
            {
                Nodes.Add(cave);
                if (!(cave[0] >= 'a' && cave[0] <= 'z'))
                {
                    return false;
                }

                if (!SmallCaves.ContainsKey(cave))
                    SmallCaves[cave] = 0;

                SmallCaves[cave]++;

                if (SmallCaves[cave] == 3)
                    return true;

                if (SmallCaves.Where(kvp => kvp.Value > 1).Count() > 1)
                    return true;

                return false;
            }

            public static Path Create(string node)
            {
                var path = new Path
                {
                    Nodes = new List<string>(),
                    SmallCaves = new Dictionary<string, int>(),
                };

                path.AddCave(node);
                return path;
            }

            public Path Clone()
            {
                return new Path
                {
                    Nodes = Nodes.ToList(),
                    SmallCaves = SmallCaves.ToDictionary(kvp => kvp.Key, kvp => kvp.Value),
                };
            }
        }
    }
}
