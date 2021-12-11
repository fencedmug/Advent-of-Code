using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day02
    {
        public static void SolveOne(string input)
        {
            int _depth = 0;
            int _horizon = 0;

            foreach (var line in input.IntoLines())
            {
                var vec = line.IntoWords();
                var direction = vec[0];
                var magnitude = int.Parse(vec[1]);

                switch (direction)
                {
                    case "forward":
                        _horizon += magnitude;
                        break;
                    case "up":
                        _depth = Math.Max(0, _depth - magnitude); //cannot be less than 0
                        break;
                    case "down":
                        _depth += magnitude;
                        break;
                }
            }

            Cout.WriteLine($"Depth {_depth}; Horizon {_horizon}; Multiplied {_depth * _horizon}");
        }

        public static void SolveTwo(string input)
        {
            int _depth = 0;
            int _horizon = 0;
            int _aim = 0;

            foreach (var line in input.IntoLines())
            {
                var vec = line.IntoWords();
                var direction = vec[0];
                var magnitude = int.Parse(vec[1]);

                switch (direction)
                {
                    case "forward":
                        _horizon += magnitude;
                        _depth += Math.Max(0, magnitude * _aim);
                        break;
                    case "up":
                        _aim -= magnitude;
                        break;
                    case "down":
                        _aim += magnitude;
                        break;
                }
            }

            Cout.WriteLine($"Depth {_depth}; Horizon {_horizon}; Aim {_aim}; Multiplied {_depth * _horizon}");
        }
    }
}
