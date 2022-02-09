using BusinessLogic.Utils;
using BusinessLogic.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<string> GetShakespeareanTranslation(string text)
        {
            var endpoint = _configuration["ShakespeareTranslatorEndpoint"];
            var response = await SimpleHttpClient.CallAPI(endpoint, text, _logger, _maxRetryAttempts, _pauseBetweenFailures);
            ShakespeareanViewModel model = JsonSerializer.Deserialize<ShakespeareanViewModel>(response);
            return model.Contents.Translated;
        }

        public async Task<string> GetPokemonDesc(string pokemon)
        {
            var endpoint = _configuration["PokemonEndpoint"];
            var response = await SimpleHttpClient.CallAPI(endpoint, pokemon, _logger, _maxRetryAttempts, _pauseBetweenFailures);
            PokemonViewModel model = JsonSerializer.Deserialize<PokemonViewModel>(response);
            return model.FlavorTextEntries.FirstOrDefault()?.FlavorText;
        }
        

    }
}