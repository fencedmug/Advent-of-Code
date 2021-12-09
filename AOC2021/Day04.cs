using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021
{
    public struct Number
    {
        public int Value;
        public int TurnMarked;
    }

    public class Day04
    {
        /*  Original solution was object oriented: each board was a class (which was pretty messy)
         *  Each number drawn would be added to board and board would check if any won
         *  If the board won, it would raise a call back to announce the num/turn
         *  Each number was a tuple of Value(int) + Marked(bool) and the sum would get only numbers
         *  that were unmarked
         *  
         *  
         *  This solution deals with deriving the number of turns to win as each board is parsed
         *  It is assumed that every row/col is winnable and it's about figuring the turns it takes to win
         *  We calculate each board's winning turn and compare with "global" result and only
         *  saving the board that has the most/least turns to win 
         *  
         */

        public static void SolveOne(string input)
        {
            var lines = input.IntoLines();
            var draws = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var drawsMap = GetDrawsMap(draws);

            var leastTurn = int.MaxValue;
            Number[][] winningBoard = null;

            foreach (var board in GetBoards(lines, 1, drawsMap))
            {
                var minimum = GetMinTurnToWin(board);
                if (minimum < leastTurn)
                {
                    leastTurn = minimum;
                    winningBoard = board;
                }
            }

            var numCalled = drawsMap.First(kvp => kvp.Value == leastTurn).Key;
            var sumOfUnmarked = winningBoard.SelectMany(row => row.Select(num => num)).Where(num => num.TurnMarked > leastTurn).Sum(num => num.Value);

            Cout.WriteLine($"Least turn: {leastTurn}; Num called: {numCalled}; Sum unmarked: {sumOfUnmarked}");
            Cout.WriteLine($"Final score: {numCalled * sumOfUnmarked}");
        }

        private static Dictionary<int, int> GetDrawsMap(string[] draws)
        {
            var drawsMap = new Dictionary<int, int>();
            for (int i = 0; i < draws.Length; i++)
            {
                var num = int.Parse(draws[i]);
                drawsMap[num] = i + 1; //array starts at 0, turn starts at 1
            }

            return drawsMap;
        }

        private static IEnumerable<Number[][]> GetBoards(string[] lines, int startIndex, Dictionary<int, int> valMap)
        {
            for (int i = startIndex; i < lines.Length; i+=5)
            {
                var board = new Number[5][];

                for (int j = 0; j < board.Length; j++)
                {
                    board[j] = lines[i + j].IntoWords()
                        .Select(x => int.Parse(x))
                        .Select(val => new Number { Value = val, TurnMarked = valMap[val] })
                        .ToArray();
                }
                
                yield return board;
            }
        }

        private static int GetMinTurnToWin(Number[][] board)
        {
            var minTurnToWinAnyRow = board.Select(row => row.Max(x => x.TurnMarked)).Min();
            var minTurnToWinAnyCol = Enumerable.Range(0, 5).Select(colIndex => board.Select(row => row[colIndex]).Max(x => x.TurnMarked)).Min();

            var minimum = Math.Min(minTurnToWinAnyCol, minTurnToWinAnyRow);
            return minimum;
        }

        public static void SolveTwo(string input)
        {
            var lines = input.IntoLines();
            var draws = lines[0].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            var drawsMap = GetDrawsMap(draws);

            var mostTurnToWin = int.MinValue;
            Number[][] lastboard = null;

            foreach (var board in GetBoards(lines, 1, drawsMap))
            {
                var minTurn = GetMinTurnToWin(board);
                if (minTurn > mostTurnToWin)
                {
                    mostTurnToWin = minTurn;
                    lastboard = board;
                }
            }

            var numCalled = drawsMap.First(kvp => kvp.Value == mostTurnToWin).Key;
            var sumOfUnmarked = lastboard.SelectMany(row => row.Select(num => num)).Where(num => num.TurnMarked > mostTurnToWin).Sum(num => num.Value);

            Cout.WriteLine($"Most turn: {mostTurnToWin}; Num called: {numCalled}; Sum unmarked: {sumOfUnmarked}");
            Cout.WriteLine($"Final score: {numCalled * sumOfUnmarked}");
        }
    }
}
