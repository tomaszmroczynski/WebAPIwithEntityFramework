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
        public class FranchisesController : ControllerBase 
        {
            private readonly IMovieService _service;
            private readonly IMapper _mapper;
            private readonly MovieCharacterContext _context;

        public FranchisesController(IMovieService service, IMapper mapper, MovieCharacterContext context)
        {
            _service = service;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// A controller that returns all franchises in database
        /// </summary>
        /// <returns>All franchises</returns>
        [HttpGet]
        [Route("getfranchises")]
        public async Task<IEnumerable<FranchiseDTO>> GetFranchisesAsync()
        {
            var franchises = await _service.GetFranchises();
            var franchiseList = franchises.ToList();
            return franchiseList.Select(x => _mapper.Map<FranchiseDTO>(x));

        }
        /// <summary>
        /// A controller that returns franchise by selected id
        /// </summary>
        /// <param name="id">Franchise id to be shown</param>
        /// <returns>Franchise</returns>
        [HttpGet]
        [Route("getfranchisebyid")]
        public async Task<IActionResult> GetFranchiseAsync([FromQuery] int id)
        {
            var franchise = await _service.GetFranchiseById(id);
            if (franchise != null)
            {
                
                using (ManualMapping mapping = new ManualMapping(_context, _mapper))
                {
                    var franchiseDTO = mapping.MapFranchiseToDTO(franchise);
                    return Ok(franchiseDTO);
                }
            }
            else
                return NotFound();
        }
        /// <summary>
        /// A controller that create franchise instance in database
        /// </summary>
        /// <param name="createFranchiseDTO">Data transfer object representation of franchise instance </param>
        /// <returns>Franchise created report</returns>
        [HttpPost]
        [Route("createfranchise")]
        public async Task<IActionResult> CreateFranchise(CreateFranchiseDTO createFranchiseDTO)
        {
            Franchise franchise;
            using (ManualMapping mapping = new ManualMapping(_context, _mapper))
            {
                franchise = mapping.MapDTOToFranchise(createFranchiseDTO);
            }
            var createdFranchise = await _service.CreateFranchise(franchise);

            if (createdFranchise != null)
                return Ok("Franchise was created");
            else
                return NotFound();
        }

        /// <summary>
        /// A controller that updatdes franchise in database
        /// </summary>
        /// <param name="updateFranchiseDTO">Data transfer object representation of franchise instance </param>
        /// <returns>Updating report</returns>
        [HttpPut]
        [Route("updatefranchise")]
        public async Task<IActionResult> UpdateFranchiseAsync(UpdateFranchiseDTO updateFranchiseDTO)
        {
            Franchise franchise = new Franchise
            {
                Id = updateFranchiseDTO.Id,
                Name = updateFranchiseDTO.Name,
                Description = updateFranchiseDTO.Description,

            };

            Franchise updatedFranchise;
            if (updateFranchiseDTO.MovieIdList.Any())
            {
                franchise.MovieList = _context.Movies.
                    Where(x => updateFranchiseDTO.MovieIdList.Contains(x.Id)).ToList();

                updatedFranchise = await _service.UpdateFranchise(franchise);
                if (updatedFranchise != null)
                    return Ok(updatedFranchise);
                else
                    NotFound();
            }
            else
            {
                franchise.MovieList = new List<Movie>();
                updatedFranchise = await _service.UpdateFranchise(franchise);
                if (updatedFranchise != null)
                    return Ok(updatedFranchise);
                else
                    NotFound();
            }
            return Ok();
        }
        /// <summary>
        /// A controller that deletes franchise from database
        /// </summary>
        /// <param name="id">Id of franchise to be deleted</param>
        /// <returns>Deleting report</returns>
        [HttpDelete]
        [Route("deletefranchise")]
        public async Task<IActionResult> DeleteFranchise([FromQuery] int id)
        {
            bool result = await _service.DeleteFranchise(id);
            if (result)
                return Ok($"Franchise, id = {id} was deleted");
            else
                return NotFound();
        }
    }
}
