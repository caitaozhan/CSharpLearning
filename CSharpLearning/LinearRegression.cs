/*
 * https://docs.microsoft.com/en-us/dotnet/api/microsoft.ml.mklcomponentscatalog.ols?view=ml-dotnet#Microsoft_ML_MklComponentsCatalog_Ols_Microsoft_ML_RegressionCatalog_RegressionTrainers_System_String_System_String_System_String_
 */

namespace CSharpLearning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.ML;
    using Microsoft.ML.Data;

    public class LinearRegression
    {
        public static void main()
        {
            var mlContext = new MLContext(seed: 0);

            var dataPoints = GenerateRandomDataPoints(1000);

            var trainingData = mlContext.Data.LoadFromEnumerable(dataPoints);

            var pipeline = mlContext.Regression.Trainers.Ols(labelColumnName: nameof(DataPoint.Label), featureColumnName: nameof(DataPoint.Features));

            var model = pipeline.Fit(trainingData);

            var testData = mlContext.Data.LoadFromEnumerable(GenerateRandomDataPoints(5, seed: 123));

            var transformedTestData = model.Transform(testData);

            var predictions = mlContext.Data.CreateEnumerable<Prediction>(transformedTestData, reuseRowObject: false).ToList();

            foreach (var p in predictions)
            {
                Console.WriteLine($"Label: {p.Label:F3}, Prediction: {p.Score:F3}");
            }

            var metrics = mlContext.Regression.Evaluate(transformedTestData);
            PrintMetrics(metrics);
        }

        private static IEnumerable<DataPoint> GenerateRandomDataPoints(int count, int seed=0)
        {
            var random = new Random(seed);
            for (int i = 0; i < count; i++)
            {
                float label = (float)random.NextDouble();
                yield return new DataPoint
                {
                    Label = label,
                    Features = Enumerable.Repeat(label, 50).Select(x => x + (float)random.NextDouble()).ToArray()
                };
            }
        }

        private class DataPoint
        {
            public float Label { get; set; }
            [VectorType(50)]
            public float[] Features { get; set; }
        }

        private class Prediction
        {
            public float Label { get; set; }
            public float Score { get; set; }
        }

        private static void PrintMetrics(RegressionMetrics metrics)
        {
            Console.WriteLine("Mean Absolute Error: " + metrics.MeanAbsoluteError);
            Console.WriteLine("Mean Squared Error: " + metrics.MeanSquaredError);
            Console.WriteLine("Root Mean Squared Error: " + metrics.RootMeanSquaredError);
            Console.WriteLine("RSquared: " + metrics.RSquared);
        }
    }
}
