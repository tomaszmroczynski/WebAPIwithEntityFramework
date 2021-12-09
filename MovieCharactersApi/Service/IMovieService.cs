using MovieCharactersApi.DTOs;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Service
{
    public interface IMovieService
    {
        Task SeedData();
        Task<IEnumerable<CharacterDTO>> GetCharacters();
        Task<IEnumerable<MovieDTO>> GetMovies();
        Task<IEnumerable<FranchiseDTO>> GetFranchises();
        Task<CharacterDTO> GetCharacterById(int id);
        Task<MovieDTO> GetMovieById(int id);

        Task<FranchiseDTO> GetFranchiseById(int id);
        Task<Character> CreateCharacter(CreateCharacterDTO characterDTO);

        Task<Movie> CreateMovie(CreateMovieDTO CreateMovieDTO);
        Task<Franchise> CreateFranchise(CreateFranchiseDTO CreateFranchiseDTO);
        Task<string> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO);
        Task<string> UpdateMovie(UpdateMovieDTO updateMovieDTO);
        Task<string> UpdateFranchise(UpdateFranchiseDTO updateFranchiseDTO);
        Task<string> DeleteCharacter(int id);
        Task<string> DeleteMovie(int id);
        Task<string> DeleteFranchise(int id);
    }
}

