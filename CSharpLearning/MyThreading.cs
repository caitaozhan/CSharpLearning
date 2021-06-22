// https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-5.0

namespace CSharpLearning
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics;

    class MyThreading
    {
        // Four ways to instantiate the Task object
        public static void TestTask()
        {
            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                Task.CurrentId, obj,
                Thread.CurrentThread.ManagedThreadId);
            };

            // Create a task but do not start it
            Task t1 = new Task(action, "alpha");

            // Construct a started task
            Task t2 = Task.Factory.StartNew(action, "beta");
            t2.Wait();

            // Launch t1
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})",
                               Thread.CurrentThread.ManagedThreadId);
            // Wait for the task to finish
            t1.Wait();

            // Construct a started task using Task.Run
            String taskData = "delta";
            Task t3 = Task.Run(() => { Console.WriteLine("Task={0}, obj={1}, Thread={2}", Task.CurrentId, taskData, Thread.CurrentThread.ManagedThreadId); });

            // Wait for the task to finish
            t3.Wait();

            // Construct an unstarted task
            Task t4 = new Task(action, "gamma");
            // Run the synchronously
            t4.RunSynchronously();
            t4.Wait();

        }

        // Creating and executing a task
        public static async Task TestAsync()
        {
            await Task.Run(() =>
            {
                // just loop
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                { }
                Console.WriteLine("Finished {0} loop iterations", ctr);
            });
        }


        static void Method(int i)
        {
            for (int j = 0; j < 5; j++)
            {
                Console.WriteLine("i={0}, j={1}, thread id={2}", i, j, Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);
            }
        }
        public static void TestParallel_1()
        {
            int N = 10;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.For(0, N, Method);   // using a named method

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }

        public static void TestParallel_2()
        {
            int N = 8;   // each thread is associated with a hyperthread in intel's CPU
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.For(0, N, i =>       // using a lambda expression
            {
                float num = 1;
                for (int j = 1; j < 1_000_000_000; j++)  // 1 billion
                {
                    num += j;
                    num -= j;
                }
            });

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
        }
    }
}
