using Microsoft.Extensions.Logging;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLogic.Utils
{
    internal static class SimpleHttpClient
    {
        private static HttpClient client = new HttpClient();

        internal static async Task<string> CallAPI(
            string endpoint, 
            string param,
            ILogger logger,
            int maxRetryAttempts = 1,
            int pauseBetweenFailures = 10)
        {
            string returnString = null;
            try
            {
                logger.LogTrace($"Starting {endpoint} request (param {param})");

                client.DefaultRequestHeaders.Clear();
                //client.DefaultRequestHeaders.Add("Accept", Environment.GetEnvironmentVariable("ECBSdmxFormat"));

                HttpResponseMessage response = new HttpResponseMessage();

                //using a simple retry policy using Polly for resilience
                var pauseBetweenFailuresTimeSpan = TimeSpan.FromSeconds(pauseBetweenFailures);
                var retryPolicy = Policy
                    .Handle<HttpRequestException>()
                    .WaitAndRetryAsync(maxRetryAttempts, i => pauseBetweenFailuresTimeSpan);

                await retryPolicy.ExecuteAsync(async () =>
                {
                    response = await client.GetAsync(string.Format(endpoint, param));
                });

                if (response.IsSuccessStatusCode)
                {
                    returnString = await response.Content.ReadAsStringAsync();
                    logger.LogDebug("Response status: {0}; Response body: {0}", (int)response.StatusCode, returnString);
                }
                else
                {
                    logger.LogDebug("Response status: {0}", (int)response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error calling {endpoint} request (param {param})");
            }
                
            return returnString;
        }

    }
}