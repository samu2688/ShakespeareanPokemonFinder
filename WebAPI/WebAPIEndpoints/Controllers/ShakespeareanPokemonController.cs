using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViewModels.Data;

namespace WebAPIEndpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShakespeareanPokemonController : ControllerBase
    {
        private readonly IBusinessService _businessService;       

        public ShakespeareanPokemonController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet(Name = "GetShakespeareanTranslation")]
        public ActionResult Get(string pokemonName)
        {
            ResultViewModel shakespeareanTranslation = _businessService.GetTranslation(pokemonName).Result;
            //string result = JsonSerializer.Serialize(shakespeareanTranslation);
            return new JsonResult(shakespeareanTranslation);
        }
    }
}