using MovieCharactersApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    /// <summary>
    /// Data transfer object repersentation of Character instance
    /// </summary>
    public class CharacterDTO
    {     
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Alias { get; set; }

        public string Gender { get; set; }

        public string Picture { get; set; }

        public ICollection<MovieDTO> MovieList { get; set; }
    }
}
