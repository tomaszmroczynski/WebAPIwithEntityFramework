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
using MovieCharactersApi.Models;
using MovieCharactersApi.Service;

namespace MovieCharactersApi.Controllers
{

    [ApiController]
    public class VariableActionsController : ControllerBase
    {
        private readonly IMovieService _service;
        private readonly MovieCharacterContext _context;
        private readonly IMapper _mapper;

        public VariableActionsController(IMovieService service, MovieCharacterContext context, IMapper mapper)
        {
            _service = service;
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// A controller that seeds database for first time
        /// </summary>
        /// <returns>Seeding report</returns>
        [HttpPost]
        [Route("seed")]
        public async Task<IActionResult> SeedDataAsync()
        {
            await _service.SeedData();
            return Ok("Data seeded succesfully");
        }

        /// <summary>
        /// A controller that updates characters in a movie
        /// </summary>
        /// <param name="movieId">Movie id to be updated</param>
        /// <param name="charactersId">Array of characters Ids</param>
        /// <returns>Updatig report</returns>
        [HttpPut]
        [Route("updatecharactersinmovie")]
        public async Task<IActionResult> UpdateCharactersInMovie(int[] charactersId, [FromQuery] int movieId)
        {
             await _service.UpdateCharactersInMovie(movieId, charactersId);
             return Ok($"Character in movie, id = {movieId} was updated succesfuy");
        }
        /// <summary>
        /// A controller that updates movies in a franchise
        /// </summary>
        /// <param name="franchiseId">Franchise id to be updated</param>
        /// <param name="moviesId">Array of movie Ids</param>
        /// <returns>Updatig report</returns>
        [HttpPut]
        [Route("updatemovieinfranchise")]
        public async Task<IActionResult> UpdateMovieInFranchise(int[] moviesId, [FromQuery] int franchiseId)
        {
            await _service.UpdateMoviesInFranchise(franchiseId, moviesId);
            return Ok($"Movies in franchise, id = {franchiseId} was updated succesfuy");
        }

        /// <summary>
        /// A controller that show all characters in selected movie
        /// </summary>
        /// <param name="movieId">Id of selected movie</param>
        /// <returns>All characters</returns>
        [HttpGet]
        [Route("getallcharactersinmovie")]
        public async Task<IEnumerable<CharacterDTO>> GetCharactersInMovieAsync([FromQuery] int movieId)
        {            
            
            var characters = await _service.GetCharactersInMovie(movieId);
            var characterList = characters.ToList();
            return characterList.Select(x => _mapper.Map<CharacterDTO>(x));
        }
        /// <summary>
        /// A controller that show all movies in franchise
        /// </summary>
        /// <param name="franchiseId">Id of selected franchise</param>
        /// <returns>All movies</returns>
        [HttpGet]
        [Route("getallmoviesinfranchise")]
        public async Task<IEnumerable<MovieDTO>> GetMoviesInFranchiseAsync([FromQuery] int franchiseId)
        {           
            var movies = await _service.GetMoviesInFranchise(franchiseId);
            var movieList = movies.ToList();
            return movieList.Select(x => _mapper.Map<MovieDTO>(x));
        }
    }
}
