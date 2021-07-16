
namespace CSharpLearning
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;
    using Serilog.Extensions.Logging.File;

    public class MyLogger
    {
        static ILoggerFactory loggerFactory;
        static ILogger logger;

        public static void main()
        {
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");
            string logPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "mylog.txt");

            IConfigurationRoot loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: false, reloadOnChange: true)
                .Build();

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    .AddConfiguration(loggingConfiguration.GetSection("Logging"))
                    .AddFile(logPath)
                    .AddConsole();
            });

            Console.WriteLine(loggingConfiguration.GetSection("Logging").Value);
            int a = loggingConfiguration.GetValue<int>("caitao");

            logger = loggerFactory.CreateLogger<MyLogger>();

            for (int i = 0; i < 3; i++)
            {
                logger.LogInformation($"Log Information! {i}");
                Thread.Sleep(1);
                logger.LogError($"Log Error! {i}");
                Thread.Sleep(1);
                logger.LogWarning($"Log Warning {i}");
                Thread.Sleep(1);
            }
        }
    }
}
