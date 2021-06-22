
namespace CSharpLearning
{
    using System.IO;
    using System.Reflection;
    using System.Threading;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;

    public class MyLogger
    {
        static ILoggerFactory loggerFactory;
        static ILogger logger;

        public static void main()
        {
            string configPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "config.json");

            IConfigurationRoot loggingConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configPath, optional: true, reloadOnChange: true)
                .Build();

            loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .ClearProviders()
                    .AddConfiguration(loggingConfiguration.GetSection("Logging"))
                    .AddConsole();
            });

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
