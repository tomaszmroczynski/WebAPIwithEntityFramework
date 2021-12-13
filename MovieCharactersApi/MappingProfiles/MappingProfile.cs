using AutoMapper;
using MovieCharactersApi.DTOs;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.MappingProfiles
{
    /// <summary>
    /// A classs that contains automatic mapping profiles
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Character, CharacterDTO>();
            CreateMap<Movie, MovieDTO>();
            CreateMap<Franchise, FranchiseDTO>();
        }
    }
}
