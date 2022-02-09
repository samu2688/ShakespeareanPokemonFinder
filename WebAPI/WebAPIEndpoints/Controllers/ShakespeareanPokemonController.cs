using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLogic;

namespace WebAPIEndpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShakespeareanPokemonController : ControllerBase
    {
        private readonly ILogger<ShakespeareanPokemonController> _logger;
        private readonly IBusinessService _businessService;       

        public ShakespeareanPokemonController(ILogger<ShakespeareanPokemonController> logger, IBusinessService businessService)
        {
            _logger = logger;
            _businessService = businessService;
        }

        [HttpGet(Name = "GetShakespeareanTranslation")]
        public string Get(string pokemon)
        {
            var pokemonDesc = _businessService.GetPokemonDesc(pokemon).Result;
            var shakespeareanTranslation = _businessService.GetShakespeareanTranslation(pokemonDesc).Result;
            return shakespeareanTranslation;
        }
    }
}