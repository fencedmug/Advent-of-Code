using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021;

public class Day21
{
    // Answer: 739785 (test), 598416 (input)
    public static void SolveOne(string input)
    {
        var lines = input.IntoLines();
        var p1Pos = int.Parse(lines[0].IntoWords().Last());
        var p2Pos = int.Parse(lines[1].IntoWords().Last());

        var dieVal = 0;
        var numOfRolls = 0;
        var p1Score = 0;
        var p2Score = 0;

        while (true)
        {
            var roll = RollDie();
            p1Pos = p1Pos + roll;
            if (p1Pos > 10) p1Pos = (p1Pos - 1) % 10 + 1;
            //while (p1Pos > 10) p1Pos -= 10;
            p1Score += p1Pos;
            if (p1Score >= 1000) break;

            roll = RollDie();
            p2Pos = p2Pos + roll;
            while (p2Pos > 10) p2Pos -= 10;
            p2Score += p2Pos;
            if (p2Score >= 1000) break;
        }

        // this will help us to roll over the die value
        // and count number of rolls
        int RollDie()
        {
            var sum = 0;
            sum += (dieVal++ % 100) + 1;
            sum += (dieVal++ % 100) + 1;
            sum += (dieVal++ % 100) + 1;
            numOfRolls += 3;
            return sum;
        }

        Cout.WriteLine($"P1 {p1Score}, P2 {p2Score}, Ans {Math.Min(p1Score, p2Score) * numOfRolls}");
    }

    // Answer:
    // 444356092776315, 341960390180808 (test)
    // 27674034218179 (input)
    public static void SolveTwo(string input)
    {
        var lines = input.IntoLines();
        var p1Start = int.Parse(lines[0].IntoWords().Last());
        var p2Start = int.Parse(lines[1].IntoWords().Last());

        var scores = new Dictionary<Score, ulong>();
        // start with origin universe
        scores.Add(new Score(p1Start, 0, p2Start, 0), 1);

        // cache the possible values from die rolls
        var rolls = PVal();
        var p1Wins = 0UL;
        var p2Wins = 0UL;

        while (scores.Count > 0)
        {
            var (newP1Wins, newScoresAfterP1) = RollDie(scores, isP1turn: true);
            p1Wins += newP1Wins;

            var (newP2Wins, newScoresAfterP2) = RollDie(newScoresAfterP1, isP1turn: false);
            p2Wins += newP2Wins;

            scores = newScoresAfterP2;
        }

        (ulong wins, Dictionary<Score, ulong> newScores) RollDie(IReadOnlyDictionary<Score, ulong> scores, bool isP1turn)
        {
            var newScores = new Dictionary<Score, ulong>();
            var wins = 0UL;
            foreach (var (score, numOfUniverses) in scores)
            {
                var player = isP1turn ? score.GetP1() : score.GetP2();
                foreach (var (sumOfRoll, numOfRolls) in rolls)
                {
                    var totalUniverses = numOfRolls * numOfUniverses;

                    var pos = player.Pos + sumOfRoll;
                    while (pos > 10) pos -= 10;

                    var playerScore = player.Score + pos;
                    if (playerScore >= 21)
                    {
                        wins += totalUniverses;
                    }
                    else
                    {
                        var newScore = isP1turn ?
                            score with { P1Pos = pos, P1Score = playerScore } :
                            score with { P2Pos = pos, P2Score = playerScore };
                        newScores.Increment(newScore, totalUniverses);
                    }
                }
            }

            return (wins, newScores);
        }

        Cout.WriteLine($"P1 {p1Wins}, P2 {p2Wins}");
    }

    private static IImmutableList<(int, ulong)> PVal()
    {
        // instead of enumerating all die roll possibilities
        // this method will produce all possible scores from sum
        // of die rolls and the number of times the score appear
        // this was inspired by python solutions in reddit

        var values = from x in Enumerable.Range(1, 3)
                     from y in Enumerable.Range(1, 3)
                     from z in Enumerable.Range(1, 3)
                     let val = x + y + z
                     group val by val into valGrp
                     select (valGrp.Key, (ulong)valGrp.Count());

        return ImmutableList.ToImmutableList(values);
    }


    public readonly record struct Score(int P1Pos, int P1Score, int P2Pos, int P2Score)
    {
        public Player GetP1() => new Player(P1Pos, P1Score);
        public Player GetP2() => new Player(P2Pos, P2Score);
    }

    public readonly record struct Player(int Pos, int Score);
}