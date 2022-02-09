using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BusinessLogic;
using System.Threading.Tasks;

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
        public async Task<string> Get(string pokemonName)
        {
            var shakespeareanTranslation = await _businessService.GetTranslation(pokemonName);
            return shakespeareanTranslation;
        }
    }
}