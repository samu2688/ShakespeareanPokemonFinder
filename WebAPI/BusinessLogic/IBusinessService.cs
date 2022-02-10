using System.Threading.Tasks;
using ViewModels.Data;

namespace BusinessLogic
{
    public interface IBusinessService
    {
        public Task<ResultViewModel> GetTranslation(string pokemonName);
    }
}