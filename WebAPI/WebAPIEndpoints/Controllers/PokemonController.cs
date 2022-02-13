using BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ViewModels.Data;

namespace WebAPIEndpoints.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IBusinessService _businessService;       

        public PokemonController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet]
        public ActionResult Get(string pokemon)
        {
            ResultViewModel shakespeareanTranslation = _businessService.GetTranslation(pokemon).Result;
            //string result = JsonSerializer.Serialize(shakespeareanTranslation);
            return new JsonResult(shakespeareanTranslation);
        }
    }
}