using BusinessLogic.Utils;
using BusinessLogic.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace BusinessLogic
{
    public class BusinessService : IBusinessService
    {
        private readonly ILogger<BusinessService> _logger;
        private readonly IConfiguration _configuration;
        private int _maxRetryAttempts;
        private int _pauseBetweenFailures;

        public BusinessService(ILogger<BusinessService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _maxRetryAttempts = Convert.ToInt32(_configuration["MaxRetryAttempts"]);
            _pauseBetweenFailures = Convert.ToInt32(_configuration["PauseBetweenFailures"]);
        }

        public async Task<string> GetTranslation(string pokemonName)
        {
            try
            {
                var pokemonDesc = await GetPokemonDesc(pokemonName);
                if (String.IsNullOrWhiteSpace(pokemonDesc))
                {
                    return null;
                }
                var transalation = await GetShakespeareanTranslation(pokemonDesc);
                if (String.IsNullOrWhiteSpace(transalation))
                {
                    transalation = pokemonDesc;
                }

                return transalation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on retrieving data");
                return null;
            }
        }

        private async Task<string> GetPokemonDesc(string pokemonName)
        {
            var endpoint = _configuration["PokemonEndpoint"];
            var response = await SimpleHttpClient.CallAPI(endpoint, pokemonName, _logger, _maxRetryAttempts, _pauseBetweenFailures);
            if (response == null)
            {
                return null;
            }

            PokemonViewModel model = JsonSerializer.Deserialize<PokemonViewModel>(response);
            //It is possible to receive multiple entries -> select only the english ones and, among them, take the first
            //This rule is not specified in the requirements but make sense -> to BE VERIFIED during testing
            return CleanText(model?.flavor_text_entries?.Where(x => x.language.name == "en").FirstOrDefault()?.flavor_text);
        }

        private async Task<string> GetShakespeareanTranslation(string text)
        {
            var endpoint = _configuration["ShakespeareTranslatorEndpoint"];
            var response = await SimpleHttpClient.CallAPI(endpoint, text, _logger, _maxRetryAttempts, _pauseBetweenFailures);
            if (response == null)
            {
                return null;
            }

            ShakespeareanViewModel model = JsonSerializer.Deserialize<ShakespeareanViewModel>(response);
            return CleanText(model?.contents?.translated);
        }

        private string CleanText(string text)
        {
            if (String.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var newString = Regex.Replace(text, @"[^0-9a-zA-Z]+", " ");
            return newString;
        }
        

    }
}