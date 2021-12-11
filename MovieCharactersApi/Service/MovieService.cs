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
    public class MovieService : IMovieService
    {
        
        private readonly MovieCharacterContext _context;
        private readonly IMapper _mapper;

        public MovieService(MovieCharacterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
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

        public async Task<Character> GetCharacterById(int id)
        {
            return await _context.Characters.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);          
        }

        public async Task<Movie> GetMovieById(int id)
        {
            return await _context.Movies.Include(c => c.CharacterList).FirstOrDefaultAsync(c => c.Id == id);
            
        }

        public async Task<Franchise> GetFranchiseById(int id)
        {
            return await _context.Franchises.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);
         
        }


        public async Task<IEnumerable<Character>> GetCharacters()
        {
            return await _context.Characters.Include(c => c.MovieList).ToListAsync();               
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _context.Movies.Include(c => c.CharacterList).ToListAsync();
        }
        public async Task<IEnumerable<Franchise>> GetFranchises()
        {
            return await _context.Franchises.Include(c => c.MovieList).ToListAsync();
          
        }

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

        public async Task<string> UpdateCharactersInMovie(int movieId, int[] charactersId)
        {
            
            var movie = await _context.Movies.FirstOrDefaultAsync(c => c.Id == movieId);
            if (movie != null)
            {
                Character character = new Character();
                foreach (var item in charactersId)
                {
                    character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == item);
                    if (!movie.CharacterList.Contains(character))
                    {
                        movie.CharacterList.Add(character);                       
                    }
                }
                return "characters was updated succesfully";
            } 
            else
            {
                return "incorrect movie id";
            }
        }

        public async Task<string> UpdateMovieInFranchise(int franchiseId, int[] moviesId)
        {
            var franchise = await _context.Franchises.FirstOrDefaultAsync(c => c.Id == franchiseId);
            if (franchise != null)
            {
                Movie movie = new Movie();
                foreach (var item in moviesId)
                {
                    movie = await _context.Movies.FirstOrDefaultAsync(c => c.Id == item);
                    if (!franchise.MovieList.Contains(movie))
                    {
                        franchise.MovieList.Add(movie);
                    }
                }
                return "movies in franchise was updated succesfully";
            }
            else
            {
                return "incorrect franchise id";
            }
        }

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
