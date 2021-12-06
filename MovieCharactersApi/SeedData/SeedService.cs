using MovieCharactersApi.Database;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.SeedData
{
    public class SeedService : ISeedService
    {
        private readonly MovieCharacterContext _context;

        public SeedService(MovieCharacterContext context)
        {
            _context = context;
        }
        public async Task SeedCharacters()
        {
            Character batman = new Character
            {
                FullName = "Bruce Wayne",
                Alias = "Batman",
                Gender = "male",
                Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Batman_%28black_background%29.jpg/800px-Batman_%28black_background%29.jpg",
                MovieList = new List<Movie>()
            };
            await _context.Characters.AddAsync(batman);
            await _context.SaveChangesAsync();
        }
    }
}
