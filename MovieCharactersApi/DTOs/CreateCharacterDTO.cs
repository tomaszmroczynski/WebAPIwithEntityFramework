using AutoMapper;
using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    /// <summary>
    /// Data transfer object repersentation of Character  instance used to create character in database
    /// </summary>
    public class CreateCharacterDTO
    {
        public string FullName { get; set; }

        public string Alias { get; set; }

        public string Gender { get; set; }

        public string Picture { get; set; }

        public List<int> MovieIdList { get; set; }

    }
}
