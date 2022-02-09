using System.Threading.Tasks;

namespace BusinessLogic
{
    public interface IBusinessService
    {
        public Task<string> GetTranslation(string pokemonName);
    }
}