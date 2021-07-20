namespace CSharpLearning
{
    using System.Threading;

    class program
    {

        public static void Main(string[] args)
        {
            // testing MyOption.cs
            //TestOptionAttribute.main(args);

            // testing Logging.cs
            // MyLogger.main();

            // testing MyThreading.cs
            //MyThreading.TestTask();
            //var t = MyThreading.TestAsync();
            //t.Wait();
            //Thread.Sleep(10);
            //MyThreading.TestParallel_1();
            //MyThreading.TestParallel_2();

            // testing MyEnumerateFiles.cs
            //MyEnumerateFiles.main();
            //MyEnumerateFiles.enumerable();
            //MyEnumerateFiles.enumerator();

            //MyYield.@return();

            //MyPerformanceCounter.main();
            //CPUPerformanceCounter.main();
            GetProcessorCount.main();

            //MyProcess.main();

            // LinearRegression.main();

        }
    }
}
