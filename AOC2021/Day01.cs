using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day01
    {
        public static void SolveOne(string input)
        {
            var lastValue = int.MinValue;
            var increments = -1; //because the first comparison always gives us +1

            foreach (var line in input.IntoLines())
            {
                var currValue = int.Parse(line);
                increments = currValue > lastValue ? increments + 1 : increments;
                lastValue = currValue;
            }

            Cout.WriteLine($"Increments: {increments}");
        }

        public static void SolveTwo(string input)
        {
            var nums = input.IntoLines().Select(x => int.Parse(x)).ToArray();
            var lastVal = int.MinValue;
            var increments = -1;

            for (var i = 0; i < nums.Length - 2; i++)
            {
                var currVal = nums[i] + nums[i + 1] + nums[i + 2];
                increments = currVal > lastVal ? increments + 1 : increments;
                lastVal = currVal;
            }

            Cout.WriteLine($"Increments: {increments}");
        }
    }
}
