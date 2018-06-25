﻿using System.ComponentModel.DataAnnotations;

namespace Exebite.API.Models
{
    public class UpdateMealModel
    {
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
    }
}
