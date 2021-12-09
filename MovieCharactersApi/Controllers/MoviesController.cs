using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _service;

        public MoviesController(IMovieService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getmovies")]
        public async Task<IEnumerable<MovieDTO>> GetMovies()
        {
            return await _service.GetMovies();
        }

        [HttpGet]
        [Route("getmoviebyid")]
        public async Task<MovieDTO> GetMovieAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.GetMovieById(number);
            }
            else
                return null;       
        }

        [HttpPut]
        [Route("createmovie")]
        public async Task<string> CreateMovie(CreateMovieDTO createMovieDTO)
        {
            var character = await _service.CreateMovie(createMovieDTO);
            if (character != null)
                return "Character was created successfully";
            else
                return "Operation unsuccesfull";
        }

 
        [HttpPost]
        [Route("updatemovie")]
        public async Task<string> UpdateMovieAsync(UpdateMovieDTO updateMovieDTO)
        {
            return await _service.UpdateMovie(updateMovieDTO);
        }

        [HttpDelete]
        [Route("deletemovie")]
        public async Task<string> DeleteMovie([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.DeleteMovie(number);
            }
            else
                return "Operation unsuccesfull - incorect movie id";
        }
    }
}
