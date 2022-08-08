using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using TanslateBusinessLayer;

namespace TranslateTextConsole
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //associated appsettings file to configuration builder 
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Path.GetDirectoryName(Environment.CommandLine))
                 .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();

            //setup our DI
            var serviceProvider = new ServiceCollection()
                 .AddLogging(c => c.AddConsole(opt => opt.LogToStandardErrorThreshold = LogLevel.Debug))
                .AddSingleton<ITranslateText, TranslateTextService>()
                .AddSingleton<IConfiguration>(configuration)
                .BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
           .CreateLogger<Program>();
            logger.LogDebug("Starting application");


            //inject translate service to do the translation
            var bar = serviceProvider.GetService<ITranslateText>();

            //string str = Console.ReadLine();

            string str = $@"Farnborough International Airshow, biennial global aerospace, defence and space trade event
                        which showcases the latest commercial and military aircraft. Manufacturers such as Airbus and Boeing 
                        are expected to display their products and announce new orders* 2020 event was held virtually after
                        the physical show was cancelled due to the coronavirus(COVID-19) pandemic";

           var translateResult = bar.Translate(str, "fr").GetAwaiter().GetResult();

            Console.WriteLine($"Converted result: {translateResult}");


        }
    }
}
