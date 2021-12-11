using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public class Day03
    {
        // Count number of bits at each position for each row of binary numbers
        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            int[] bitCounter = new int[lines[0].Length];

            foreach (var line in lines)
            {
                for (int i = 0; i < line.Length; i++)
                {
                    // each position will tell us if we encountered 1 or 0 more
                    bitCounter[i] += line[i] == '0' ? -1 : 1;
                }
            }

            int gamma = 0;
            int epsilon = 0;
            for (var i = 0; i < bitCounter.Length; i++)
            {
                int gammaBit = bitCounter[bitCounter.Length - 1 - i] > 0 ? 1 : 0; //start from the back
                int epsilonBit = bitCounter[bitCounter.Length - 1 - i] < 1 ? 1 : 0; //start from the back

                var power = 1 << i;
                gamma += gammaBit * power;
                epsilon += epsilonBit * power;
            }

            Cout.WriteLine($"Gamma {gamma}; Epsilon {epsilon}; Power {gamma * epsilon}");
        }

        // TODO: find time to optimize and make it more readable
        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var lengthOfBinary = lines[0].Length;

            var listForOnes = new List<string>();
            var listForZeroes = new List<string>();

            foreach (var line in lines)
            {
                if (line.First() == '1')
                {
                    listForOnes.Add(line);
                }
                else
                {
                    listForZeroes.Add(line);
                }
            }

            var oxygen = Read(listForZeroes.Count > listForOnes.Count ? listForZeroes : listForOnes, lengthOfBinary);
            var co2 = Read(listForOnes.Count < listForZeroes.Count ? listForOnes : listForZeroes, lengthOfBinary, false);
            Cout.WriteLine($"Oxygen: {oxygen}; CO2: {co2}; Life: {oxygen * co2}");
        }

        private static int Read(List<string> list, in int lengthOfBinary, bool useZeroesFirst = true)
        {
            var listOfZeroes = new List<string>();
            var listOfOnes = new List<string>();

            //start at one since zero pos has been handled
            for (int i = 1; i < lengthOfBinary; i++)
            {
                foreach (var line in list)
                {
                    if (line[i] == '1')
                    {
                        listOfOnes.Add(line);
                    }
                    else
                    {
                        listOfZeroes.Add(line);
                    }
                }

                list = useZeroesFirst ?
                    listOfZeroes.Count > listOfOnes.Count ? listOfZeroes : listOfOnes :
                    listOfOnes.Count < listOfZeroes.Count ? listOfOnes : listOfZeroes;

                //reset
                listOfZeroes = new List<string>();
                listOfOnes = new List<string>();

                if (list.Count == 1)
                {
                    return BinaryStringToInteger(list.First());
                }
            }

            return BinaryStringToInteger(list.First());
        }

        public static int BinaryStringToInteger(string value)
        {
            var sum = 0;

            for (var i = 0; i < value.Length; i++)
            {
                int bit = value[value.Length - 1 - i] == '1' ? 1 : 0; //start from the back
                var power = 1 << i;
                sum += bit * power;
            }

            return sum;
        }
    }
}
