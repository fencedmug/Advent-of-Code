using AOC2021.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2021;

public class Day16
{
    public static void PartOneTests(string input)
    {
        var list = new List<string>
        {
            "8A004A801A8002F478",               // 16 (answers)
            "620080001611562C8802118E34",       // 12
            "C0015000016115A2E0802F182340",     // 23
            "A0016C880162017C3686B18A3D4780",   // 31
        };

        foreach (var item in list)
        {
            Console.WriteLine($"Runing PartOne test for {item}");
            SolveOne(item);
        }
    }

    public static void PartTwoTests(string input)
    {
        var list = new List<string>
        {
            "C200B40A82",                   // 3 (answers)
            "04005AC33890",                 // 54
            "880086C3E88112",               // 7
            "CE00C43D881120",               // 9
            "D8005AC2A8F0",                 // 1
            "F600BC2D8F",                   // 0
            "9C005AC2F8F0",                 // 0
            "9C0141080250320F1802104A08",   // 1
        };

        foreach (var item in list)
        {
            Console.WriteLine($"Runing PartTwo test for {item}");
            SolveTwo(item);
        }
    }

    public static void SolveOne(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Cout.WriteLine($"No input?");
            return;
        }

        var decoded = GetDecodedString(input).AsSpan();
        var msg = GetMessage(decoded, out _);

        int versionSum = 0;
        var queue = new Queue<Message>(new[] { msg });
        while (queue.TryDequeue(out var qmsg))
        {
            versionSum += qmsg.Version;
            foreach (var child in qmsg.SubPackets)
                queue.Enqueue(child);
        }

        Cout.WriteLine($"Sum of versions: {versionSum}");
    }

    public static void SolveTwo(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            Cout.WriteLine($"No input?");
            return;
        }

        var decoded = GetDecodedString(input).AsSpan();
        var msg = GetMessage(decoded, out _);

        Cout.WriteLine($"Outermost packet's value: {msg.Value}");
    }

    private static string GetDecodedString(string input)
    {
        var sb = new StringBuilder();
        foreach (var c in input)
        {
            sb.Append(c switch
            {
                '0' => "0000",
                '1' => "0001",
                '2' => "0010",
                '3' => "0011",
                '4' => "0100",
                '5' => "0101",
                '6' => "0110",
                '7' => "0111",
                '8' => "1000",
                '9' => "1001",
                'A' => "1010",
                'B' => "1011",
                'C' => "1100",
                'D' => "1101",
                'E' => "1110",
                'F' => "1111",
                _ => " ",
            });
        }

        return sb.ToString();
    }

    // todo: find a way to do message parsing without the use of recursion
    // SpanExtensions.Pick(..) was created due to being inspired by someone's solution in ruby (slices are awesome)
    // Span.Pick(..) is used here to minimize code clutter (hopefully)
    private static Message GetMessage(ReadOnlySpan<char> decoded, out ReadOnlySpan<char> retSpan)
    {
        var msg = new Message
        {
            Version = ReadAsInt(decoded.Pick(3)),
            TypeId = ReadAsInt(decoded.Pick(3)),
        };

        if (msg.TypeId == 4)
        {
            var literalStr = "";
            while (true)
            {
                var part = decoded.Pick(5);
                literalStr += part[1..5].ToString();
                if (part[0] == '0')
                    break;
            }

            msg.Value = ReadAsUlong(literalStr.AsSpan());
            retSpan = decoded;
            return msg;
        }

        msg.LengthId = decoded.Pick(1)[0] - '0';
        if (msg.LengthId == 0)
        {
            var length = ReadAsInt(decoded.Pick(15));
            var toDecode = decoded.Pick(length);
            while (toDecode.Length > 0)
            {
                msg.SubPackets.Add(GetMessage(toDecode, out var innerRetSpan));
                toDecode = innerRetSpan;
            }
        }
        else if (msg.LengthId == 1)
        {
            var numPackets = ReadAsInt(decoded.Pick(11));
            while (numPackets-- > 0)
            {
                msg.SubPackets.Add(GetMessage(decoded, out var innerRetSpan));
                decoded = innerRetSpan;
            }
        }

        // need this condition so that PartOne solutions don't throw exceptions
        msg.Value = msg.SubPackets.Count == 0 ? msg.Value : msg.TypeId switch
        {
            0 => msg.SubPackets.Select(x => x.Value).Aggregate((acc, val) => acc + val),
            1 => msg.SubPackets.Select(x => x.Value).Aggregate(1ul, (acc, val) => acc * val),
            2 => msg.SubPackets.Select(x => x.Value).Min(),
            3 => msg.SubPackets.Select(x => x.Value).Max(),
            5 => msg.SubPackets[0].Value > msg.SubPackets[1].Value ? 1ul : 0,
            6 => msg.SubPackets[0].Value < msg.SubPackets[1].Value ? 1ul : 0,
            7 => msg.SubPackets[0].Value == msg.SubPackets[1].Value ? 1ul : 0,
            _ => msg.Value,
        };

        retSpan = decoded;
        return msg;
    }

    private static int ReadAsInt(ReadOnlySpan<char> span)
    {
        return (int)ReadAsUlong(span);
    }

    private static ulong ReadAsUlong(ReadOnlySpan<char> span)
    {
        ulong val = 0;
        for (var i = 0; i < span.Length; i++)
            val = span[i] == '1' ? val + (1ul << (span.Length - 1 - i)) : val;

        return val;
    }
}

public class Message
{
    public int Version;
    public int TypeId;
    public int LengthId;
    public ulong Value;
    public List<Message> SubPackets = new List<Message>();
}
