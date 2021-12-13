using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.Models
{
    /// <summary>
    /// A class that contains character database model
    /// </summary>
    public class Character
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Alias { get; set; }

        public string Gender { get; set; }

        public string Picture { get; set; }

        public ICollection<Movie> MovieList { get; set; }
    }
}
