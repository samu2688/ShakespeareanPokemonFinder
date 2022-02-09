using System.Collections.Generic;

namespace BusinessLogic.ViewModels
{
    //map only relevant infos
    internal class PokemonViewModel
    {
        internal int Id { get; set; }
        internal string Name { get; set; }

        internal List<FlavorTextEntries> FlavorTextEntries { get; set; }
    }

    internal class FlavorTextEntries
    {
        internal string FlavorText { get; set; }
    }

}