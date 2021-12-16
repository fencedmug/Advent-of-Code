using AOC2021.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day10
    {
        public static IReadOnlyDictionary<char, char> Pairs = new Dictionary<char, char>
        {
            ['('] = ')',
            ['['] = ']',
            ['{'] = '}',
            ['<'] = '>',
        };

        public static IReadOnlyDictionary<char, int> ErrorPts = new Dictionary<char, int>
        {
            [')'] = 3,
            [']'] = 57,
            ['}'] = 1197,
            ['>'] = 25137,
        };

        public static IReadOnlyDictionary<char, int> Points = new Dictionary<char, int>
        {
            ['('] = 1,
            ['['] = 2,
            ['{'] = 3,
            ['<'] = 4,
        };

        // My version of solution for part two
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            var score = 0;

            foreach (var line in lines)
            {
                var openChars = new Stack<char>();
                foreach (var c in line)
                {
                    if (Pairs.ContainsKey(c))
                    {
                        openChars.Push(c);
                        continue;
                    }

                    if (Pairs[openChars.Pop()] != c)
                    {
                        score += ErrorPts[c];
                        break;
                    }
                }
            }

            Cout.WriteLine($"Total error score: {score}");
        }

        // Wondering what the code would look like if I used switch expressions
        // instead of dictionaries and a for loop instead of foreach
        public static void SolveOne_v2(string input)
        {
            var lines = input.IntoLines();
            var score = 0;

            for (var h = 0; h < lines.Length; h++)
            {
                var openChars = new Stack<char>();
                for (var i = 0; i < lines[h].Length; i++)
                {
                    var c = lines[h][i];
                    if (c is '(' or '[' or '{' or '<')
                    {
                        openChars.Push(c);
                        continue;
                    }

                    if (c != openChars.Pop() switch { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => ' ' })
                    {
                        score += c switch { ')' => 3, ']' => 57, '}' => 1197, '>' => 25137, _ => 0 };
                        break;
                    }
                }
            }

            //code definitely looks more compact without needing to look at the dictionary's definitions
            Cout.WriteLine($"Total error score (v2): {score}");
        }

        // foreach + switch expressions :)
        public static void SolveOne_v3(string input)
        {
            var lines = input.IntoLines();
            var score = 0;

            foreach (var line in lines)
            {
                var openChars = new Stack<char>();
                foreach (var c in line)
                {
                    if (c is '(' or '[' or '{' or '<')
                    {
                        openChars.Push(c);
                        continue;
                    }

                    if (c != openChars.Pop() switch { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => ' ' })
                    {
                        score += c switch { ')' => 3, ']' => 57, '}' => 1197, '>' => 25137, _ => 0 };
                        break;
                    }
                }
            }

            Cout.WriteLine($"Total error score: {score}");
        }


        // My version of solution for part two
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var points = new List<long>();

            foreach (var line in lines)
            {
                var openChars = new Stack<char>();
                foreach (var c in line)
                {
                    if (Pairs.ContainsKey(c))
                    {
                        openChars.Push(c);
                        continue;
                    }

                    if (Pairs[openChars.Pop()] != c)
                    {
                        // originally I used stack.Clear then check if empty
                        // I think setting to null would be a bit faster
                        openChars = null; 
                        break;
                    }
                }
                
                if (openChars == null)
                    continue;

                var sum = 0L;
                while (openChars.TryPop(out var c))
                {
                    sum = sum * 5 + Points[c];
                }
                points.Add(sum);
                //points.Sort(); //I wonder if it will run faster if we keep it sorted often instead of at the end
            }

            points.Sort();
            Cout.WriteLine($"Middle score: {points[points.Count / 2]}");
        }

        // this version seems a little more readable and all information could fit into a screen
        // probably lucky that the switch conditions are short so it doesn't extend too much
        public static void SolveTwo_v2(string input)
        {
            var lines = input.IntoLines();
            var points = new List<long>();

            foreach (var line in lines)
            {
                var openChars = new Stack<char>();
                foreach (var c in line)
                {
                    if (c is '(' or '[' or '{' or '<')
                    {
                        openChars.Push(c);
                        continue;
                    }

                    if (c != openChars.Pop() switch { '(' => ')', '[' => ']', '{' => '}', '<' => '>', _ => ' ' })
                    {
                        openChars = null;
                        break;
                    }
                }

                if (openChars == null) 
                    continue;

                var sum = 0L;
                while (openChars.TryPop(out var c))
                {
                    sum = sum * 5 + c switch { '(' => 1L, '[' => 2L, '{' => 3L, '<' => 4L, _ => 0 };
                }

                points.Add(sum);
            }

            points.Sort();
            Cout.WriteLine($"Total error score: {points[points.Count / 2]}");
        }
    }
}
