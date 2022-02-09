using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IBusinessService
    {
        public Task<string> GetShakespeareanTranslation(string text);

        public Task<string> GetPokemonDesc(string pokemon);
    }
}