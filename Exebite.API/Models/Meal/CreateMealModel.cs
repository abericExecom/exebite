﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Exebite.API.Models
{
    public class CreateMealModel
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public List<FoodModel> Foods { get; set; }
    }
}
