using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day14
    {
        // Answer (test): 1588
        // Answer (input): 2602
        // Embarassingly bad solution; left it here to remind myself to do better
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines(); ;
            var polymer = lines[0].Select(x => x).ToList();

            var templates = new Dictionary<string, char>();
            foreach (var line in lines.Skip(1))
            {
                var split = line.Split(" -> ");
                templates[split[0]] = split[1].First();
            }

            var steps = 10;
            for (int i = 0; i < steps; i++)
            {
                var insertPosition = 1;
                for (var j = 0; j < (polymer.Count - 1); j += 2)
                {
                    var temp = polymer.ToList();
                    var pair = $"{temp[j]}{temp[j + 1]}";

                    if (!templates.ContainsKey(pair))
                        continue;

                    polymer.Insert(insertPosition, templates[pair]);
                    insertPosition += 2;
                }
            }

            var count = new Dictionary<char, ulong>();
            foreach (var c in polymer)
            {
                if (!count.ContainsKey(c))
                    count[c] = 0;

                count[c]++;
            }

            var max = count.Values.Max();
            var min = count.Values.Min();
            Cout.WriteLine($"Most {max}, Least {min}, Diff = {max - min}");
        }


        // Answer (test): 2188189693529
        // Answer (input): 2942885922173
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var pairCount = new Dictionary<string, ulong>();
            var charCount = new Dictionary<char, ulong>();

            //parsed first line - the original polymer string
            char prev = lines[0][0];
            charCount[prev] = 1;
            for (int i = 1; i < lines[0].Length; i++)
            {
                var curr = lines[0][i];
                var pair = new string(new[] { prev, curr });

                pairCount.Increment(pair);
                charCount.Increment(curr);
                prev = curr;
            }

            // parsed templates: 2 letters produce a letter
            var lookUps = new Dictionary<string, char>();
            for (int i = 1; i < lines.Length; i++)
            {
                var split = lines[i].Split(" -> ");
                lookUps[split[0]] = split[1].First();
            }

            var steps = 40;
            for (int i = 0; i < steps; i++)
            {
                // each pair produces 2 pairs formed by pair[0] + new && new + pair[1]
                // the overall effect on polymer is that we produce 1 char when 1 pair is mutated into 2 pairs
                // temp will keep track of how many new pairs were produced
                var temp = new Dictionary<string, ulong>();
                foreach (var kvp in pairCount)
                {
                    var letter = lookUps[kvp.Key];
                    var pairA = new string(new[] { kvp.Key[0], letter });
                    var pairB = new string(new[] { letter, kvp.Key[1] });
                    
                    temp.Increment(pairA, kvp.Value);
                    temp.Increment(pairB, kvp.Value);
                    charCount.Increment(letter, kvp.Value);
                }

                pairCount = temp;
            }

            var max = charCount.Values.Max();
            var min = charCount.Values.Min();
            Cout.WriteLine($"Most {max}, Least {min}, Diff = {max - min}");
        }

        // attempt to use stackalloc; inconsequential changes
        // removing the use of Dictionary<..> would optimize the solution greatly
        public static void SolveTwo_v2(string input)
        {
            var lines = input.IntoLines();
            var pairCount = new Dictionary<string, ulong>();
            Span<ulong> charCount = stackalloc ulong[26];

            char prev = lines[0][0];
            charCount[prev - 'A'] = 1;
            for (int i = 1; i < lines[0].Length; i++)
            {
                var curr = lines[0][i];
                var pair = new string(new[] { prev, curr });

                pairCount.Increment(pair);
                charCount[curr - 'A']++;
                prev = curr;
            }

            var lookUps = new Dictionary<string, char>();
            for (int i = 1; i < lines.Length; i++)
            {
                var split = lines[i].Split(" -> ");
                lookUps[split[0]] = split[1].First();
            }

            var steps = 40;
            for (int i = 0; i < steps; i++)
            {
                var temp = new Dictionary<string, ulong>();
                foreach (var kvp in pairCount)
                {
                    var letter = lookUps[kvp.Key];
                    var pairA = new string(new[] { kvp.Key[0], letter });
                    var pairB = new string(new[] { letter, kvp.Key[1] });

                    temp.Increment(pairA, kvp.Value);
                    temp.Increment(pairB, kvp.Value);
                    charCount[letter - 'A'] += kvp.Value;
                }

                pairCount = temp;
            }

            ulong max = 0;
            ulong min = ulong.MaxValue;
            foreach (var val in charCount)
            {
                if (val == 0) continue;
                max = Math.Max(val, max);
                min = Math.Min(val, min);
            }

            Cout.WriteLine($"Most {max}, Least {min}, Diff = {max - min}");
        }
    }
}
