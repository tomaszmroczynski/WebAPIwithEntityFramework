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
        Task<IEnumerable<Character>> GetCharacters();
        Task<IEnumerable<Movie>> GetMovies();
        Task<IEnumerable<Franchise>> GetFranchises();
        Task<Character> GetCharacterById(int id);
        Task<Movie> GetMovieById(int id);

        Task<Franchise> GetFranchiseById(int id);
        Task<Character> CreateCharacter(Character character);

        Task<Movie> CreateMovie(Movie movie);
        Task<Franchise> CreateFranchise(Franchise franchise);
        Task<Character> UpdateCharacter(Character character);
        Task<Movie> UpdateMovie(Movie movie);
        Task<Franchise> UpdateFranchise(Franchise franchise);

        Task<string> UpdateCharactersInMovie(int movieId, int[] charactersId);

        Task<string> UpdateMovieInFranchise(int franchiseId, int[] moviesId);
        Task<bool> DeleteCharacter(int id);
        Task<bool> DeleteMovie(int id);
        Task<bool> DeleteFranchise(int id);
    }
}

