using Moq;
using NUnit.Framework;
using TanslateBusinessLayer;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace TranslateText.UnitTest
{
    public class TranslateTextUnitTests
    {
        private ServiceProvider serviceProvider;
        [SetUp]
        public void Setup()
        {
            var myConfiguration = new Dictionary<string, string>
                {
                    {"GoogleAPIToTranslate", "https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}"}
                };

            var configuration = new ConfigurationBuilder()

                .AddInMemoryCollection(myConfiguration)
                .Build();

            var serviceCollection = new ServiceCollection();
            
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<ITranslateText, TranslateTextService>();

            serviceProvider = serviceCollection.BuildServiceProvider();
            serviceProvider.GetService<ILogger<TranslateTextService>>();
        }

        [TestCase("Farnborough International Airshow, biennial global aerospace, defence and space trade event which showcases the latest commercial and military aircraft. Manufacturers such as Airbus and Boeing are expected to display their products and announce new orders * 2020 event was held virtually after the physical show was cancelled due to the coronavirus (COVID-19) pandemic")]
        [TestCase("Labour market statistics: integrated national release, including the latest data for employment, economic activity, economic inactivity, unemployment, claimant count, average earnings, productivity, unit wage costs, vacancies & labour disputes")]
        [TestCase("City of London Corporation's Financial and Professional Services dinner. Chancellor Rishi Sunak and Bank of England Governor Andrew Bailey make their annual Mansion House speeches at the event hosted by the Lord Mayor of the City of London Vincent Keaveny")]
        public void TranslateText(string testStr)
        {
            var bar = serviceProvider.GetService<ITranslateText>();
            var translateResult = bar.Translate(testStr, "fr").GetAwaiter().GetResult();
            Assert.IsInstanceOf<String>(translateResult);
        }
    }
}