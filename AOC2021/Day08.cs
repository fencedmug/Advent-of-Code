using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day08
    {
        public static void SolveOne(string input)
        {
            var sum = input
                .IntoLines()
                .Select(line => line.Split(" | ").Last())
                .SelectMany(line => line.Split(' '))
                .Count(text => text.Length is 2 or 4 or 3 or 7);

            Cout.WriteLine($"PartOne = {sum}");
        }

        /*  PartTwo:
         *      my original solution used array of HashSet for each word
         *      current PartTwo was inspired by z16/zed from C# Discord
         *      made used of String.Intersect instead of HashSet's Union/Except
         *      
         *  PartTwo_v2:
         *      inspired by a python solution https://gist.github.com/scmbradley/94046851e3d68948c1770b8881432471
         *      each number has a unique pattern based on occurrence of each character
         *      could use some feedback on how to make it cleaner/readable
         *      
         *      explanation:
         *          original zero was represented by abcefg, then the pattern would be 8, 6, 8, 4, 9, 7
         *          sort that pattern and we get 4, 6, 7, 8, 8, 9
         *          we count the number of occurrence for each char in output and derive a number pattern for each word
         *          then we try to match output's pattern with original pattern, we get the value the pattern matches   
         *      
         *  Other solutions observed:
         *      - figure out what's 'a', 'b', etc through intersecting strings
         *        then use a switch case to check what's the output's value          
         *      
         */

        public static void SolveTwo(string input)
        {
            int total = 0;

            foreach (var line in input.IntoLines().Select(line => line.Split(" | ")))
            {
                var signals = line[0].Split(" ");
                var sets = new string[10];

                sets[1] = signals.First(x => x.Length is 2);
                sets[4] = signals.First(x => x.Length is 4);
                sets[7] = signals.First(x => x.Length is 3);
                sets[8] = signals.First(x => x.Length is 7);

                var lengthOf5 = signals.Where(x => x.Length is 5);
                sets[2] = lengthOf5.First(x => x.Intersect(sets[4]).Count() is 2); // 2 intersects with 4 twice; 3 & 5 intersect thrice
                sets[3] = lengthOf5.First(x => x.Intersect(sets[7]).Count() is 3); // 3 intersects with 7 thrice; 2 & 5 intersect twice
                sets[5] = lengthOf5.First(x => x.Intersect(sets[2]).Count() is 3); // 5 intersects with 2 thrice; 2 intersect 5x; 3 intersect 4x

                var lengthOf6 = signals.Where(x => x.Length is 6);
                sets[9] = lengthOf6.First(x => x.Intersect(sets[4]).Count() is 4); // 9 intersects with 4 four times; 0 & 6 intersect thrice
                sets[0] = lengthOf6.First(x => x.Intersect(sets[5]).Count() is 4); // 0 intersects with 5 four times; 6 & 9 intersect 5x
                sets[6] = lengthOf6.First(x => x.Intersect(sets[7]).Count() is 2); // 6 intersects with 7 twice; 0 & 9 intersect thrice

                total += line[1]
                    .Split(" ")
                    .Select(x => Array.FindIndex(sets, set => set.Length == x.Length && set.Intersect(x).Count() == x.Length)) // alternatively use HashSet and check if same set
                    .Aggregate((acc, value) => acc * 10 + value);
            }

            Cout.WriteLine($"PartTwo = {total}");
        }

        public static void SolveTwo_InspiredFromPython(string input)
        {
            var originalPattern = "abcefg cf acdeg acdfg bdcf abdfg abdefg acf abcdefg abcdfg"; // 0 to 9 assuming all segments were in order
            var originalCounter = GetCounter(originalPattern);
            var translator = originalPattern
                .IntoWords()
                .Select((word, index) => new  
                    { 
                        Value = index, 
                        Sequence = word.Select(c => originalCounter[c]).OrderBy(num => num).ToArray() 
                    })
                .ToList();

            int total = 0;
            foreach (var line in input.IntoLines().Select(line => line.Split(" | ")))
            {
                var counter = GetCounter(line[0]);

                total += line[1]
                    .IntoWords()
                    .Select(word => word
                        .Select(c => counter[c])
                        .OrderBy(num => num)
                        .ToArray())
                    .Select(seq => translator
                        .First(pattern => pattern.Sequence.SequenceEqual(seq))      // will need to change translator from a List<..> to a Dictionary<..> for faster lookup
                        .Value)
                    .Aggregate((acc, value) => acc * 10 + value);
            }

            Dictionary<char, int> GetCounter(string signals)
            {
                var ctr = new Dictionary<char, int>();
                foreach (var c in signals)
                {
                    if (c == ' ') continue;
                    if (ctr.ContainsKey(c) is false) ctr[c] = 0;
                    ctr[c]++;
                }
                return ctr;
            }

            Cout.WriteLine($"Sum = {total}");
        }
    }
}
