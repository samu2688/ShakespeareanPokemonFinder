using BusinessLogic.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ViewModels.Data;
using ViewModels.Enum;

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

        public async Task<ResultViewModel> GetTranslation(string pokemonName)
        {
            ResultViewModel resultViewModel = new ResultViewModel();
            resultViewModel.HasError = false;
            try
            {
                //to lower is necessary in order to make API working properly
                var pokemonDesc = await GetPokemonDesc(pokemonName?.ToLower());
                if (String.IsNullOrWhiteSpace(pokemonDesc))
                {
                    resultViewModel.Status = ResultEnum.RESULT.NO_DATA.ToString();
                    resultViewModel.HasError = true;
                }
                else
                {
                    var transalation = await GetShakespeareanTranslation(pokemonDesc);
                    if (String.IsNullOrWhiteSpace(transalation))
                    {
                        transalation = pokemonDesc;
                    }
                    resultViewModel.Translation = transalation;
                    resultViewModel.Status = ResultEnum.RESULT.OK.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error on retrieving data");
                resultViewModel.Status = ResultEnum.RESULT.ERROR.ToString();
                resultViewModel.HasError = true;
            }

            return resultViewModel;
        }

        private async Task<string> GetPokemonDesc(string pokemonName)
        {
            if (String.IsNullOrWhiteSpace(pokemonName))
            {
                return null;
            }

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
            if (String.IsNullOrWhiteSpace(text))
            {
                return null;
            }

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