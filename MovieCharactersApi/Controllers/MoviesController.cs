using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersApi.Database;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.MappingProfiles;
using MovieCharactersApi.Models;
using MovieCharactersApi.Service;

namespace MovieCharactersApi.Controllers
{   
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly IMapper _mapper;
        private readonly MovieCharacterContext _context;

        public MoviesController(IMovieService service, IMapper mapper, MovieCharacterContext context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [Route("getmovies")]
        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            var movies = await _service.GetMovies();
            var movieList = movies.ToList();
            return movieList.Select(x => _mapper.Map<MovieDTO>(x));

        }

        [HttpGet]
        [Route("getmoviebyid")]
        public async Task<IActionResult> GetMovieAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                var movie = await _service.GetMovieById(number);
                using (ManualMapping mapping = new ManualMapping(_context, _mapper))
                {
                    var MovieDTO = mapping.MapMovieToDTO(movie);
                    return Ok(MovieDTO);
                }
            }
            else
                return NotFound();
        }

        [HttpPut]
        [Route("createmovie")]
        public async Task<IActionResult> CreateMovieDTO(CreateMovieDTO createMovieDTO)
        {
            using (ManualMapping mapping = new ManualMapping(_context, _mapper))
            {
                var movie = mapping.MapDTOtoMovie(createMovieDTO);
                var createdMovie = await _service.CreateMovie(movie);
                if (createdMovie != null)
                    return Created(new Uri($"{Request.Path}/{createdMovie.Id}"), createdMovie);
                else
                    return NotFound();
            }

        }

        [HttpPost]
        [Route("updatemovie")]
        public async Task<IActionResult> UpdateMovieAsync(UpdateMovieDTO updateMovieDTO)
        {
            Movie movie = new Movie
            {
                Id = updateMovieDTO.Id,
                MovieTitle = updateMovieDTO.MovieTitle,
                Genre = updateMovieDTO.Genre,
                ReleaseYear = updateMovieDTO.ReleaseYear,
                Director = updateMovieDTO.Director,
                Picture = updateMovieDTO.Picture,
                Trailer = updateMovieDTO.Trailer,
        };

            Movie updatedMovie;
            if (updateMovieDTO.CharacterIdList.Any())
            {
                movie.CharacterList = _context.Characters.
                    Where(x => updateMovieDTO.CharacterIdList.Contains(x.Id)).ToList();

                updatedMovie = await _service.UpdateMovie(movie);
                if (updatedMovie != null)
                    return Ok(updatedMovie);
                else
                    NotFound();
            }
            else
            {
                movie.CharacterList = new List<Character>();
                updatedMovie = await _service.UpdateMovie(movie);
                if (updatedMovie != null)
                    return Ok(updatedMovie);
                else
                    NotFound();
            }
            return Ok();
        }


        [HttpDelete]
        [Route("deletemovie")]
        public async Task<IActionResult> DeleteMovie([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                bool result = await _service.DeleteMovie(number);
                if (result)
                    return Ok($"Movie, id = {id} was deleted");
                else
                    return NotFound();
            }
            else
                return NotFound();
        }

    }
}
