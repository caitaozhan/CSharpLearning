
namespace CSharpLearning
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Collections;
    using System.Threading;

    class MyProcess
    {
        public static void main()
        {
            int sleep = 1;
            int num   = 120;
            //string cur = Directory.GetCurrentDirectory();
            //Console.WriteLine(cur);

            string program = @"C:\Users\t-caitaozhan\source\repos\CSharpLearning\task\task\bin\Debug\netcoreapp3.1\task.exe";
            string args = String.Format("-s {0}", sleep);
            ArrayList processes = new ArrayList();
            try
            {
                for (int i = 0; i < num; i++)
                {
                    processes.Add(Process.Start(program, args));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            //Console.WriteLine("Caitao");
            //foreach (Process process in processes)
            //{
            //    process.WaitForExit();
            //}
            //Console.WriteLine("Zhan");
        
            for (int i = 15; i > 0 ; i--)
            {
                Console.WriteLine("Killing Countdown {0}", i);
                Thread.Sleep(1000);
            }
            foreach (Process process in processes)
            {
                process.Kill();
            }
            Console.WriteLine("All killed!");
        }
    }
}
