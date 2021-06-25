
namespace CSharpLearning
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Collections;

    class MyProcess
    {
        public static void main()
        {
            int sleep = 0;
            int num   = 5;
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

            Console.WriteLine("Caitao");
            foreach (Process process in processes)
            {
                process.WaitForExit();
            }
            Console.WriteLine("Zhan");
        }

    }
}
