﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Favorite
{
    public class FavoriteDto
    {
        public int  Id { get; set; }
        public int  UserId { get; set; }
        public int RecipeId { get; set; }
    }
}
