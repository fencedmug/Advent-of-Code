using System;
using System.IO;
using System.Linq;
using System.Reflection;

Console.WriteLine("AOC 2021\n");

//find the latest day to run
var methods = Assembly.GetExecutingAssembly()
    .GetTypes()
    .Where(type => type.Name.Contains("Day"))
    .OrderBy(x => x.Name)
    .Last()
    .GetMethods(BindingFlags.Public | BindingFlags.Static);

var dayName = methods.First().DeclaringType.Name;
var tests = File.ReadAllText("inputs\\" + dayName + "_tests.txt");
var inputs = File.ReadAllText("inputs\\" + dayName + "_inputs.txt");

Console.WriteLine($"Solving {dayName}");
foreach (var method in methods)
{
    Console.WriteLine($"Running {method.Name}");
    Console.WriteLine("Using tests: ");
    method.Invoke(null, new[] { tests });

    Console.WriteLine("Using Inputs: ");
    method.Invoke(null, new[] { inputs });

    Console.WriteLine();
}