using Microsoft.AspNetCore.Mvc;
using MovieCharactersApi.SeedData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Controllers
{
    [ApiController]
    [Route("createdata")]
    public class SeedController : ControllerBase
    {
        private readonly ISeedService _service;

        public SeedController(ISeedService service)
        {           
            _service = service;
        }

        [HttpGet]
        public async Task<string> CreateData()
        {
            await _service.SeedCharacters();
            return "data has been created";
        }
    }
}
