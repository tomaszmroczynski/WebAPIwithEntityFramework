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
        public class FranchisesController : ControllerBase 
        { 
            private readonly IMovieService _service;

        public FranchisesController(IMovieService service)
        {
            _service = service;
        }


        [HttpGet]
        [Route("getfranchises")]
        public async Task<IEnumerable<FranchiseDTO>> GetFranchisesAsync()
        {
            return await _service.GetFranchises();
        }

        [HttpGet]
        [Route("getfranchisebyid")]
        public async Task<FranchiseDTO> GetFranchiseAsync([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.GetFranchiseById(number);
            }
            else
                return null;
        }

        [HttpPut]
        [Route("createfranchise")]
        public async Task<string> CreateFranchise(CreateFranchiseDTO createFranchiseDTO)
        {
            var character = await _service.CreateFranchise(createFranchiseDTO);
            if (character != null)
                return "Franchise was created successfully";
            else
                return "Operation unsuccesfull";
        }

        [HttpPost]
        [Route("updatefranchise")]
        public async Task<string> UpdateFranchiseAsync(UpdateFranchiseDTO updateFranchiseDTO)
        {
            return await _service.UpdateFranchise(updateFranchiseDTO);
        }

        [HttpDelete]
        [Route("deletefranchise")]
        public async Task<string> DeleteFranchise([FromQuery] string id)
        {
            bool correctId = int.TryParse(id, out int number);
            if (correctId)
            {
                return await _service.DeleteFranchise(number);
            }
            else
                return "Operation unsuccesfull - incorect franchise id";
        }
    }
}
