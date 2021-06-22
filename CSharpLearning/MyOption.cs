// https://github.com/commandlineparser/commandline

namespace CSharpLearning
{
    using System;
    using CommandLine;

    public class Options
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose message.")]
        public bool Verbose { get; set; }

        [Option('n', "numThreads", Default = 1, HelpText = "Number of threads. Should be greater or equal than number of files with queries supplied")]
        public int NumberThreads { get; set; }
    }

    public class TestOptionAttribute
    {
        public static void main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>                               // lambda function
                {
                    if (o.Verbose)
                    {
                        Console.WriteLine($"Verbose output enabled. Current Argument: -v {o.Verbose}");
                        Console.WriteLine("Quick Start Example!");
                    }
                    else
                    {
                        Console.WriteLine($"Current Arguments: -v {o.Verbose}");
                        Console.WriteLine("Quick Start Example!");
                    }
                    if (o.NumberThreads == 1)
                    {
                        Console.WriteLine("Single thread");
                    }
                    else
                    {
                        Console.WriteLine($"Multiple threads, thread number is {o.NumberThreads}");
                    }
                })
                .WithNotParsed<Options>(o =>
                {
                    Console.WriteLine("Oops! Failed to parse options.");
                });
        }
    }
}
