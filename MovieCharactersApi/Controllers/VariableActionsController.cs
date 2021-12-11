using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieCharactersApi.Database;
using MovieCharactersApi.Models;
using MovieCharactersApi.Service;

namespace MovieCharactersApi.Controllers
{

    [ApiController]
    public class VariableActionsController : ControllerBase
    {
        private readonly IMovieService _service;

        public VariableActionsController(IMovieService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("seed")]
        public async Task<string> SeedDataAsync()
        {
            await _service.SeedData();
            return "data has been created";
        }
    }
}
