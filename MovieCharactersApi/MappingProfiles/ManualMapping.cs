using AutoMapper;
using MovieCharactersApi.Database;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.MappingProfiles
{
    /// <summary>
    /// A class hat contains all neccessary manual DTO mapping methods
    /// </summary>
    public class ManualMapping : IDisposable
    {
        private  MovieCharacterContext _context;
        private readonly IMapper _mapper;

        public ManualMapping(MovieCharacterContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Character MapDTOtoCharacter(CreateCharacterDTO dto)
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

        public Franchise MapDTOToFranchise(CreateFranchiseDTO dto)
        {
            var franchise = new Franchise
            {
                Name = dto.Name,
                Description = dto.Description,

            };

            var movies = _context.Movies.Where(x => dto.MovieIdList.Contains(x.Id));
            dto.MovieIdList = movies.Select(x => x.Id).ToList();
            return franchise;
        }

        public Movie MapDTOtoMovie(CreateMovieDTO dto)
        {
            var movie = new Movie
            {
                MovieTitle = dto.MovieTitle,
                Genre= dto.Genre,
                ReleaseYear = dto.ReleaseYear,
                Director = dto.Director,
                Picture = dto.Picture,
                Trailer = dto.Trailer
            };

            var characters = _context.Characters.Where(x => dto.CharacterIdList.Contains(x.Id));
            movie.CharacterList = characters.ToList();
            return movie;
        }

        public CharacterDTO MapCharacterToDTO(Character character)
        {
            var dto = new CharacterDTO
            {
                Id = character.Id,
                FullName = character.FullName,
                Alias = character.Alias,
                Gender = character.Gender,
                Picture = character.Picture,                       
            };
            var movies = character.MovieList.Select(m => _mapper.Map<MovieDTO>(m));
            dto.MovieList = movies.ToList();
            return dto;
        }

        public FranchiseDTO MapFranchiseToDTO(Franchise franchise)
        {
            var dto = new FranchiseDTO
            {
                Id = franchise.Id,
                Name = franchise.Name,
                Description = franchise.Description,

            };
            //var movies = character.MovieList.Select(m => _mapper.Map<MovieDTO>(m));
            //dto.MovieList = movies.ToList();
            return dto;
        }

        public MovieDTO MapMovieToDTO(Movie movie)
        {
            var dto = new MovieDTO
            {
                MovieTitle = movie.MovieTitle,
                Genre = movie.Genre,
                ReleaseYear = movie.ReleaseYear,
                Director = movie.Director,
                Picture = movie.Picture,
                Trailer = movie.Trailer
            };
            //var characters = movie.CharacterList.Select(m => _mapper.Map<CharacterDTO>(m));
            //dto.CharacterList= characters.ToList();
            return dto;
        }
        public void Dispose()
        {
           
        }
    }
}
