using MovieCharactersApi.DTOs;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Service
{
    /// <summary>
    /// An interface containing all MovieService methods for implemetation
    /// </summary>
    public interface IMovieService
    {
        Task SeedData();
        Task<IEnumerable<Character>> GetCharacters();
        Task<IEnumerable<Character>> GetCharactersInMovie(int movieId);        
        Task<IEnumerable<Movie>> GetMoviesInFranchise(int franchiseId);
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

        Task<Movie> UpdateCharactersInMovie(int movieId, int[] characterIds);

        Task<Franchise> UpdateMoviesInFranchise(int franchiseId, int[] movieId);
        Task<bool> DeleteCharacter(int id);
        Task<bool> DeleteMovie(int id);
        Task<bool> DeleteFranchise(int id);
    }
}

