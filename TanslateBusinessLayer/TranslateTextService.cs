using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.Extensions.Configuration;

namespace TanslateBusinessLayer
{
    public class TranslateTextService : ITranslateText
    {
        private readonly ILogger _logger;
        private IConfiguration _configuration;

        public TranslateTextService(IConfiguration configuration, ILogger<TranslateTextService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Translate the given text in to selected language
        /// </summary>
        /// <param name="text">The text you want to be translate</param>
        /// <param name="expected">Represent the Target language</param>
        /// <returns></returns>
        public async Task<string> Translate(string text, string expected)
        {
            string url = String.Format(_configuration.GetValue<string>("GoogleAPIToTranslate"), "en", expected, Uri.EscapeUriString(text));
            HttpClient httpClient = new HttpClient();
            string result = httpClient.GetStringAsync(url).Result;

            // Get all json data
            var jsonData = JsonConvert.DeserializeObject<List<dynamic>>(result);

            // Extract just the first array element (This is the only data we are interested in)
            var translationItems = jsonData[0];

            // Translation Data
            string translation = "";

            // Loop through the collection extracting the translated objects
            foreach (object item in translationItems)
            {
                // Convert the item array to IEnumerable
                IEnumerable translationLineObject = item as IEnumerable;

                // Convert the IEnumerable translationLineObject to a IEnumerator
                IEnumerator translationLineString = translationLineObject.GetEnumerator();

                // Get first object in IEnumerator
                translationLineString.MoveNext();

                // Save its value (translated text)
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            // Remove first blank character
            if (translation.Length > 1) { translation = translation.Substring(1); };

            // Return translation
            return translation;
        }
    }
}
