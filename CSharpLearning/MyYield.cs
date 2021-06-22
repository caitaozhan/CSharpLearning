
namespace CSharpLearning
{
    using System;
    using System.Collections.Generic;

    class MyYield
    {
        static List<int> numbersList = new List<int> { 1, 2, 3, 4, 5 };

        public static void @return()
        {
            foreach (int i in RunningTotal())
            {
                Console.WriteLine(i);
            }
        }

        public static IEnumerable<int> RunningTotal()
        {
            int runningTotal = 0;
            foreach (int i in numbersList)      // i = 5 will not execute, because of the yield break statement
            {
                runningTotal += i;
                yield return runningTotal;

                if (runningTotal >= 10)
                {
                    yield break;
                }
            }
        }
    }
}
