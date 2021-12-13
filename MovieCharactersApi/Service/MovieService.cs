using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieCharactersApi.Database;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Service
{
    /// <summary>
    /// A service containing all CRUD methods for character , franchise, movie database models
    /// </summary>
    public class MovieService : IMovieService
    {
        
        private readonly MovieCharacterContext _context;


        public MovieService(MovieCharacterContext context)
        {
            _context = context;

        }
        /// <summary>
        /// A service method that creates character in database
        /// </summary>
        /// <param name="character">Character instance</param>
        /// <returns>Created character</returns>
        public async Task<Character> CreateCharacter(Character character)
        {
            try
            {               
                await _context.AddAsync(character);
                await _context.SaveChangesAsync();
                return character;
            }
            catch (Exception)
            {
                return null;
            }          
        }
        /// <summary>
        /// A service method that creates movie in database
        /// </summary>
        /// <param name="movie">Movie instance</param>
        /// <returns>Created movie</returns>
        public async Task<Movie> CreateMovie(Movie movie)
        {
            try
            {
                await _context.AddAsync(movie);
                await _context.SaveChangesAsync();
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// A service method that creates franchise in database
        /// </summary>
        /// <param name="franchise">Franchise instance</param>
        /// <returns>Created franchise</returns>
        public async Task<Franchise> CreateFranchise(Franchise franchise)
        {
            try
            {  
                await _context.AddAsync(franchise);
                await _context.SaveChangesAsync();
                return franchise;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// A service method that deletes character from database
        /// </summary>
        /// <param name="id">An id of character to be deleted</param>
        /// <returns>Boolean result of action</returns>
        public async Task<bool> DeleteCharacter(int id)
        {
            var character = await _context.Characters.SingleOrDefaultAsync(c => c.Id == id);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;           
        }
        /// <summary>
        /// A service method that deletes movie from database
        /// </summary>
        /// <param name="id">An id of movie to be deleted</param>
        /// <returns>Boolean result of action</returns>
        public async Task<bool> DeleteMovie(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(c => c.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// A service method that deletes franchise from database
        /// </summary>
        /// <param name="id">An id of franchise to be deleted</param>
        /// <returns>Boolean result of action</returns>
        public async Task<bool> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.SingleOrDefaultAsync(c => c.Id == id);
            if (franchise != null)
            {
                _context.Franchises.Remove(franchise);
                await _context.SaveChangesAsync();
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// A service method that show selected character from database
        /// </summary>
        /// <param name="id">Id of selected character</param>
        /// <returns>Selected character</returns>
        public async Task<Character> GetCharacterById(int id)
        {
            return await _context.Characters.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);          
        }
        /// <summary>
        /// A service method that show selected movie from database
        /// </summary>
        /// <param name="id">Id of selected movie</param>
        /// <returns>Selected movie</returns>
        public async Task<Movie> GetMovieById(int id)
        {
            return await _context.Movies.Include(c => c.CharacterList).FirstOrDefaultAsync(c => c.Id == id);           
        }
        /// <summary>
        /// A service method that show selected franchise from database
        /// </summary>
        /// <param name="id">Id of selected franchise</param>
        /// <returns>Selected franchise</returns>
        public async Task<Franchise> GetFranchiseById(int id)
        {
            return await _context.Franchises.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);
         
        }
        /// <summary>
        /// A service method that show all characters from database
        /// </summary>
        /// <returns>All characters/returns>
        public async Task<IEnumerable<Character>> GetCharacters()
        {
            return await _context.Characters.Include(c => c.MovieList).ToListAsync();               
        }
        /// <summary>
        /// A service method that show all characters from selected movie
        /// </summary>
        /// <param name="id">Id of selected movie</param>
        /// <returns>All characters in selected movie/returns>
        public async Task<IEnumerable<Character>> GetCharactersInMovie(int movieId)
        {
            var moviesWithCharacters = await _context.Movies.Include(c => c.CharacterList).ToListAsync();
            var foundMovie = moviesWithCharacters.FirstOrDefault(x => x.Id == movieId);           
            return foundMovie.CharacterList;
        }
        /// <summary>
        /// A service method that show all movies from selected franchise
        /// </summary>
        /// <param name="id">Id of selected franchise</param>
        /// <returns>All characters in selected franchise/returns>
        public async Task<IEnumerable<Movie>> GetMoviesInFranchise(int franchiseId)
        {          
            var franchiseWithMovies = await _context.Franchises.Include(c => c.MovieList).ToListAsync();
            var foundfranchise = franchiseWithMovies.FirstOrDefault(x => x.Id == franchiseId);
            return foundfranchise.MovieList;
        }

        /// <summary>
        /// A service method that show all movies in database
        /// </summary>
        /// <returns>All movies</returns>
        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _context.Movies.Include(c => c.CharacterList).ToListAsync();
        }
        /// <summary>
        /// A service method that show all franchises in database
        /// </summary>
        /// <returns>All franchises</returns>
        public async Task<IEnumerable<Franchise>> GetFranchises()
        {
            return await _context.Franchises.Include(c => c.MovieList).ToListAsync();          
        }
        /// <summary>
        /// A service method that seeds data for first time
        /// </summary>
        public async Task SeedData()
        {           
            var characters = CreateStartCharacters();
            var movies = CreateMovies();
            var franchises = CreateFranchises();           
            await _context.Characters.AddRangeAsync(characters);
            await _context.Movies.AddRangeAsync(movies);
            await _context.Franchises.AddRangeAsync(franchises);
            await _context.SaveChangesAsync();
        }
        /// <summary>
        /// A service method that updates character in database
        /// </summary>
        /// <param name="character">New character parameters</param>
        /// <returns>Updated character</returns>
        public async Task<Character> UpdateCharacter(Character character)
        {
            var characterFound = await _context.Characters.SingleOrDefaultAsync(c => c.Id == character.Id);

            if (characterFound != null)
            {
                characterFound.FullName = character.FullName;
                characterFound.Alias = character.Alias;
                characterFound.Gender = character.Gender;
                characterFound.Picture = character.Picture;
                characterFound.MovieList = character.MovieList;
                await _context.SaveChangesAsync();
                return characterFound;
            }
            else
                return null;
        }
        /// <summary>
        /// A service method that updates movie in database
        /// </summary>
        /// <param name="movie">New movie parameters</param>
        /// <returns>Updated movie</returns>
        public async Task<Movie> UpdateMovie(Movie movie)
        {
            var movieFound = await _context.Movies.SingleOrDefaultAsync(c => c.Id == movie.Id);
            if (movieFound != null)
            {
                movieFound.MovieTitle = movie.MovieTitle;
                movieFound.Genre = movie.Genre;
                movieFound.ReleaseYear = movie.ReleaseYear;
                movieFound.Director = movie.Director;
                movieFound.Picture = movie.Picture;
                movieFound.Trailer = movie.Trailer;
                await _context.SaveChangesAsync();
                return movieFound;
            }
            else
                return null;
        }
        /// <summary>
        /// A service method that updates characters in selected movie
        /// </summary>
        /// <param name="movieId">Selected movie id</param>
        /// <param name="characterIds">Array of  character ids for updating</param>
        /// <returns>Updated movie</returns>
        public async Task<Movie> UpdateCharactersInMovie(int movieId, int[]characterIds)
        {    
            var movieFound = await _context.Movies.Include(m => m.CharacterList).SingleOrDefaultAsync(c => c.Id == movieId);
            if (movieFound != null)
            {
                movieFound.CharacterList.Clear();
                foreach (var item in characterIds)
                {
                    var character = await _context.Characters.FirstOrDefaultAsync(x => x.Id == item);
                    if (character != null)
                    {
                        movieFound.CharacterList.Add(character);
                    }
                    else
                        continue;
                }              
                await _context.SaveChangesAsync();
                return movieFound;
            }
            else
                return null;
        }
        /// <summary>
        /// A service method that updates movies in selected franchise
        /// </summary>
        /// <param name="franchiseId">Selected franchise id</param>
        /// <param name="movieIds">An array of movie Id for updating</param>
        /// <returns>Updated franchise</returns>
        public async Task<Franchise> UpdateMoviesInFranchise(int franchiseId, int[] movieIds)
        {
            var franchiseFound = await _context.Franchises.Include(m => m.MovieList).SingleOrDefaultAsync(c => c.Id == franchiseId);
            if (franchiseFound != null)
            {
                franchiseFound.MovieList.Clear();
                foreach (var item in movieIds)
                {
                    var movie = await _context.Movies.FirstOrDefaultAsync(x => x.Id == item);
                    if (movie != null)
                    {
                        franchiseFound.MovieList.Add(movie);
                    }
                    else
                        continue;
                }
                await _context.SaveChangesAsync();
                return franchiseFound;
            }
            else
                return null;
        }
        /// <summary>
        /// A service methods that updates franchise
        /// </summary>
        /// <param name="franchise">Selected franchise</param>
        /// <returns>Updated franchise</returns>
        public async Task<Franchise> UpdateFranchise(Franchise franchise)
        {
            var franchiseFound = await _context.Franchises.SingleOrDefaultAsync(c => c.Id == franchise.Id);
            if (franchiseFound != null)
            {
                franchiseFound.Name = franchise.Name;
                franchiseFound.Description = franchise.Description;

                await _context.SaveChangesAsync();
                return franchiseFound;
            }
            else
                return null;
        }
        /// <summary>
        /// A private method for first time characters seed to database
        /// </summary>
        private ICollection<Character> CreateStartCharacters() 
        {
            return new List<Character>
            {
                new Character
                {
                    FullName = "Bruce Wayne",
                    Alias = "Batman",
                    Gender = "male",
                    Picture = "https://upload.wikimedia.org/wikipedia/commons/thumb/1/1b/Batman_%28black_background%29.jpg/800px-Batman_%28black_background%29.jpg",
                    MovieList = new List<Movie>()
                },
                new Character
                {
                    FullName = "Selina Kyle",
                    Alias = "Catwoman",
                    Gender = "female",
                    Picture = "https://ocdn.eu/pulscms-transforms/1/R1ak9kuTURBXy80MDA5OTk2OC0zNTVmLTRmMzQtOWYzYy04NmE3MTRjMzMyNDQuanBlZ5GTBc0DFM0BvIGhMAU",
                    MovieList = new List<Movie>()
                },
                    new Character
                {
                    FullName = "Peter Benjamin Parker",
                    Alias = "Spiderman",
                    Gender = "male",
                    Picture = "https://www.sideshow.com/storage/product-images/907439/spider-man-classic-suit_marvel_silo.png",
                    MovieList = new List<Movie>()
                }
            };
        }
        /// <summary>
        /// A private method for first time franchises seed to database
        /// </summary>
        private ICollection<Franchise> CreateFranchises()
        {
            return new List<Franchise>
            {
                 new Franchise
                {
                    Name = "Warner Brothers",
                    Description = "an American diversified multinational mass media and entertainment conglomerate headquartered at the Warner Bros. Studios complex in Burbank, California, and a subsidiary of AT&T's WarnerMedia through its Studios & Networks division. Founded in 1923 by four brothers Harry, Albert (Abe), Sam, and Jack Warner, the company established itself as a leader in the American film industry before diversifying into animation, television, and video games, and is one of the Big Five major American film studios, as well as a member of the Motion Picture Association (MPA)."
                },
                new Franchise
                {
                    Name = "Marvel Cinematic Universe",
                    Description = "an American media franchise and shared universe centered on a series of superhero films produced by Marvel Studios. The films are based on characters that appear in American comic books published by Marvel Comics. The franchise also includes television series, short films, digital series, and literature. The shared universe, much like the original Marvel Universe in comic books, was established by crossing over common plot elements, settings, cast, and characters."
                },
                new Franchise
                {
                    Name = "Universal Pictures",
                    Description = "an American film production and distribution company owned by Comcast through the NBCUniversal Film and Entertainment division of NBCUniversal The films are based on characters that appear in American comic books published by Marvel Comics. The franchise also includes television series, short films, digital series, and literature. The shared universe, much like the original Marvel Universe in comic books, was established by crossing over common plot elements, settings, cast, and characters."
                }
            };       
        }
        /// <summary>
        /// A private method for first time movies seed to database
        /// </summary>
        private ICollection<Movie> CreateMovies()
        {
            return new List<Movie>
            {
                   new Movie
                {
                    MovieTitle = "Batman Returns",
                    Genre = "Scfi",
                    ReleaseYear = 1992,
                    Director = "Tim Burton",
                    Picture = "https://i.pinimg.com/564x/ab/bc/62/abbc62393070b84ad118de02f76aafc7.jpg",
                    Trailer = "https://www.youtube.com/watch?v=Too3qgNaYBE"
                },
                new Movie
                {
                    MovieTitle = "Batman",
                    Genre = "Scfi",
                    ReleaseYear = 1989,
                    Director = "Tim Burton",
                    Picture = "https://static.wikia.nocookie.net/batman/images/b/bd/Batman_89.png",
                    Trailer = "https://www.youtube.com/watch?v=dgC9Q0uhX70"
                },
                new Movie
                {
                    MovieTitle = "Spiderman",
                    Genre = "Scfi",
                    ReleaseYear = 2002,
                    Director = "Sam Raimi",
                    Picture = "https://upload.wikimedia.org/wikipedia/en/f/f3/Spider-Man2002Poster.jpg",
                    Trailer = "https://www.youtube.com/watch?v=TYMMOjBUPMM"
                }
            };
        }
    }
}
