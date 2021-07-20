
namespace CSharpLearning
{
#pragma warning disable CA1416

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Linq;

    public class MyPerformanceCounter
    {
        // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.performancecounter?view=net-5.0

        private static PerformanceCounter avgCounter64Sample;
        private static PerformanceCounter avgCounter64SampleBase;

        public static void main()
        {
            ArrayList samplesList = new ArrayList();    // non-generic type of collection

            // If the category does not exist, create the category and exit.
            // Performance counters should not be created and immediately used.
            // There is a latency time to enable the counters, they should be created
            // prior to executing the application that uses the counters.
            // Execute this sample a second time to use the category.
            if (SetupCategory())
                return;
            CreateCounters();
            CollectSamples(samplesList);
            CalculateResults(samplesList);
        }

        private static bool SetupCategory()
        {
            if (!PerformanceCounterCategory.Exists("AverageCounter64SampleCategory"))
            {
                CounterCreationDataCollection counterDataCollection = new CounterCreationDataCollection();

                // Add the counter
                CounterCreationData averageCount64 = new CounterCreationData();
                averageCount64.CounterType = PerformanceCounterType.AverageCount64;
                averageCount64.CounterName = "AverageCounter64Sample";
                counterDataCollection.Add(averageCount64);

                // Add the base counter
                CounterCreationData averageCount64Base = new CounterCreationData();
                averageCount64Base.CounterType = PerformanceCounterType.AverageBase;
                averageCount64Base.CounterName = "AverageCounter64SampleBase";
                counterDataCollection.Add(averageCount64Base);

                // Create the category
                PerformanceCounterCategory.Create("AverageCounter64SampleCategory",             // NOTE need to run VS Studio as Administrator
                    "Demonstrates usage of the AverageCounter64 performance counter type.",
                    PerformanceCounterCategoryType.SingleInstance, counterDataCollection);

                return true;
            }
            else
            {
                Console.WriteLine("Category exists - AverageCounter64SampleCategory");
                return false;
            }
        }

        private static void CreateCounters()
        {
            // create the counters

            avgCounter64Sample = new PerformanceCounter("AverageCounter64SampleCategory", "AverageCounter64Sample", false);
            avgCounter64SampleBase = new PerformanceCounter("AverageCounter64SampleCategory", "AverageCounter64SampleBase", false);

            avgCounter64Sample.RawValue = 0;
            avgCounter64SampleBase.RawValue = 0;
        }

        private static void CollectSamples(ArrayList samplesList)
        {
            Random r = new Random(DateTime.Now.Millisecond);

            // Loop for the samples
            for (int j = 0; j < 100; j++)
            {
                int value = r.Next(10, 11);
                Console.Write(j + " = " + value);
                avgCounter64Sample.IncrementBy(value);                  // increase the raw value (numerator)
                avgCounter64SampleBase.Increment();                     // increase the base      (denomenator)
                if (j % 10 == 9)
                {
                    OutputSample(avgCounter64Sample.NextSample());      // this NextSample() returns the cumulative data.
                    samplesList.Add(avgCounter64Sample.NextSample());   // therefore need to compute the difference between two consecutive samples
                }
                else
                {
                    Console.WriteLine();
                }
                System.Threading.Thread.Sleep(50);
            }
        }

        private static void CalculateResults(ArrayList samplesList)
        {
            for (int i = 0; i < samplesList.Count - 1; i++)
            {
                // Output the sample
                OutputSample((CounterSample)samplesList[i]);
                OutputSample((CounterSample)samplesList[i + 1]);

                // use .NET to calculate the counter value
                Console.WriteLine(".Net comupted counter value = " + CounterSampleCalculator.ComputeCounterValue((CounterSample)samplesList[i], (CounterSample)samplesList[i + 1]));

                // Calculate the counter value manually
                Console.WriteLine("My computed counter value = " + MyComputeCounterValue((CounterSample)samplesList[i], (CounterSample)samplesList[i + 1]));
            }
        }

        private static Single MyComputeCounterValue(CounterSample s0, CounterSample s1)
        {
            Single numerator = (Single)s1.RawValue - (Single)s0.RawValue;
            Single denomenator = (Single)s1.BaseValue - (Single)s0.BaseValue;
            Single counterValue = numerator / denomenator;
            return counterValue;
        }

        // Output information about the counter sample
        private static void OutputSample(CounterSample s)
        {
            Console.WriteLine("\r\n++++++++++");
            Console.WriteLine("Sample values - \r\n");
            Console.WriteLine("   BaseValue        = " + s.BaseValue);
            Console.WriteLine("   CounterFrequency = " + s.CounterFrequency);
            Console.WriteLine("   CounterTimeStamp = " + s.CounterTimeStamp);
            Console.WriteLine("   CounterType      = " + s.CounterType);
            Console.WriteLine("   RawValue         = " + s.RawValue);
            Console.WriteLine("   SystemFrequency  = " + s.SystemFrequency);
            Console.WriteLine("   TimeStamp        = " + s.TimeStamp);
            Console.WriteLine("   TimeStamp100nSec = " + s.TimeStamp);
        }
    }

    public class CPUPerformanceCounter
    {
        /*
         * % Processor Time is the percentage of elapsed time that the processor spends to execute a non-Idle thread. 
         * It is calculated by measuring the percentage of time that the processor spends executing the idle thread 
         * and then subtracting that value from 100%. (Each processor has an idle thread that consumes cycles when 
         * no other threads are ready to run). This counter is the primary indicator of processor activity, and 
         * displays the average percentage of busy time observed during the sample interval. It should be noted that
         * the accounting calculation of whether the processor is idle is performed at an internal sampling interval of 
         * the system clock (10ms). On todays fast processors, % Processor Time can therefore underestimate the processor 
         * utilization as the processor may be spending a lot of time servicing threads between the system clock sampling
         * interval. Workload based timer applications are one example  of applications  which are more likely to be 
         * measured inaccurately as timers are signaled just after the sample is taken.
         */

        public static void main()
        {
            Queue<double> cpuQueue = new Queue<double>();
            PerformanceCounter theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            for (int i = 0; i < 10000; i++)
            {
                Thread.Sleep(1000);
                float cpu = theCPUCounter.NextValue();
                if (i >= 1)
                {
                    queue_add(cpuQueue, cpu);
                }
                Console.WriteLine("{0} - Current CPU: {1:0.000}, Average: {2:0.000}", i, cpu, queue_avg(cpuQueue));
            }
        }

        public static void queue_add(Queue<double> q, double cpu, int max_size=25)
        {
            if (q.Count == max_size)
            {
                q.Dequeue();
            }
            q.Enqueue(cpu);
        }

        public static double queue_avg(Queue<double> q)
        {
            double avg = 0;
            if (q.Count > 0)
            {
                avg = q.Average();
            }
            return avg;
        }
    }

    public class GetProcessorCount
    {
        public static int main()
        {
            int upBound = 512;
            int count = 1;
            PerformanceCounter theCPUCounter;
            for (int i = 0; i < upBound; i++)
            {
                try
                {
                    theCPUCounter = new PerformanceCounter("Processor", "% Processor Time", string.Format("{0}", i));
                    theCPUCounter.NextValue();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    count = i;
                    break;
                }
            }
            Console.WriteLine("Logical CPU number is {0}", count);
            return count;
        }
    }
}


