// This program will be called by another program to stress the CPU

namespace Task
{
    using System;
    using System.Threading;
    using CommandLine;

    internal class Options
    {
        [Option('s', "sleep", Required=true, HelpText="Sleep time in each loop in milliseconds.")]
        public int SleepTime { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<Options>(args).WithParsed(Run);
        }

        private static void Run(Options options)
        {
            int i = 1;
            while (true)  // stop by control + C
            {
                i += 1;
                i -= 1;
                i *= 10;
                i /= 10;
                Thread.Sleep(options.SleepTime);
            }
        }
    }
}
