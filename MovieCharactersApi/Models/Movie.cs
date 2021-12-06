using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Models
{
    public class Movie
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string MovieTitle { get; set; }
        
        [Required]
        [MaxLength(200)]
        public string Genre { get; set; }

        [Required]
        public int ReleaseYear { get; set; }

        [Required]
        [MaxLength(100)]
        public string Director { get; set; }

        public string Picture { get; set; }

        public string Trailer { get; set; }

        public ICollection<Character> CharacterList { get; set; }



    }
}
