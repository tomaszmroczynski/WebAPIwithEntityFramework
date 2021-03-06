using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    /// <summary>
    /// Data transfer object repersentation of Franchise  instance used to update franchise in database
    /// </summary>
    public class UpdateFranchiseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> MovieIdList { get; set; }
    }
}
