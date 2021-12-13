using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MovieCharactersApi.Database;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.MappingProfiles;
using MovieCharactersApi.Models;
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
        private readonly MovieCharacterContext _context;
        private readonly IMapper _mapper;

        public CharacterController(IMovieService service, MovieCharacterContext context, IMapper mapper)
        {           
            _service = service;
            _context = context;
            _mapper = mapper;
        }
        /// <summary>
        /// A controller that returns character by selected id
        /// </summary>
        /// <param name="id">Character id to be shown</param>
        /// <returns>Character</returns>
        [HttpGet]
        [Route("getcharacter")]
        public async Task<IActionResult> GetCharacterByIdAsync([FromQuery] int id)
        {
            var character = await _service.GetCharacterById(id);
            if (character != null)
            {
                using (ManualMapping mapping = new ManualMapping(_context, _mapper))
                {
                    var characterDTO = mapping.MapCharacterToDTO(character);
                    return Ok(characterDTO);
                }
            }
            else
                return
                    NotFound();

        }
        /// <summary>
        /// A controller that returns all characters from database
        /// </summary>
        /// <returns>All characters</returns>
        [HttpGet]
        [Route("getcharacters")]
        public async Task<IEnumerable<CharacterDTO>> GetCharactersAsync()
        {
            var characters = await _service.GetCharacters();
            var characterList = characters.ToList();
            return characterList.Select(x => _mapper.Map<CharacterDTO>(x));           
        }
        /// <summary>
        /// A controller that create character instance in database
        /// </summary>
        /// <param name="characterDTO">Data transfer object representation of character instance </param>
        /// <returns>Character created report</returns>
        [HttpPost]
        [Route("createcharacter")]
        public async Task<IActionResult> CreateCharacterAsync(CreateCharacterDTO characterDTO)
        {
            Character character;
            using (ManualMapping mapping = new ManualMapping(_context, _mapper))
            {
                character = mapping.MapDTOtoCharacter(characterDTO);
            } 
            var createdCharacter = await _service.CreateCharacter(character);

            if (createdCharacter != null)
                return Ok("Character was created");
            else
                return NotFound();
        }

        /// <summary>
        /// A controller that updatdes character in database
        /// </summary>
        /// <param name="updateCharacterDTO">Data transfer object representation of character instance </param>
        /// <returns>Updating report</returns>
        [HttpPut]
        [Route("updatecharacter")]
        public async Task<IActionResult> UpdateCharacterAsync(UpdateCharacterDTO updateCharacterDTO)
        {
            Character character = new Character
            {
                Id = updateCharacterDTO.Id,
                FullName = updateCharacterDTO.FullName,
                Alias = updateCharacterDTO.Alias,
                Gender = updateCharacterDTO.Gender,
                Picture = updateCharacterDTO.Picture,
            };

            Character updatedCharacter;
            if (updateCharacterDTO.MovieIdList.Any())
            {
                character.MovieList = _context.Movies.
                    Where(x => updateCharacterDTO.MovieIdList.Contains(x.Id)).ToList();

                updatedCharacter = await _service.UpdateCharacter(character);
                if (updatedCharacter != null)
                    return Ok(updatedCharacter);
                else
                    NotFound();
            }
            else
            {
                character.MovieList = new List<Movie>();
                updatedCharacter = await _service.UpdateCharacter(character);
                if (updatedCharacter != null)
                    return Ok(updatedCharacter);
                else
                    NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// A controller that deletes character from database
        /// </summary>
        /// <param name="id">Id of character to be deleted</param>
        /// <returns>Deleting report</returns>
        [HttpDelete]
        [Route("deletecharacter")]
        public async Task<IActionResult> DeleteCharacterAsync([FromQuery] int id)
        {
                bool result = await _service.DeleteCharacter(id);
                if (result)
                    return Ok($"Character, id = {id} was deleted");
                else
                    return NotFound();
        }

    }
}
