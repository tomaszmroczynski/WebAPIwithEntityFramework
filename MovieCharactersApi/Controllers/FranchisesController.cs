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


        [HttpGet]
        [Route("getfranchises")]
        public async Task<IEnumerable<FranchiseDTO>> GetFranchisesAsync()
        {
            var franchises = await _service.GetFranchises();
            var franchiseList = franchises.ToList();
            return franchiseList.Select(x => _mapper.Map<FranchiseDTO>(x));

        }

        [HttpGet]
        [Route("getfranchisebyid")]
        public async Task<IActionResult> GetFranchiseAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                var franchise = await _service.GetFranchiseById(number);
                using (ManualMapping mapping = new ManualMapping(_context, _mapper))
                {
                    var franchiseDTO = mapping.MapFranchiseToDTO(franchise);
                    return Ok(franchiseDTO);
                }
            }
            else
                return NotFound();
        }

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
                return Created(new Uri($"{Request.Path}/{createdFranchise.Id}"), createdFranchise);
            else
                return NotFound();
        }
    

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

        [HttpDelete]
        [Route("deletefranchise")]
        public async Task<IActionResult> DeleteFranchise([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                bool result = await _service.DeleteFranchise(number);
                if (result)
                    return Ok($"Franchise, id = {id} was deleted");
                else
                    return NotFound();
            }
            else
                return NotFound();
        }
    }
}
