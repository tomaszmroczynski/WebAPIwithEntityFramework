using Microsoft.AspNetCore.Mvc;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Controllers
{
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly IMovieService _service;

        public CharacterController(IMovieService service)
        {           
            _service = service;
        }

        [HttpGet]
        [Route("getcharacter")]
        public async Task<CharacterDTO> GetCharacterByIdAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.GetCharacterById(number);
            }
            else
                return null;
        }

        [HttpGet]
        [Route("getcharacters")]
        public async Task<IEnumerable<CharacterDTO>> GetCharactersAsync()
        {
            return await _service.GetCharacters();           
        }

        [HttpPost]
        [Route("createcharacter")]
        public async Task<string> CreateCharacterAsync(CreateCharacterDTO characterDTO)
        {
            var character = await _service.CreateCharacter(characterDTO);
            if (character != null)
                return "Character was created successfully";
            else
                return "Operation unsuccesfull";
        }

        [HttpPut]
        [Route("updatecharacter")]
        public async Task<string> UpdateCharacterAsync(UpdateCharacterDTO updateCharacterDTO)
        {
            return await _service.UpdateCharacter(updateCharacterDTO);
        }

        [HttpDelete]
        [Route("deletecharacter")]
        public async Task<string> DeleteCharacterAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.DeleteCharacter(number);
            }
            else
                return "Operation unsuccesfull - incorect character id";
        }
    }
}
