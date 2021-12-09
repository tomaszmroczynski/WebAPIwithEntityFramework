using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    public class CreateFranchiseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<int> MovieIdList { get; set; }
    }
}
