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
        public async Task<Character> CreateCharacter(CreateCharacterDTO characterDTO)
        {
            try
            {
                var character = MapDTOtoCharacter(characterDTO);
                await _context.AddAsync(character);
                await _context.SaveChangesAsync();
                return character;
            }
            catch (Exception)
            {
                return null;
            }          
        }

        public async Task<Movie> CreateMovie(CreateMovieDTO createMovieDTO)
        {
            try
            {
                var movie = MapDTOtoMovie(createMovieDTO);
                await _context.AddAsync(movie);
                await _context.SaveChangesAsync();
                return movie;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Franchise> CreateFranchise(CreateFranchiseDTO dto)
        {
            try
            {
                var franchise = MapDTOtoFranchise(dto);
                await _context.AddAsync(franchise);
                await _context.SaveChangesAsync();
                return franchise;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<string> DeleteCharacter(int id)
        {
            var character = await _context.Characters.SingleOrDefaultAsync(c => c.Id == id);
            if (character != null)
            {
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();
                return "Character was deleted succesfully";
            }
            else
                return "Character not found";
            
        }

        public async Task<string> DeleteMovie(int id)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(c => c.Id == id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
                await _context.SaveChangesAsync();
                return "Movie was deleted succesfully";
            }
            else
                return "Movie not found";

        }

        public async Task<string> DeleteFranchise(int id)
        {
            var franchise = await _context.Franchises.SingleOrDefaultAsync(c => c.Id == id);
            if (franchise != null)
            {
                _context.Franchises.Remove(franchise);
                await _context.SaveChangesAsync();
                return "Franchise was deleted succesfully";
            }
            else
                return "Franchise not found";

        }

        public async Task<CharacterDTO> GetCharacterById(int id)
        {
            var character = await _context.Characters.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);
            return  _mapper.Map<CharacterDTO>(character);
        }

        public async Task<MovieDTO> GetMovieById(int id)
        {
            var movie = await _context.Movies.Include(c => c.CharacterList).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<MovieDTO>(movie);
        }

        public async Task<FranchiseDTO> GetFranchiseById(int id)
        {
            var movie = await _context.Franchises.Include(c => c.MovieList).FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<FranchiseDTO>(movie);
        }


        public async Task<IEnumerable<CharacterDTO>> GetCharacters()
        {
            return await _context.Characters.Include(c => c.MovieList)
                .Select(c => _mapper.Map<CharacterDTO>(c)).ToListAsync();        
        }

        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            return await _context.Movies.Include(c => c.CharacterList)
                .Select(c => _mapper.Map<MovieDTO>(c)).ToListAsync();
        }
        public async Task<IEnumerable<FranchiseDTO>> GetFranchises()
        {
            return await _context.Franchises.Include(c => c.MovieList)
                .Select(c => _mapper.Map<FranchiseDTO>(c)).ToListAsync();
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

        public async Task<string> UpdateCharacter(UpdateCharacterDTO updateCharacterDTO)
        {
            var character = await _context.Characters.SingleOrDefaultAsync(c => c.Id == updateCharacterDTO.Id);
            if (character != null)
            {
                character.FullName = updateCharacterDTO.FullName;
                character.Alias = updateCharacterDTO.Alias;
                character.Gender = updateCharacterDTO.Gender;
                character.Picture = updateCharacterDTO.Picture;
                if (updateCharacterDTO.MovieIdList.Any())
                {
                    character.MovieList = await _context.Movies.
                        Where(x => updateCharacterDTO.MovieIdList.Contains(x.Id)).ToListAsync();
                }
                await _context.SaveChangesAsync();
                return "Character was updated succesfully";
            }
            else
                return "Incorrect character Id";
        }

        public async Task<string> UpdateMovie(UpdateMovieDTO dto)
        {
            var movie = await _context.Movies.SingleOrDefaultAsync(c => c.Id == dto.Id);
            if (movie != null)
            {
                movie.MovieTitle = dto.MovieTitle;
                movie.Genre = dto.Genre;
                movie.ReleaseYear = dto.ReleaseYear;
                movie.Director = dto.Director;
                movie.Picture = dto.Picture;
                movie.Trailer = dto.Trailer;

                if (dto.CharacterIdList.Any())
                {
                    movie.CharacterList = await _context.Characters.
                        Where(x => dto.CharacterIdList.Contains(x.Id)).ToListAsync();
                }
                await _context.SaveChangesAsync();
                return "Movie was updated succesfully";
            }
            else
                return "Incorrect movie Id";
        }

        public async Task<string> UpdateFranchise(UpdateFranchiseDTO dto)
        {
            var franchise = await _context.Franchises.SingleOrDefaultAsync(c => c.Id == dto.Id);
            if (franchise != null)
            {
                franchise.Name = dto.Name;
                franchise.Description = dto.Description;


                if (dto.MovieIdList.Any())
                {
                    franchise.MovieList = await _context.Movies.
                        Where(x => dto.MovieIdList.Contains(x.Id)).ToListAsync();
                }
                await _context.SaveChangesAsync();
                return "Franchise was updated succesfully";
            }
            else
                return "Incorrect franchise Id";
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

        private Character MapDTOtoCharacter(CreateCharacterDTO dto)
        {
            var character = new Character
            {
                FullName = dto.FullName,
                Alias = dto.Alias,
                Gender = dto.Gender,
                Picture = dto.Picture
            };

            var movies = _context.Movies.Where(x => dto.MovieIdList.Contains(x.Id));
            character.MovieList = movies.ToList();
            return character;
        }

        private Movie MapDTOtoMovie(CreateMovieDTO dto)
        {

        var movie = new Movie
            {
                MovieTitle = dto.MovieTitle,
                Genre = dto.Genre,
                ReleaseYear = dto.ReleaseYear,
                Director = dto.Director,
                Picture = dto.Picture,
                Trailer = dto.Trailer
        };

            var characters = _context.Characters.Where(x => dto.CharacterIdList.Contains(x.Id));
            movie.CharacterList = characters.ToList();
            return movie;
        }

        private Franchise MapDTOtoFranchise(CreateFranchiseDTO dto)
        {

            var franchise = new Franchise
            {
                Name = dto.Name,
                Description = dto.Description,

            };

            var movies = _context.Movies.Where(x => dto.MovieIdList.Contains(x.Id));
            franchise.MovieList = movies.ToList();
            return franchise;
        }
    }
}
