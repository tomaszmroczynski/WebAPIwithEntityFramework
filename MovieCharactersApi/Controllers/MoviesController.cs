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
        /// <summary>
        /// A controller that returns all movies in database
        /// </summary>
        /// <returns>All movies</returns>
        [HttpGet]
        [Route("getmovies")]
        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            var movies = await _service.GetMovies();
            var movieList = movies.ToList();
            return movieList.Select(x => _mapper.Map<MovieDTO>(x));
        }
        /// <summary>
        /// A controller that returns movie by selected id
        /// </summary>
        /// <param name="id">Movie id to be shown</param>
        /// <returns>Movie</returns>
        [HttpGet]
        [Route("getmoviebyid")]
        public async Task<IActionResult> GetMovieAsync([FromQuery] int id)
        {
                var movie = await _service.GetMovieById(id);
            if (movie != null)
            {
                using (ManualMapping mapping = new ManualMapping(_context, _mapper))
                {
                    var MovieDTO = mapping.MapMovieToDTO(movie);
                    return Ok(MovieDTO);
                }
            }
            else
                return NotFound();

        }
        /// <summary>
        /// A controller that create movie instance in database
        /// </summary>
        /// <param name="createMovieDTO">Data transfer object representation of movie instance </param>
        /// <returns>Movie created report</returns>
        [HttpPost]
        [Route("createmovie")]
        public async Task<IActionResult> CreateMovie(CreateMovieDTO createMovieDTO)
        {
           
            using (ManualMapping mapping = new ManualMapping(_context, _mapper))
            {
                var movie = mapping.MapDTOtoMovie(createMovieDTO);
                var createdMovie = await _service.CreateMovie(movie);
                if (createdMovie != null)
                    return Ok("Movie was created");
                else
                    return NotFound();
            }
        }
        /// <summary>
        /// A controller that updatdes movie in database
        /// </summary>
        /// <param name="updateMovieDTO">Data transfer object representation of movie instance </param>
        /// <returns>Updating report</returns>
        [HttpPut]
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

        /// <summary>
        /// A controller that deletes movie from database
        /// </summary>
        /// <param name="id">Id of movie to be deleted</param>
        /// <returns>Deleting report</returns>
        [HttpDelete]
        [Route("deletemovie")]
        public async Task<IActionResult> DeleteMovie([FromQuery] int id)
        {
                bool result = await _service.DeleteMovie(id);
                if (result)
                    return Ok($"Movie, id = {id} was deleted");
                else
                    return NotFound();
        }

    }
}
