using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    public class UpdateMovieDTO
    {
        public int Id { get; set; }

        public string MovieTitle { get; set; }

        public string Genre { get; set; }

        public int ReleaseYear { get; set; }

        public string Director { get; set; }

        public string Picture { get; set; }

        public string Trailer { get; set; }
        public List<int> CharacterIdList { get; set; }
    }
}
