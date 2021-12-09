using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day06
    {
        public const int MaxLifespan = 8;
        public const int SimulateDays = 256; //PartOne: 80 days; PartTwo: 256 days
        public const int NewLifespan = 6;

        public static void SolveTwo(string input)
        {
            var fishes = GetFishes(input);
            var buffer = new ulong[fishes.Length];

            for (int i = 0; i < SimulateDays; i++)
            {
                for (int j = 0; j < fishes.Length; j++)
                {
                    if (j == 0)
                    {
                        buffer[fishes.Length - 1] += fishes[j];
                        buffer[NewLifespan] += fishes[j];
                    }
                    else
                    {
                        buffer[j - 1] += fishes[j];
                    }
                }

                buffer.CopyTo(fishes, 0);
                buffer = new ulong[fishes.Length];
            }

            //Calculate number of fishes
            ulong totalFishes = 0;
            foreach (var fish in fishes)
            {
                totalFishes += fish;
            }

            Cout.WriteLine($"Total fishes: {totalFishes}");
        }

        private static ulong[] GetFishes(string line)
        {
            var fishes = new ulong[MaxLifespan + 1];

            foreach (var c in line.Split(",", StringSplitOptions.RemoveEmptyEntries))
            {
                var life = int.Parse(c);
                fishes[life]++;
            }

            return fishes;
        }
    }
}
