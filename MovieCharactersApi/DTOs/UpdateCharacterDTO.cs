﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieCharactersApi.DTOs
{
    public class UpdateCharacterDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Alias { get; set; }

        public string Gender { get; set; }

        public string Picture { get; set; }

        public List<int> MovieIdList { get; set; }

    }
}