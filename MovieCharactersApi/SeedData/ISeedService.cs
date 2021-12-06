using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.SeedData
{
    public interface ISeedService
    {
        Task SeedCharacters();
    }
}
